using Microsoft.AspNetCore.Mvc;
using Shop3.Application.Interfaces;


namespace Shop3.WebApi.Controllers
{
    // [Authorize]
    //  https://localhost:44303/swagger
    public class ProductController : ApiControllerBase
    {
        private readonly IProductCategoryService _productCategoryService;
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
