using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ProductService.API.Controllers;
using ProductService.ApplicationLayer.Interfaces;
using ProductService.CoreLayer.DTOs;
using ProductService.CoreLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ProductService.Tests
{
    public class ProductsControllerTest
    {
        [Fact]
        public async Task PostProductReturnsSuccess()
        {
            //Arrange
            var jwtInfo = new JWTInfo()
            {
                EmailAddress = "abcd@example.com",
                UserId = 1
            };
            ProductPostAndPutDTO productDTO = new ProductPostAndPutDTO()
            {
                Name="testName",
                Description="testDescription",
                Price=1                
            };
           
            var productServiceMock = new Mock<IProductsService>();
            var authServiceMock = new Mock<IAuthService>();

            //var httpMock = new Mock<HttpRequest>();
            var httpContext = new DefaultHttpContext().Request;

            authServiceMock.Setup(p => p.GetJWTFromHeader(It.IsAny<HttpRequest>())).Returns("jwt");
            authServiceMock.Setup(p => p.ParseJWT("jwt")).Returns(jwtInfo);
            var userServiceMock = new Mock<IUserService>();
            var controller = new ProductsController(productServiceMock.Object,
                authServiceMock.Object, userServiceMock.Object);

            //Act
            var result = await controller.Post(productDTO);

            //Assert
            productServiceMock.Verify(p => p.AddProductAsync(It.IsAny<Product>()  
            ));
            Assert.IsType<OkResult>(result);

        }



        [Fact]
        public async Task PutProductReturnsSuccess()
        {
            //Arrange
            var jwtInfo = new JWTInfo()
            {
                EmailAddress = "abcd@example.com",
                UserId = 1
            };
            ProductPostAndPutDTO productDTO = new ProductPostAndPutDTO()
            {
                Name = "testName",
                Description = "testDescription",
                Price = 1
            };

            var productServiceMock = new Mock<IProductsService>();
            productServiceMock.Setup(p => p.CheckPossessionAsync(1, 1)).ReturnsAsync(true);
            var authServiceMock = new Mock<IAuthService>();
            authServiceMock.Setup(p => p.GetJWTFromHeader(It.IsAny<HttpRequest>())).Returns("jwt");
            authServiceMock.Setup(p => p.ParseJWT("jwt")).Returns(jwtInfo);

            var userServiceMock = new Mock<IUserService>();
            var httpContext = new DefaultHttpContext().Request;

            var controller = new ProductsController(productServiceMock.Object,
                authServiceMock.Object, userServiceMock.Object);

            //Act
            var result = await controller.Put(1,productDTO);

            //Assert
            productServiceMock.Verify(p => p.UpdateProductAsync(1,It.IsAny<Product>()
            ));
            Assert.IsType<OkResult>(result);

        }


        [Fact]
        public async Task PutProductReturnsUnauthorized()
        {
            //Arrange
            var jwtInfo = new JWTInfo()
            {
                EmailAddress = "abcd@example.com",
                UserId = 1
            };
            ProductPostAndPutDTO productDTO = new ProductPostAndPutDTO()
            {
                Name = "testName",
                Description = "testDescription",
                Price = 1
            };

            var productServiceMock = new Mock<IProductsService>();
            productServiceMock.Setup(p => p.CheckPossessionAsync(1, 1)).ReturnsAsync(false);
            var authServiceMock = new Mock<IAuthService>();
            authServiceMock.Setup(p => p.GetJWTFromHeader(It.IsAny<HttpRequest>())).Returns("jwt");
            authServiceMock.Setup(p => p.ParseJWT("jwt")).Returns(jwtInfo);

            var userServiceMock = new Mock<IUserService>();
            var httpContext = new DefaultHttpContext().Request;

            var controller = new ProductsController(productServiceMock.Object,
                authServiceMock.Object, userServiceMock.Object);

            //Act
            var result = await controller.Put(1, productDTO);

            //Assert
            productServiceMock.Verify(p => p.UpdateProductAsync(1, It.IsAny<Product>()),
                Times.Never);
            Assert.IsType<UnauthorizedObjectResult>(result);

        }
    }
}
