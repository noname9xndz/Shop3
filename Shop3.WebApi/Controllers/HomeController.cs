using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shop3.Application.Interfaces;
using Shop3.Models;

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
        public HomeController( IProductService productService,
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

        //[Route("getall")]
        //[HttpGet]
        //public HttpResponseMessage Get(HttpRequestMessage request, int? categoryId, string keyword, int page, int pageSize = 20)
        //{
        //    return CreateHttpResponse(request,
        //        () =>
        //        {
        //            var model = _productService.GetAllPaging(categoryId, keyword, page, pageSize);
        //            HttpRequestMessage reques2t = new HttpRequestMessage(
        //                HttpMethod.Get, Uri.UriSchemeFtp
        //            );
        //            var response = request.CreateResponse(HttpStatusCode.OK);
        //            return response;

        //        });
        //}

        [Route("Index")]
        [HttpGet]
        public ActionResult<IEnumerable<HomeViewModel>> Get()
        {
            var homeVm = new HomeViewModel();
            homeVm.HomeCategories = _productCategoryService.GetHomeCategories(5);
            homeVm.HotProducts = _productService.GetHotProduct(5);
            homeVm.TopSellProducts = _productService.GetLastest(3);
            homeVm.LastestBlogs = _blogService.GetLastest(8);
            homeVm.NewProducts = _productService.GetNewProduct(3);
            homeVm.HomeSlides = _commonService.GetSlides("top");
            homeVm.SpecialOfferProducts = _productService.GetSpecialOfferProduct(3);
            return new OkObjectResult(homeVm);
        }

    }
}