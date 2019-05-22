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
        private Mock<IRepository<ProductCategory, int>> _mockProductCategoryRepository;
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private List<ProductCategory> _listCategory;

        [Fact]
        public void ProductCategoryServiceGetAllTest()
        {
            _mockProductCategoryRepository = new Mock<IRepository<ProductCategory, int>>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            var productCategoryService = new ProductCategoryService(_mockProductCategoryRepository.Object, _mockUnitOfWork.Object);
            _listCategory = new List<ProductCategory>()
            {
                
                new ProductCategory(){Name="danh mục 1",HomeFlag = false, SortOrder = 7, Status = 0,DateCreated=DateTime.Now,DateModified=DateTime.Now},
                new ProductCategory(){Name="danh mục 2",HomeFlag = false, SortOrder = 7, Status = 0,DateCreated=DateTime.Now,DateModified=DateTime.Now},
                new ProductCategory(){Name="danh mục 3",HomeFlag = false, SortOrder = 7, Status = 0,DateCreated=DateTime.Now,DateModified=DateTime.Now}
            };

          // var categories = productCategoryService; 
           //_mockProductCategoryRepository.Setup(m => m.);

            //using (var testServer = new ConfigurableServer(sc => sc.Replace(serviceDescriptor)))
            //{
            //    var client = testServer.CreateClient();
            //    var value = await client.GetStringAsync("api/value");
            //    Assert.Equal("Hello mockworld", value);
            //}
            //var categories = _categoryService.GetAll();
           //Assert.NotNull(categories);
          // Assert.NotEmpty(categories);


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
