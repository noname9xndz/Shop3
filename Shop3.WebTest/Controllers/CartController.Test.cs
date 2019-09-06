
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using Newtonsoft.Json;
using Shop3.Application.Interfaces;
using Shop3.Application.ViewModels.Products;
using Shop3.Controllers;
using Shop3.Data.Enums;
using Shop3.Models;
using Shop3.Services;
using Shop3.Utilities.Constants;
using Shop3.WebTest.Mock;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using Xunit;
using IEmailSender = Shop3.Services.IEmailSender;

namespace Shop3.WebTest.Controllers
{
    public class CartControllerTest
    {

        private readonly Mock<IBillService> _mockBillService;
        private readonly Mock<IProductService> _mockProductService;
        private readonly Mock<IConfiguration> _configuration;
        private readonly Mock<IEmailSender> _emailSender;
        private readonly Mock<IViewRenderService> _viewRenderService;

        public CartControllerTest()
        {
            _mockBillService = new Mock<IBillService>();
            _mockProductService = new Mock<IProductService>();
            _configuration = new Mock<IConfiguration>();
            _emailSender = new Mock<IEmailSender>();
            _viewRenderService = new Mock<IViewRenderService>();

        }


        [Fact]
        public void Checkout_NullSession_RedirectResult()
        {
            var data = new List<ShoppingCartViewModel>
            {
                new ShoppingCartViewModel
                {
                    Color = null,
                    Price = 1000,
                    Product = new ProductViewModel
                    {
                        Id = 1,
                        Name = "test"
                    },
                    Quantity = 1,
                    Size = null
                }
            };

            // mock cả http context và session
            var mockHttpContext = new Mock<HttpContext>();
            var mockSession = new MockHttpSession();
            mockSession[CommonConstants.CartSession] = JsonConvert.SerializeObject(data);
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);


            var controller = new CartController(_mockProductService.Object, _emailSender.Object, _configuration.Object,
                _mockBillService.Object, _viewRenderService.Object);

            controller.ControllerContext.HttpContext = mockHttpContext.Object;
            var result = controller.Checkout();

            // Assert
            Assert.IsType<RedirectResult>(result);
        }

        [Fact]
        public void Checkout_PostValid_OkResult()
        {
            var data = new List<ShoppingCartViewModel>
            {
                new ShoppingCartViewModel
                {
                    Color = new ColorViewModel
                    {
                        Id = 1,
                        Name = "Red",
                        Code = "RED"
                    },
                    Price = 1000,
                    Product = new ProductViewModel
                    {
                        Id = 1,
                        Name = "test"
                    },
                    Quantity = 1,
                    Size = new SizeViewModel
                    {
                        Id = 1,
                        Name = "M"
                    }
                }
            };

            var mockHttpContext = new Mock<HttpContext>();
            var mockSession = new MockHttpSession();
            mockSession[CommonConstants.CartSession] = JsonConvert.SerializeObject(data);
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);

            // mock user Id
            var claims = new List<Claim>
            {
                new Claim("UserId", Guid.NewGuid().ToString())
            };
            var identity = new ClaimsIdentity(claims, "Test");
            var claimsPrincipal = new ClaimsPrincipal(identity);
            mockHttpContext.Setup(x => x.User).Returns(claimsPrincipal);

            _mockBillService.Setup(x => x.Create(It.IsAny<BillViewModel>()));

            _mockBillService.Setup(x => x.Save());//

            var controller = new CartController(_mockProductService.Object, _emailSender.Object, _configuration.Object,
                _mockBillService.Object, _viewRenderService.Object);

            controller.ControllerContext.HttpContext = mockHttpContext.Object;

            var viewModel = new CheckoutViewModel();
            viewModel.Carts = data;
            viewModel.PaymentMethod = PaymentMethod.PaymentGateway;
            viewModel.CustomerName = "test";
            viewModel.CustomerAddress = "test";
            viewModel.CustomerMobile = "23123";


            var result = controller.Checkout(viewModel);

            var viewResult = Assert.IsType<ViewResult>(result.Result);
            Assert.Equal(true, viewResult.ViewData["Success"]);
        }

        [Fact]
        public void Checkout_ValidRequest_OkResult()
        {
            var data = new List<ShoppingCartViewModel>
            {
                new ShoppingCartViewModel
                {
                    Color = new ColorViewModel
                    {
                        Id = 1,
                        Name = "Red",
                        Code = "RED"
                    },
                    Price = 1000,
                    Product = new ProductViewModel
                    {
                        Id = 1,
                        Name = "test"
                    },
                    Quantity = 1,
                    Size = new SizeViewModel
                    {
                        Id = 1,
                        Name = "M"
                    }
                }
            };

            var mockHttpContext = new Mock<HttpContext>();
            var mockSession = new MockHttpSession();
            mockSession[CommonConstants.CartSession] = JsonConvert.SerializeObject(data);


            mockHttpContext.Setup(s => s.Session).Returns(mockSession);

            var controller = new CartController(_mockProductService.Object, _emailSender.Object, _configuration.Object,
               _mockBillService.Object, _viewRenderService.Object);

            controller.ControllerContext.HttpContext = mockHttpContext.Object;


            var result = controller.Checkout();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<CheckoutViewModel>(
                viewResult.ViewData.Model);
            Assert.Single(model.Carts);
        }
    }
}
