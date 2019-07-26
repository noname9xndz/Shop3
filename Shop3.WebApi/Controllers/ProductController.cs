using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop3.Application.Interfaces;
using Shop3.Application.ViewModels.Products;
using Shop3.Data.Entities;


namespace Shop3.WebApi.Controllers
{
   // [Authorize]
    //  https://localhost:44303/swagger
    public class ProductController : ApiControllerBase
    {
        private  readonly IProductCategoryService _productCategoryService;
        private readonly IProductService _productService;
        public ProductController(IProductCategoryService productCategoryService,
            IProductService productService)
        {
            _productCategoryService = productCategoryService;
            _productService = productService;
        }

       
        [HttpGet]
        public IActionResult Get()
        {
            return new OkObjectResult(_productCategoryService.GetAll());
        }



    }
}
