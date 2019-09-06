using Microsoft.AspNetCore.Mvc;
using Moq;
using Shop3.Application.Interfaces;
using Shop3.Application.ViewModels.Products;
using Shop3.WebApi.Controllers;
using System;
using System.Collections.Generic;
using Xunit;

namespace Shop3.WebTest
{
    // https://viblo.asia/p/viet-mot-test-case-tot-eXaRlwdPGmx
    // https://viblo.asia/p/tu-dong-test-automation-testing-cho-trang-web-aspnet-core-20-phan-1-unit-test-ByEZk96E5Q0#_gioi-thieu-ve-unit-test-test-tung-ham-1
    // https://code-maze.com/unit-testing-aspnetcore-web-api/?fbclid=IwAR331GVTFflhseeNII5OlHs_rLhCG-pkaW9djouav5O-1g6QgQq09Nz7GPQ
    /*
       Arrange  khởi tạo dữ liệu phục vụ cho việc test
       Act Đây là giai đoạn test case gọi hàm cần test.
       Assert là giai đoạn đảm bảo rằng kết quả đầu ra khớp với kỳ vọng, có thể kết hợp nhiều đối tượng với nhau, gọi nhiều phương thức, nhưng phải gọi câu lệnh assert với những kết quả ý nghĩa
       Fact : chỉ ra phương thức này là 1 phương thức test (test các điều kiện ko cần biến truyền vào)
       Theory : chỉ ra phương thức này là 1 phương thức test(test các điều kiện có tham số truyền vào), theo nó là tập các testcase
       InlineData : chứa các test
       Mock : giúp thay thế các đối tượng thật thuộc tính phương thức và giúp độc lập việc test 1 đơn vị code cụ thể với các denpendency khác
       
         */
    public class ProductControllerTest
    {
        private readonly Mock<IProductCategoryService> _mockProductCategoryService;
        private readonly Mock<IProductService> _mockProductService;

        public ProductControllerTest()
        {
            _mockProductCategoryService = new Mock<IProductCategoryService>();
            _mockProductService = new Mock<IProductService>();

        }
        [Fact]
        public void Get_ValidRequest_OkResult()
        {

            _mockProductCategoryService.Setup(x => x.GetAll())
                .Returns(new List<ProductCategoryViewModel>()
            {
                 new ProductCategoryViewModel(){Id = 1, Name="test 1"},
                 new ProductCategoryViewModel(){Id = 2, Name="test 2"},
            });
            var controller = new ProductController(_mockProductCategoryService.Object, _mockProductService.Object);

            var result = controller.Get();

            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, (result as OkObjectResult).StatusCode);
        }

        [Fact]
        public void Get_ServiceException_BadRequestResult()
        {
            _mockProductCategoryService.Setup(x => x.GetAll()).Throws<Exception>();

            var controller = new ProductController(_mockProductCategoryService.Object, _mockProductService.Object);

            Assert.ThrowsAny<Exception>(() => { controller.Get(); });
        }
    }
}
