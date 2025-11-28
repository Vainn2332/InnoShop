using Microsoft.Extensions.Logging;
using ProductService.ApplicationLayer.Interfaces;
using ProductService.CoreLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Net.Http.Json;
namespace ProductService.ApplicationLayer
{
    public class UserService : IUserService
    {
        private HttpClient _httpClient;
        private IAuthService _authService;
        private ILogger<UserService> _logger;
        public UserService(HttpClient httpClient, IAuthService authService, ILogger<UserService> logger)
        {
            _httpClient = httpClient;
            _authService = authService;
            _logger = logger;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            _logger.LogInformation("Получение всех пользователей по id...");

            var jwt = _authService.GenerateShortLivedJWT();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

            var response = await _httpClient.GetAsync("http://user_service:8080/api/Users");
            
            var users = await response.Content.ReadFromJsonAsync<IEnumerable<User>>();
            return users;
        }

        public async Task<User?> GetUserAsync(int id)
        {
            _logger.LogInformation($"Получение пользователя по id={id}...");
            if (id < 1)
            {
                _logger.LogError($"Получение пользователя по id={id} неудалось:id должно быть >=1");
                throw new ArgumentException("некорректный id!");
            }

            var jwt = _authService.GenerateShortLivedJWT();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",jwt);

            var response = await _httpClient.GetAsync($"http://user_service:8080/api/Users/{id}");        
            _logger.LogInformation("Запрос на получение пользователя выполнен успешно!");
            return await response.Content.ReadFromJsonAsync<User>();
        }

    }
}
