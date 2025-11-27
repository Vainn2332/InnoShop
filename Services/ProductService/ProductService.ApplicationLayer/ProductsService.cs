using Microsoft.Extensions.Logging;
using ProductService.ApplicationLayer.Interfaces;
using ProductService.CoreLayer;
using ProductService.CoreLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.ApplicationLayer
{
    public class ProductsService : IProductsService
    {
        private IProductRepository _productRepository;
        private ILogger<ProductsService> _logger;
        private IUserService _userService;
        public ProductsService(IProductRepository productRepository, ILogger<ProductsService> logger, IUserService userService)
        {
            _productRepository = productRepository;
            _logger = logger;
            _userService = userService;
        }

        public Task AddProductAsync(Product product)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> CheckPossessionAsync(int productId, int userId)
        {
            _logger.LogInformation($"Проверка продукта с id={productId} на принадлежность пользователю с userId={userId}...");
            if (productId < 1 || userId < 1)
            {
                _logger.LogError($"id продукта={productId} и userId={userId} должны быть >= 1");
                throw new ArgumentException("некорректно введены id!");
            }
            var target = await this.GetProductAsync(productId);
            if (target is null)
            {
                _logger.LogError($"Проверка на принадлежность продукта не удалась:Продукт с id={productId} не найден!");
                throw new ArgumentException("Продукт не найден!");
            }
            //не проверяем пользователя на существование т.к. у нас не может быть сиротской связи + нам в данном случае не важно(всё равно false)
            return target.UserId == userId;
        }

        public async Task DeleteProductAsync(int id)
        {
            _logger.LogInformation($"удаление продукта по id={id}...");
            if (id < 1)
            {
                _logger.LogError($"Удаление не удалось:id продукта={id} должно быть >= 1");
                throw new ArgumentException("некорректный id!");
            }

            var target = await this.GetProductAsync(id);
            if (target == null)
            {
                _logger.LogError($"Удаление не удалось:Продукт с id={id} не найден!");
                throw new ArgumentException("Продукт не найден!");
            }

            await _productRepository.DeleteAsync(id);
            _logger.Log(LogLevel.Information, $"продукт с id={id} удалён успешно");
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            _logger.LogInformation("Получение всех продуктов...");
            var products= await _productRepository.GetAllAsync();
            var users = await _userService.GetAllUsersAsync();
            
            var verifiedUsers=users.Where(u=>u.HasVerifiedEmail==true);

            //softDelete
            _logger.LogInformation("Все продукты получены успешно");
            return products;
        }

        public async Task<Product?> GetProductAsync(int id)
        {
            _logger.LogInformation($"Получение продукта по id={id}...");
            var product = await _productRepository.GetAsync(id);
            if(product is null)
            {
                _logger.LogWarning($"продукт с id={id} не найден,возвращение null!");
            }
            else
                _logger.LogInformation($"продукт с id={id} получен успешно!");
            return product;
        }

        public async Task<IEnumerable<Product>> GetProductsOfUserAsync(int userId)
        {
            //проверить наличие пользователя

            var products = await this.GetAllProductsAsync();
            var productsOfUser=products.Where(p=>p.UserId== userId);
            return productsOfUser;
        }

        public async Task UpdateProductAsync(int id, Product newProduct)
        {
            _logger.LogInformation($"обновление продукта по id={id}...");
            if (id < 1)
            {
                _logger.LogError($"Обновить продукт с id продукта={id} не удалось: id должно быть >= 1");
                throw new ArgumentException("некорректный id!");
            }

            var target = await this.GetProductAsync(id);
            if (target == null)
            {
                _logger.LogError($"Обновление не удалось:Продукт с id={id} не найден!");
                throw new ArgumentException("Продукт не найден!");
            }
            await _productRepository.UpdateAsync(id, newProduct);
            _logger.LogInformation($"обновление продукта по id={id} выполнено успешно");
        }
    }
}
