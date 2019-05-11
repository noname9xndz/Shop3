using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Shop3.Application.Implementation;
using Shop3.Application.Interfaces;
using Shop3.Data.Entities;
using Shop3.Data.Enums;
using Shop3.Infrastructure.Interfaces;
using Xunit;

namespace Shop3.AppliactionTest.ServiceTest
{
    public class ProductCategoryServiceTest
    {
       // private readonly IProductCategoryService _categoryService;
       // private Mock<IUnitOfWork> _mockUnitOfWork;

        
        [Fact]
        public void ProductCategoryServiceGetAllTest()
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
           var serviceMock = new Mock<IProductCategoryService>();
           //serviceMock.Setup(m => m.GetAll());
           //var productCategoryService = new ProductCategoryService(serviceMock.Object, mockUnitOfWork.Object);

            //using (var testServer = new ConfigurableServer(sc => sc.Replace(serviceDescriptor)))
            //{
            //    var client = testServer.CreateClient();
            //    var value = await client.GetStringAsync("api/value");
            //    Assert.Equal("Hello mockworld", value);
            //}

            //var categories = _categoryService.GetAll();
            //Assert.NotNull(categories);
            
        }

        //[Fact]
        //public  void ProductCategoryServiceAddTest()
        //{

           
        //    //Arrange
        //    //ProductCategory productCategory = new ProductCategory
        //    //{
        //    //    Name = "Product Category XTest",
        //    //    Description = "description Test",
        //    //    ParentId = 2,
        //    //    HomeOrder = 2,
        //    //    Image = "/test_url_Img",
        //    //    HomeFlag = false,
        //    //    SortOrder = 7,
        //    //    Status = 0,
        //    //    SeoPageTitle = "SeoPageTitle Test",
        //    //    SeoAlias = " SeoAlias Test ",
        //    //    SeoKeywords = "seoKeywords test",
        //    //    SeoDescription = "seoDescription ",
        //    //};

        //    //Act
        //    //Assert
        //}
    }
}
