using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Configuration;
using Shop3.Application.Interfaces;
using Shop3.Application.ViewModels.Common;
using Shop3.Application.ViewModels.Products;
using Shop3.Data.Enums;
using Shop3.Extensions;
using Shop3.Models;
using Shop3.Models.BillViewModels;
using Shop3.Models.ProductViewModels;
using Shop3.Services;
using Shop3.Utilities.Constants;
using Shop3.Utilities.Extensions;

namespace Shop3.Controllers
{
    public class CartController : Controller
    {
        private readonly IProductService _productService;
        private readonly IBillService _billService;
        private readonly IConfiguration _configuration;
        private readonly IEmailSender _emailSender;
        private readonly IViewRenderService _viewRenderService;
        public CartController(IProductService productService, IEmailSender emailSender,
            IConfiguration configuration, IBillService billService, IViewRenderService viewRenderService)
        {
            _productService = productService;
            _billService = billService;
            _configuration = configuration;
            _emailSender = emailSender;
            _viewRenderService = viewRenderService;  // tạo ra 1 chuỗi html từ ViewName và model tương tự như Mustache truyền vào 1 template html và data => tạo ra view
                                                     // sử dụng cơ chế render của asp.net mvc để binding dữ liệu
        }

        [Route("cart.html", Name = "Cart")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("checkout.html", Name = "Checkout")]
        [HttpGet]
        public IActionResult Checkout()
        {
            var model = new CheckoutViewModel();
            var session = HttpContext.Session.Get<List<ShoppingCartViewModel>>(CommonConstants.CartSession);
            if (session == null)
            {
                return Redirect("/");
            }

            if (session.Any(x => x.Color == null || x.Size == null))
            {
                return Redirect("/cart.html");
            }

            model.Carts = session;
            return View(model);
        }

