using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.CoreLayer.Entities;

namespace UserService.ApplicationLayer.Interfaces
{
    public interface IAuthService
    {
        public const int tokenExpirationTimeInMinutes = 90;
        public string GenerateJWT(int userId, string userEmail);
        public JWTInfo ParseJWT(string jwt);
        public string GetJWTFromHeader(HttpRequest request);
    }
}
