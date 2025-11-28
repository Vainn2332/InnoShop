using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using UserService.ApplicationLayer.Interfaces;

namespace UserService.ApplicationLayer
{
    public class AdminService : IAdminService
    {
        private readonly IUsersService _userService;
        private HttpClient _client;
        public AdminService(IUsersService userService, HttpClient client)
        {
            _userService = userService;
            _client = client;
        }

        public async Task ActivateUserAsync(int id)
        {
            var user = await _userService.GetUserAsync(id);
            user.HasVerifiedEmail = true;
            await _userService.UpdateUserAsync(id, user);

            var json = JsonSerializer.Serialize(id);
            var content=new StringContent(json, Encoding.UTF8, "application/json");
            await _client.PostAsync("http://product_service/api/Products/ActivateProductsOfUser", content);
        }

        public async Task DeactivateUserAsync(int id)
        {
            var user = await _userService.GetUserAsync(id);
            user.HasVerifiedEmail = false;
            await _userService.UpdateUserAsync(id, user);

            var json = JsonSerializer.Serialize(id);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            await _client.PostAsync("http://product_service/api/Products/DeactivateProductsOfUser", content);
        }
    }
}
