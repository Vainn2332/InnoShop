using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using UserService.CoreLayer.Entities;
using UserService.CoreLayer.Static_Entities;

namespace UserService.ApplicationLayer.Interfaces
{
    public interface IAuthService
    {
        protected const int tokenExpirationTimeInMinutes = 90;
        protected const int shortTokenExpirationTimeInMinutes = 5;
        public string GenerateShortLivedJWT();
        
        public string GenerateJWT(int userId, string userEmail);
        public JWTInfo ParseJWT(string jwt);
        public string GetJWTFromHeader(HttpRequest request);
    }
}
