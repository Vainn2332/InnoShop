using Microsoft.AspNetCore.Builder;
using Microsoft.IdentityModel.Tokens;
using Moq;
using ProductService.ApplicationLayer.Interfaces;
using ProductService.CoreLayer.Static_Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;
using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
namespace ProductService.Tests
{
    public class ProductServiceIntegrationTest
    {
       
        [Fact]
        public async Task GetRequestReturnsOkStatusCode()
        {
            //Arrange
            var jwt = GenerateTestShortLivedJWT();
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
            //Act
            var response = await httpClient.GetAsync("http://localhost:8082/api/Products");

            //Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetRequestReturnsUnAuthorized()
        {
            //Arrange
            var httpClient = new HttpClient();
            //Act
            var response = await httpClient.GetAsync("http://localhost:8082/api/Products");

            //Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }


        public string GenerateTestShortLivedJWT()
        {
            var claims = new List<Claim>();

            var jwt = new JwtSecurityToken(
                issuer: AuthOptions.ISSUER,
                audience: AuthOptions.AUDIENCE,
                claims: claims,
                expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(5)),
                signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256)
                );
            var encodedJWT = new JwtSecurityTokenHandler().WriteToken(jwt);
            return encodedJWT;
        }
    }
}
