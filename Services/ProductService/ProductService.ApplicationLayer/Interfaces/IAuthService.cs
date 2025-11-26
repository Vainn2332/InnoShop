using Microsoft.AspNetCore.Http;
using ProductService.CoreLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.ApplicationLayer.Interfaces
{
    public interface IAuthService
    {
        public const int tokenExpirationTimeInMinutes = 90;

        public JWTInfo ParseJWT(string jwt);
        public string GetJWTFromHeader(HttpRequest request);
    }
}
