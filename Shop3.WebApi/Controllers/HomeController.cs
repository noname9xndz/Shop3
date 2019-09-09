using System;
using Microsoft.AspNetCore.Mvc;
using Shop3.Application.Interfaces;
using Shop3.Models;
using System.Collections.Generic;
using Shop3.Utilities.Enum;
using Shop3.Utilities.Extensions;

namespace Shop3.WebApi.Controllers
{
    public class HomeController : ApiControllerBase
    {
        private readonly IProductService _productService;
        private readonly IProductCategoryService _productCategoryService;
        private readonly IBillService _billService;
        private readonly IBlogService _blogService;
        private readonly ICommonService _commonService;

        // private readonly IStringLocalizer<HomeController> _localizer;
        public HomeController(IProductService productService,
            IBlogService blogService, ICommonService commonService,
            IProductCategoryService productCategoryService,
            IBillService billService)
        {
            _blogService = blogService;
            _commonService = commonService;
            _productService = productService;
            _productCategoryService = productCategoryService;
            _billService = billService;
        }

        

        [Route("Index")]
        [HttpGet]
        public IActionResult Get()
       // public ActionResult<IEnumerable<HomeViewModel>> Get()
        {
            try
            {
                var homeVm = new HomeViewModel();
                homeVm.HomeCategories = _productCategoryService.GetHomeCategories(5);
                homeVm.HotProducts = _productService.GetHotProduct(5);
                homeVm.TopSellProducts = _productService.GetLastest(3);
                homeVm.LastestBlogs = _blogService.GetLastest(8);
                homeVm.NewProducts = _productService.GetNewProduct(3);
                homeVm.HomeSlides = _commonService.GetSlides("top");
                homeVm.SpecialOfferProducts = _productService.GetSpecialOfferProduct(3);
                return this.OkResult(homeVm);
            }
            catch (Exception)
            {
                return this.ErrorResult(ErrorCode.GET_FAILURE);
            }
            
           // return new OkObjectResult(homeVm);
        }

    }
}