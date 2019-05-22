using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Shop3.Application.Interfaces;
using Shop3.Application.ViewModels.Products;
using Shop3.Data.Enums;
using Shop3.Extensions;
using Shop3.Models;
using Shop3.Services;
using Shop3.Utilities.Constants;

namespace Shop3.Controllers
{
    public class CartController : Controller
    {
        IProductService _productService;
        IBillService _billService;
        readonly IConfiguration _configuration;
        IEmailSender _emailSender;
        IViewRenderService _viewRenderService;
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
                        BillDetails = details
                    };
                    if (User.Identity.IsAuthenticated == true)
                    {
                        billViewModel.CustomerId = Guid.Parse(User.GetSpecificClaim("UserId"));
                    }
                    _billService.Create(billViewModel);
                    try
                    {

                        _billService.Save();

                        // sử dụng service ViewRender để tạo ra viewhtml
                        var content = await _viewRenderService.RenderToStringAsync("Cart/_BillMail", billViewModel);
                        //Send mail to admin
                        await _emailSender.SendEmailAsync(_configuration["MailSettings:AdminMail"], "New bill from Noname Shop", content);
                        //Todo Send mail to user

                        //  HttpContext.Session.Remove(CommonConstants.CartSession); remove sau khi đặt hàng thành công

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
                        Price = product.PromotionPrice ?? product.Price
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
                    Price = product.PromotionPrice ?? product.Price
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
                if (hasChanged ==true)
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
        #endregion

    }
}