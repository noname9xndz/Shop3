using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using Shop3.Application.Interfaces;
using Shop3.Application.ViewModels.Products;
using Shop3.Controllers;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace Shop3.ApiTest
{
    public class ProductControllerTest
    {
        private readonly Mock<IProductService> _mockproductService;
        private readonly Mock<IProductCategoryService> _mockproductCategoryService;
        private readonly Mock<IBillService> _mockbillService;

        public ProductControllerTest()
        {
            _mockproductService = new Mock<IProductService>();
            _mockproductCategoryService = new Mock<IProductCategoryService>();
            _mockbillService = new Mock<IBillService>();
        }

        [Fact]
        public void Index_GetCategory_ValidModel()
        {
            // mock : IConfiguration
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            var configuration = builder.Build();

            _mockproductCategoryService.Setup(x => x.GetAll()).Returns(new List<ProductCategoryViewModel>()
            {
                new ProductCategoryViewModel()
                {
                    Name = "Test"
                }
            });

            ProductController controller = new ProductController(_mockproductService.Object, configuration,
                _mockbillService.Object, _mockproductCategoryService.Object);

            var result = controller.Index();
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<ProductCategoryViewModel>>(
                viewResult.ViewData.Model);
            Assert.Single(model);
        }
    }
}