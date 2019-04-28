using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Shop3.Application.Interfaces;
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
        IConfiguration _configuration;
        IEmailSender _emailSender;
        public CartController(IProductService productService, IEmailSender emailSender,
            IConfiguration configuration, IBillService billService)
        {
            _productService = productService;
            _billService = billService;
            _configuration = configuration;
            _emailSender = emailSender;
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
            //var model = new CheckoutViewModel();
            //var session = HttpContext.Session.Get<List<ShoppingCartViewModel>>(CommonConstants.CartSession);
            //if (session.Any(x => x.Color == null || x.Size == null))
            //{
            //    return Redirect("/cart.html");
            //}

            //model.Carts = session;
            //return View(model);
            return View();
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