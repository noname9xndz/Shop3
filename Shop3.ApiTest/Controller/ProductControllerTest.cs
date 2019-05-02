using System;
using Moq;
using Shop3.Application.Interfaces;
using Shop3.WebApi.Controllers;
using Xunit;

namespace Shop3.WebTest
{
    // https://viblo.asia/p/viet-mot-test-case-tot-eXaRlwdPGmx
    // https://viblo.asia/p/tu-dong-test-automation-testing-cho-trang-web-aspnet-core-20-phan-1-unit-test-ByEZk96E5Q0#_gioi-thieu-ve-unit-test-test-tung-ham-1
    // https://code-maze.com/unit-testing-aspnetcore-web-api/?fbclid=IwAR331GVTFflhseeNII5OlHs_rLhCG-pkaW9djouav5O-1g6QgQq09Nz7GPQ
    // Arrange  khởi tạo dữ liệu phục vụ cho việc test
    // Act Đây là giai đoạn test case gọi hàm cần test.
    // Assert là giai đoạn đảm bảo rằng kết quả đầu ra khớp với kỳ vọng, có thể kết hợp nhiều đối tượng với nhau, gọi nhiều phương thức, nhưng phải gọi câu lệnh assert với những kết quả ý nghĩa
    public class ProductControllerTest
    {
        
        private Mock<IProductCategoryService> _productCategoryService;
        private ProductController _productController;
        public ProductControllerTest()
        {
            _productCategoryService = new Mock<IProductCategoryService>();
            _productController = new ProductController(_productCategoryService.Object);
        }

        [Fact]
        public void Test1()
        {  

        }
    }
}
