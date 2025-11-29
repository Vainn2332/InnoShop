using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using UserService.ApplicationLayer.Interfaces;

namespace UserService.ApplicationLayer
{
    public class AdminService : IAdminService
    {
        private IAuthService _authService;
        private IUsersService _userService;
        private HttpClient _client;
        public AdminService(IUsersService userService, HttpClient client, IAuthService authService)
        {
            _userService = userService;
            _client = client;
            _authService = authService;
        }

        public async Task ActivateUserAsync(int id)
        {
            var user = await _userService.GetUserAsync(id);
            user.HasVerifiedEmail = true;
            await _userService.UpdateUserAsync(id, user);


            var jwt = _authService.GenerateShortLivedJWT();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

            var json = JsonSerializer.Serialize(id);//это смущает возможно из за этого ошибка
            var content=new StringContent(json, Encoding.UTF8, "application/json");
            await _client.PostAsync("http://product_service:8080/api/Products/ActivateProductsOfUser", content);
        }

        public async Task DeactivateUserAsync(int id)
        {
            var user = await _userService.GetUserAsync(id);
            user.HasVerifiedEmail = false;
            await _userService.UpdateUserAsync(id, user);

            var jwt = _authService.GenerateShortLivedJWT();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

            var json = JsonSerializer.Serialize(id);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            await _client.PostAsync("http://product_service:8080/api/Products/DeactivateProductsOfUser", content);
        }
    }
}
