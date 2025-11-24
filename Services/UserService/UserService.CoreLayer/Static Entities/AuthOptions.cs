using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
namespace UserService.CoreLayer.Static_Entities
{
    public static class AuthOptions
    {
        public const string ISSUER = "myServer";
        public const string AUDIENCE = "myClient";
        private const string secretKey = "superSecretKeyThatOnlyIKnowBecauseIAmTheCreator!!!";
        public static SymmetricSecurityKey GetSymmetricSecurityKey() =>
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
       
    }
}
