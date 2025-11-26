using Microsoft.AspNetCore.Http;
using ProductService.ApplicationLayer.Interfaces;
using ProductService.CoreLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ProductService.ApplicationLayer
{
    public class AuthService : IAuthService
    {
        public string GetJWTFromHeader(HttpRequest request)
        {
            request.Headers.TryGetValue("Authorization", out var token);
            string jwt = token.ToString().Replace("Bearer", "");
            return jwt;
        }

        public JWTInfo ParseJWT(string jwt)
        {
            var parts = jwt.Split('.');
            if (parts.Length != 3 || string.IsNullOrEmpty(jwt))
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
