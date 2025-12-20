using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.API.Controllers;
using UserService.API.Validators;
using UserService.ApplicationLayer.Interfaces;
using UserService.CoreLayer.Entities;
using Xunit;

namespace UserService.Tests
{
    public class UsersControllerTest
    {
        [Fact]
        public async Task GetUserReturnsSuccess()
        {
            int testUserId = 2;
            var authServiceMock=new Mock<IAuthService>();
            var userServiceMock = new Mock<IUsersService>();//создали заглушку на данный интерфейс
            userServiceMock.Setup(p => p.GetUserAsync(testUserId)).ReturnsAsync(GetUsers().FirstOrDefault(u => u.ID == testUserId));
            var passwordServiceMock = new Mock<IPasswordService>();
            var userValidator = new UserValidator(userServiceMock.Object);
            var usersController = new UsersController(userServiceMock.Object,passwordServiceMock.Object,userValidator,authServiceMock.Object);

            var result =await usersController.Get(testUserId);//act

            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetUserReturnsNullWithBadRequest()
        {
            var userServiceMock = new Mock<IUsersService>();
            var authServiceMock = new Mock<IAuthService>();
            userServiceMock.Setup(p => p.GetUserAsync(1)).ReturnsAsync((User)null);
            var passwordServiceMock = new Mock<IPasswordService>();
            var userValidator = new UserValidator(userServiceMock.Object);
            var usersController = new UsersController(userServiceMock.Object, passwordServiceMock.Object, userValidator, authServiceMock.Object);

            var result = await usersController.Get(1);

            Assert.IsType<NotFoundObjectResult>(result);

        }

       

        private List<User> GetUsers()
        {
            var users = new List<User>()
            {
                new User()
                {
                    ID=1,
                    EmailAddress="test"
                },
                new User()
                {
                    ID=2,
                    EmailAddress="example.com"
                },
                new User()
                {
                    ID=3,
                    EmailAddress="blabla"
                }
            };
            return users;
        } 
    }
}