        [Route("checkout.html", Name = "Checkout")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Checkout(CheckoutViewModel model)
        {
            
            var session = HttpContext.Session.Get<List<ShoppingCartViewModel>>(CommonConstants.CartSession);
        
            if (ModelState.IsValid)
            {
                if (session != null)
                {
                    var total = session.Select(x => x.OrderTotal).Sum();
                    var details = new List<BillDetailViewModel>();
                    foreach (var item in session)
                    {
                        details.Add(new BillDetailViewModel()
                        {
                            Product = item.Product,
                            Price = item.Price,
                            ColorId = item.Color.Id,
                            SizeId = item.Size.Id,
                            Quantity = item.Quantity,
                            ProductId = item.Product.Id
                        });
                    }
                    var billViewModel = new BillViewModel()
                    {
                        CustomerMobile = model.CustomerMobile,
                        BillStatus = BillStatus.New,
                        CustomerAddress = model.CustomerAddress,
                        CustomerName = model.CustomerName,
                        CustomerMessage = model.CustomerMessage,
                        BillDetails = details,
                        CustomerEmail = model.CustomerEmail,
                        OrderTotal = session.Select(x => x.OrderTotal).Sum()
                    };
                    if (User.Identity.IsAuthenticated == true)
                    {
                        billViewModel.CustomerId = Guid.Parse(User.GetSpecificClaim("UserId"));
                    }
                    _billService.Create(billViewModel);
                    try
                    {

                        _billService.Save();

                        // sử dụng service ViewRender để tạo ra viewhtml : view to string
                       // string viewNameSendAdmin = "Cart/_BillMail";
                       // string subjectSendToAdmin = "New bill from Noname Shop";
                       // var content = await _viewRenderService.RenderToStringAsync(viewNameSendAdmin, billViewModel);
                      //  await _emailSender.SendEmailAsync(_configuration["MailSettings:AdminMail"], subjectSendToAdmin, content);
                       
                        //if (model.CustomerEmail != null )
                        //{
                        //   // string viewNameSendToUser = "Cart/_BillMailSendToUser";
                        //    //string subjectSendToUser = "Thank you for order at NonameShop";
                        //    //var contentSendToUser = await _viewRenderService.RenderToStringAsync(viewNameSendToUser, billViewModel);
                        //    //await _emailSender.SendEmailAsync(model.CustomerEmail, subjectSendToUser, content);
                        //}

                        HttpContext.Session.Remove(CommonConstants.CartSession); //remove sau khi đặt hàng thành công

                        ViewData["Success"] = true;
                    }
                    catch (Exception ex)
                    {
                        ViewData["Success"] = false;
                        ModelState.AddModelError("", ex.Message);
                    }

                }
            }
            model.Carts = session;
            return View(model);
        }

        [Route("wishlist.html")]
        public IActionResult WishListProduct()
        {


            if (User.Identity.IsAuthenticated == true)
            {

                return View();
            }
            else
            {
                return Redirect("/login.html");
            }
        }

        [Route("mydashboards.html")]
        public IActionResult Mydashboard()
        {


            if (User.Identity.IsAuthenticated == true)
            {

                return View();
            }
            else
            {
                return Redirect("/login.html");
            }
        }

        [Route("myorders.html")]
        public IActionResult MyOrders()
        {


            if (User.Identity.IsAuthenticated == true)
            {

                return View();
            }
            else
            {
                return Redirect("/login.html");
            }
        }


        #region AJAX Request

        // Get list item
        public IActionResult GetCart()
        {
            // use  get set session theo generic in SessionExtensions
            var session = HttpContext.Session.Get<List<ShoppingCartViewModel>>(CommonConstants.CartSession);
            if (session == null)
                session = new List<ShoppingCartViewModel>(); // tránh trường hợp null
            return new OkObjectResult(session);
        }


        // Remove all products in cart
        public IActionResult ClearCart()
        {
            // remove session hiện tại
            HttpContext.Session.Remove(CommonConstants.CartSession);
            return new OkObjectResult("OK");
        }

        /// <summary>
        /// Add product to cart
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AddToCart(int productId, int quantity, int color, int size)
        {
            //Get product detail
            var product = _productService.GetById(productId);

            //Get session with item list from cart
            var session = HttpContext.Session.Get<List<ShoppingCartViewModel>>(CommonConstants.CartSession);
            if (session != null)
            {
                //Convert string to list object
                bool hasChanged = false; // gắn cờ

                //Check exist with item product id
                if (session.Any(x => x.Product.Id == productId)) // chheck xem có sp chưa
                {
                    foreach (var item in session)
                    {
                        //Update quantity for product if match product id
                        if (item.Product.Id == productId)
                        {
                            item.Quantity += quantity; // nếu có rồi tăng sl lên 1
                            item.Price = product.PromotionPrice ?? product.Price; // nếu PromotionPrice khác null thì lấy km ko thì lấy Price
                            hasChanged = true;
                            item.OrderTotal += (quantity * item.Price);
                        }
                    }
                }
                else
                {
                    session.Add(new ShoppingCartViewModel()
                    {
                        Product = product,
                        Quantity = quantity,
                        Color = _billService.GetColor(color),
                        Size = _billService.GetSize(size),
                        Price = product.PromotionPrice ?? product.Price,
                        OrderTotal = (product.PromotionPrice > 0 ) ? (decimal)(product.PromotionPrice * quantity)  : (decimal)(product.Price * quantity)
                    });
                    hasChanged = true;
                }

                //Update back to cart
                if (hasChanged == true)
                {
                    HttpContext.Session.Set(CommonConstants.CartSession, session);
                }
            }
            else
            {
                //Add new cart
                var cart = new List<ShoppingCartViewModel>();
                cart.Add(new ShoppingCartViewModel()
                {
                    Product = product,
                    Quantity = quantity,
                    Color = _billService.GetColor(color),
                    Size = _billService.GetSize(size),
                    Price = product.PromotionPrice ?? product.Price,
                    OrderTotal = (product.PromotionPrice > 0) ? (decimal)(product.PromotionPrice * quantity) : (decimal)(product.Price * quantity)
                });
                HttpContext.Session.Set(CommonConstants.CartSession, cart);
            }
            return new OkObjectResult(productId);
        }

        /// <summary>
        /// Remove a product
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public IActionResult RemoveFromCart(int productId)
        {
            var session = HttpContext.Session.Get<List<ShoppingCartViewModel>>(CommonConstants.CartSession);
            if (session != null)
            {
                bool hasChanged = false; // gắn cờ
                foreach (var item in session)
                {
                    if (item.Product.Id == productId)
                    {
                        session.Remove(item);
                        hasChanged = true;
                        break;
                    }
                }
                if (hasChanged == true)
                {
                    HttpContext.Session.Set(CommonConstants.CartSession, session);
                }
                return new OkObjectResult(productId);
            }
            return new EmptyResult();
        }

        /// <summary>
        /// Update product quantity
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        public IActionResult UpdateCart(int productId, int quantity, int color, int size)
        {
            // todo error null color or size 
            var session = HttpContext.Session.Get<List<ShoppingCartViewModel>>(CommonConstants.CartSession);
            if (session != null)
            {
                bool hasChanged = false;
                foreach (var item in session)
                {
                    if (item.Product.Id == productId)
                    {
                        var product = _productService.GetById(productId);
                        item.Product = product;
                        item.Size = _billService.GetSize(size);
                        item.Color = _billService.GetColor(color);
                        item.Quantity = quantity;
                        item.Price = product.PromotionPrice ?? product.Price;
                        item.OrderTotal += (quantity * item.Price);
                        hasChanged = true;
                    }
                }
                if (hasChanged)
                {
                    HttpContext.Session.Set(CommonConstants.CartSession, session);
                }
                return new OkObjectResult(productId);
            }
            return new EmptyResult();
        }

        /// <summary>
        /// Add wish list product 
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AddWishList(int productId)
        {
            if (User.Identity.IsAuthenticated == true)
            {
                var user = Guid.Parse(User.GetSpecificClaim("UserId"));
                if (_productService.CheckWishProduct(productId, user) == false)
                {
                    var product = _productService.GetById(productId);
                    var wishproduct = new WishProductViewModel();
                    wishproduct.CustomerId = user;
                    wishproduct.ProductId = product.Id;
                    _productService.AddWish(wishproduct);

                    return new OkObjectResult(productId);
                }
                return Json(new { status = "error", message = "please login" });

            }

            return new BadRequestResult();


        }

        /// <summary>
        /// Get all wish list product paging
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult GetAllWishListPaging(int pageSize, int page = 1)
        {
            if (User.Identity.IsAuthenticated == true)
            {
                var user = Guid.Parse(User.GetSpecificClaim("UserId"));

                if (pageSize == 0)
                    pageSize = _configuration.GetValue<int>("PageSize");
                var listProduct = _productService.GetAllWishListPaging(user, page, pageSize);
                if (listProduct.RowCount < 0)
                {
                    return Json(new { status = "error", message = "no product in wish list" });
                }
                else
                {
                    return new OkObjectResult(listProduct);
                }

            }
            else
            {
                return Json(new { status = "error", message = "please login" });
            }

        }

        /// <summary>
        /// remove wish product 
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult RemovewishList(int productId)
        {

            if (ModelState.IsValid)
            {
                var user = Guid.Parse(User.GetSpecificClaim("UserId"));
                if (User.Identity.IsAuthenticated == true)
                {
                    _productService.DeleteWishProduct(productId, user);
                    return new OkObjectResult(productId);
                }
            }
            return new BadRequestResult();
        }

        /// <summary>
        /// Add all wish list product to cart
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AddAllWishProductToCart()
        {

            if (User.Identity.IsAuthenticated == true)
            {
                int quantity = 1;
                int color = 1;
                int size = 1;
                var user = Guid.Parse(User.GetSpecificClaim("UserId"));
                //Get product detail
                var listproduct = _productService.GetAllWishProduct(user);

                //Get session with item list from cart
                var session = HttpContext.Session.Get<List<ShoppingCartViewModel>>(CommonConstants.CartSession);
                if (session != null)
                {
                    //Convert string to list object
                    bool hasChanged = false; // gắn cờ
                    foreach (var product in listproduct)
                    {
                        //Check exist with item product id
                        if (session.Any(x => x.Product.Id == product.Id)) // chheck xem có sp chưa
                        {
                            foreach (var item in session)
                            {
                                //Update quantity for product if match product id
                                if (item.Product.Id == product.Id)
                                {
                                    item.Quantity += quantity; // nếu có rồi tăng sl lên 1
                                    item.Price = product.PromotionPrice ?? product.Price; // nếu PromotionPrice khác null thì lấy km ko thì lấy Price

                                }
                            }
                            hasChanged = true;
                        }
                        else
                        {
                            foreach (var item in session)
                            {
                                session.Add(new ShoppingCartViewModel()
                                {
                                    Product = product,
                                    Quantity = quantity,
                                    Color = _billService.GetColor(color),
                                    Size = _billService.GetSize(size),
                                    Price = product.PromotionPrice ?? product.Price
                                });

                            }
                            hasChanged = true;
                        }

                        //Update back to cart
                        if (hasChanged == true)
                        {
                            HttpContext.Session.Set(CommonConstants.CartSession, session);
                        }
                    }

                }
                else
                {
                    var cart = new List<ShoppingCartViewModel>();
                    foreach (var product in listproduct)
                    {
                        //Add new cart

                        cart.Add(new ShoppingCartViewModel()
                        {
                            Product = product,
                            Quantity = quantity,
                            Color = _billService.GetColor(color),
                            Size = _billService.GetSize(size),
                            Price = product.PromotionPrice ?? product.Price
                        });

                    }
                    HttpContext.Session.Set(CommonConstants.CartSession, cart);
                }
                return new OkObjectResult(quantity);
            }

            return new BadRequestResult();

        }

        [HttpPost]
        public IActionResult GetAllBillDetailPagingByUserId(int page, int pageSize)
        {
            if (User.Identity.IsAuthenticated == true)
            {
                var user = Guid.Parse(User.GetSpecificClaim("UserId"));
                if (pageSize == 0)
                    pageSize = _configuration.GetValue<int>("PageSize");
                var listProduct = _billService.GetAllPagingByCustomerId(user, page, pageSize);
                if (listProduct.RowCount < 0)
                {
                    return Json(new { status = "error", message = "no product in my Dashboard" });
                }
                else
                {
                    return new OkObjectResult(listProduct);
                }
            }
            return new BadRequestResult();
        }
        [HttpPost]
        public IActionResult GetAllBillPagingByUserId(int page, int pageSize)
        {
            if (User.Identity.IsAuthenticated == true)
            {
                
                var user = Guid.Parse(User.GetSpecificClaim("UserId"));
                if (pageSize == 0)
                    pageSize = _configuration.GetValue<int>("PageSize");
               
                var listProduct = _billService.GetBillByIdAndUserId(string.Empty, user, page, pageSize);
                if (listProduct.RowCount < 0)
                {
                    return Json(new { status = "error", message = "no product in my Dashboard" });
                }
                else
                {
                    return new OkObjectResult(listProduct);
                }
            }
            return new BadRequestResult();
        }

        public IActionResult GetAllBillCompeleteByUserId(int page, int pageSize)
        {
            if (User.Identity.IsAuthenticated == true)
            {
                var user = Guid.Parse(User.GetSpecificClaim("UserId"));
                if (pageSize == 0)
                    pageSize = _configuration.GetValue<int>("PageSize");
                var listProduct = _billService.GetBillByIdAndUserId(CommonConstants.BillCompeleted, user, page, pageSize);
                if (listProduct.RowCount < 0)
                {
                    return Json(new { status = "error", message = "no product in my Dashboard" });
                }
                else
                {
                    return new OkObjectResult(listProduct);
                }
            }
            return new BadRequestResult();
        }

        [HttpGet]
        public IActionResult GetBillById(int id)
        {
            if (User.Identity.IsAuthenticated == true)
            {
                var user = Guid.Parse(User.GetSpecificClaim("UserId"));
                var model = _billService.GetDetailByUser(id, user);
                return new OkObjectResult(model);

            }
            return new BadRequestResult();

        }

        [HttpPost]
        public IActionResult ReOrder(int id, string reOrderMesssage)
        {

            if (User.Identity.IsAuthenticated == true)
            {
                var user = Guid.Parse(User.GetSpecificClaim("UserId"));

                var check = _billService.CheckStatusBillWithUser(id, user);
                if (check == true)
                {
                    _billService.ReOderByUser(id, user, reOrderMesssage);
                    _billService.Save();
                    return new OkObjectResult(id);
                }
                return new BadRequestResult();


            }
            return new BadRequestResult();
        }

        [HttpGet]
        public IActionResult CheckStatus(int id, string reOrderMesssage)
        {

            if (User.Identity.IsAuthenticated == true)
            {
                var user = Guid.Parse(User.GetSpecificClaim("UserId"));
                var check = _billService.CheckStatusBillWithUser(id, user);
                if (check == true)
                {
                    return new OkObjectResult(id);
                }
               

            }
            return new OkResult();
        }

        [HttpPost]
        public IActionResult GetBillById()
        {
            return new OkResult();
        }
        [HttpGet]
        public IActionResult GetOrderBill(int billId)
        {
            return new OkObjectResult(_billService.GetOrderTotal(billId));
        }

        // get thông tin color ,size khi add binding html chọn lại size color khi user add cart ngoài view
        [HttpGet]
        public IActionResult GetColors()
        {
            var colors = _billService.GetColors();
            return new OkObjectResult(colors);
        }

        [HttpGet]
        public IActionResult GetSizes()
        {
            var sizes = _billService.GetSizes();
            return new OkObjectResult(sizes);
        }

        [HttpGet]
        public IActionResult GetPaymentMethod()
        {  // get phương thức thanh toán
            // get name và và danh sách giá trị của enum PaymentMethod
            List<EnumModel> enums = ((PaymentMethod[])Enum.GetValues(typeof(PaymentMethod)))
                .Select(c => new EnumModel()
                {
                    Value = (int)c,
                    Name = c.GetDescription() // lấy ra thông tin hiển thi của paymentmethod
                }).ToList();
            return new OkObjectResult(enums);
        }

        [HttpGet]
        public IActionResult GetBillStatus()
        {// get trạng thái của bill
            List<EnumModel> enums = ((BillStatus[])Enum.GetValues(typeof(BillStatus)))
                .Select(c => new EnumModel()
                {
                    Value = (int)c,
                    Name = c.GetDescription()
                }).ToList();
            return new OkObjectResult(enums);
        }
        #endregion

    }
}