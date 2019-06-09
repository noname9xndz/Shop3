using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using Shop3.Application.Interfaces;
using Shop3.Application.ViewModels.Products;
using Shop3.Controllers.Components;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Shop3.WebTest.Controllers.Components
{
    public class CategoryMenuViewComponentTest
    {

        [Fact]
        public async Task Invoke_ValidInput_ResultOk()
        {
            //Arrange
            var mockProductCategoryService = new Mock<IProductCategoryService>();
            mockProductCategoryService.Setup(x => x.GetAll())
                .Returns(new List<ProductCategoryViewModel>()
                {
                    new ProductCategoryViewModel()
                    {
                        Id = 1,
                        Name = "test"
                    }
                });
            var cache = new MemoryCache(new MemoryCacheOptions());
            var viewComponent = new CategoryMenuViewComponent(mockProductCategoryService.Object, cache);

            //Act
            var result = await viewComponent.InvokeAsync();

            //Assert
            Assert.IsAssignableFrom<IViewComponentResult>(result);
            //...other assertions
        }
    }
}
