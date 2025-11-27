using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using ProductService.ApplicationLayer.Interfaces;
using ProductService.CoreLayer.Entities;
using ProductService.CoreLayer.Static_Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ProductService.ApplicationLayer
{
    public class AuthService : IAuthService
    {
        public string GenerateShortLivedJWT()
        {
            var claims = new List<Claim>();
            
            var jwt = new JwtSecurityToken(
                issuer: AuthOptions.ISSUER,
                audience: AuthOptions.AUDIENCE,
                claims: claims,
                expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(IAuthService.tokenExpirationTimeInMinutes)),
                signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256)
                );
            var encodedJWT = new JwtSecurityTokenHandler().WriteToken(jwt);
            return encodedJWT;
        }

        public string GetJWTFromHeader(HttpRequest request)
        {
            request.Headers.TryGetValue("Authorization", out var token);
            string jwt = token.ToString().Replace("Bearer", "");
            return jwt;
        }

        public JWTInfo ParseJWT(string jwt)
        {
            var parts = jwt?.Split('.');
            if (parts?.Length != 3 || string.IsNullOrEmpty(jwt))
            {
                throw new ArgumentException("Неверный формат jwt токена!");
            }
            var payload = parts[1];

            //без этого не работает(скорее всего нужно чтобы добить до нужной длины)
            payload = payload.PadRight(payload.Length + (4 - payload.Length % 4) % 4, '=');

            var payloadBytes = Convert.FromBase64String(payload);
            var payloadJSON = Encoding.UTF8.GetString(payloadBytes);
            var info = JsonSerializer.Deserialize<JWTInfo>(payloadJSON);
            return info;
        }
    }
}
