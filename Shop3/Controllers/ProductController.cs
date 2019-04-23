using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Shop3.Application.Interfaces;
using Shop3.Models.ProductViewModels;

namespace Shop3.Controllers
{
    public class ProductController : Controller
    {
        IProductService _productService;
        IBillService _billService;
        IProductCategoryService _productCategoryService;
        IConfiguration _configuration;
        public ProductController(IProductService productService, IConfiguration configuration,
            IBillService billService,
            IProductCategoryService productCategoryService)
        {
            _productService = productService;
            _productCategoryService = productCategoryService;
            _configuration = configuration;
            _billService = billService;
        }

        //https://docs.microsoft.com/en-us/aspnet/core/mvc/controllers/routing?view=aspnetcore-2.2
        // khi view bất kỳ nào đó use route kiểu này thì view đó sẽ mất route mặc định trong startup
        [Route("products.html")] // điều hướng cho controller
        public IActionResult Index()
        {
            return View();
        }

        [Route("{alias}-c.{id}.html")]
        // danh muc sp
        public IActionResult Catalog(int id, int? pageSize, string sortBy, int page = 1)
        {
            var catalog = new CatalogViewModel();
            ViewData["BodyClass"] = "shop_grid_full_width_page";
            if (pageSize == null)
                pageSize = _configuration.GetValue<int>("PageSize");
             
            catalog.PageSize = pageSize;
            catalog.SortType = sortBy;
            catalog.Data = _productService.GetAllPaging(id, string.Empty, page, pageSize.Value);
            catalog.Category = _productCategoryService.GetById(id);

            return View(catalog);
        }
        [Route("{alias}-p.{id}.html", Name = "ProductDetail")]
        public IActionResult Details(int id)
        {
            return View();
        }
        //[Route("{alias}.html", Name = "ProductDetail")]
        //public IActionResult Details(string alias)
        //{
        //    return View();
        //}

    }
}