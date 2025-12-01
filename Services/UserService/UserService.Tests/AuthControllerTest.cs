using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.API.Controllers;
using UserService.API.Validators;
using UserService.ApplicationLayer.Interfaces;
using Xunit;
using UserService.CoreLayer.Entities;
using UserService.CoreLayer.DTOs;
using Microsoft.AspNetCore.Mvc;
namespace UserService.Tests
{
    public class AuthControllerTest
    {
        [Fact]
        public async Task LoginUserReturnsSuccess()
        {
            var testUser = new UserLoginDTO()
            {
                EmailAddress = "vladuk2332@gmail.com",
                Password = "123321"
            };
            var userServiceMock = new Mock<IUsersService>();
            userServiceMock.Setup(p => p.GetUserByEmailAsync("vladuk2332@gmail.com")).ReturnsAsync(new User()
            {
                ID=1,
                Name="Vlad",
                EmailAddress= "vladuk2332@gmail.com",
                HasVerifiedEmail=true,
                Password="123321"
            });
            var adminServiceMock = new Mock<IAdminService>();
            var authServiceMock = new Mock<IAuthService>();
            var emailServiceMock = new Mock<IEmailService>();
            var passwordServiceMock = new Mock<IPasswordService>();
            passwordServiceMock.Setup(p => p.VerifyPassword(testUser.Password, "123321")).Returns(true);
            var userValidator = new UserValidator(userServiceMock.Object);
            var controller = new AuthenticationController(userServiceMock.Object,emailServiceMock.Object,authServiceMock.Object
                ,adminServiceMock.Object,passwordServiceMock.Object,userValidator);


            var result = await controller.Login(testUser);


            passwordServiceMock.Verify(p => p.VerifyPassword(testUser.Password, "123321"));
            authServiceMock.Verify(p => p.GenerateJWT(1, "vladuk2332@gmail.com"));
            Assert.IsType<OkObjectResult>(result);
        }
    }
}
