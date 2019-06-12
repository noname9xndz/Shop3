using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Shop3.Application.Interfaces;
using Shop3.Extensions;
using Shop3.Models;

namespace Shop3.Controllers
{
    public class HomeController : Controller
    {
        private IProductService _productService;
        private IProductCategoryService _productCategoryService;
        private readonly IBillService _billService;
        private IBlogService _blogService;
        private ICommonService _commonService;

        private readonly IStringLocalizer<HomeController> _localizer; // truy cập đa ngôn ngữ trong controller bằng CookieRequestCultureProvider

        public HomeController(IProductService productService,
        IBlogService blogService, ICommonService commonService,
        IProductCategoryService productCategoryService,
        IBillService billService,
        IStringLocalizer<HomeController> localizer)
        {
            _blogService = blogService;
            _commonService = commonService;
            _productService = productService;
            _productCategoryService = productCategoryService;
            _billService = billService;
             _localizer = localizer;
        }

        // responsecache : https://docs.microsoft.com/en-us/aspnet/core/performance/caching/response?view=aspnetcore-2.2
        // response caching bản chất là viết lại các response header để trình duyệt hiểu được để trình duyệt hiểu được cache trong bao lâu
        // [ResponseCache(CacheProfileName = "Default")] // get thông số trong startup
        // cache output bằng HTTP-based response caching , cache dữ liệu : bằng distributed hoặc memory 
        public IActionResult Index()
        {
            var title = _localizer["Title"];
            var culture = HttpContext.Features.Get<IRequestCultureFeature>().RequestCulture.Culture.Name;

            ViewData["BodyClass"] = "cms-index-index cms-home-page";
            var homeVm = new HomeViewModel();
            homeVm.HomeCategories = _productCategoryService.GetHomeCategories(5);
            homeVm.HotProducts = _productService.GetHotProduct(5);
            homeVm.TopSellProducts = _productService.GetLastest(3);
            homeVm.LastestBlogs = _blogService.GetLastest(8);
            homeVm.NewProducts = _productService.GetNewProduct(3);
            homeVm.HomeSlides = _commonService.GetSlides("top");
            homeVm.SpecialOfferProducts = _productService.GetSpecialOfferProduct(3);
            return View(homeVm);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            return LocalRedirect(returnUrl);
        }
        [HttpGet]
        public IActionResult GetById(int id)
        {
            var model = _productService.GetById(id);

            return new OkObjectResult(model);
        }
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
        public IActionResult GetQuantities(int productId)
        {
            var quantities = _productService.GetQuantities(productId);
            return new OkObjectResult(quantities);
        }

    }
}
