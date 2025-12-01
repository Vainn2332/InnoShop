using Moq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using UserService.ApplicationLayer.Interfaces;
using UserService.CoreLayer.DTOs;
using Xunit;

namespace UserService.Tests
{
    public class UserServiceIntegrationTest
    {
        [Fact]
        public async Task LoginUserReturnsOkStatusCode()//не будет работать если в бд с пользователями нет такого пользователя
        {
            //Arrange
            UserLoginDTO user = new UserLoginDTO()
            {
                EmailAddress = "admin@gmail.com",
                Password = "admin"
            };
            var client = new HttpClient();
            var json = JsonSerializer.Serialize(user);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            //Act
            var response = await client.PostAsync("http://localhost:8081/api/Authentication/login", content);

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);



        }
    }
}
