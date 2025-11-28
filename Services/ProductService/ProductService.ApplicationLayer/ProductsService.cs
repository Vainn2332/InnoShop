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

        public async Task DeactivateAllProductsOfUserAsync(int userId)
        {
            _logger.LogInformation($"Деактивация всех продуктов пользователя по id={userId}...");
            if (userId < 1)
            {
                _logger.LogError($"Деактивация не удалась:id пользователя={userId} должно быть >= 1");
                throw new ArgumentException("некорректный id!");
            }

            var target = await _userService.GetUserAsync(userId);
            if(target == null)
            {
                _logger.LogError($"Деактивация всех продуктов пользователя по id={userId} прервана: пользователь с таким id не найден!");
                throw new ArgumentException("Данный пользователь не найден!");
            }

            var products = await this.GetProductsOfUserAsync(userId);

            await _productRepository.DeactivateProductsAsync(products);
            _logger.LogInformation($"Деактивация всех продуктов пользователя по id={userId} выполнена успешно!");
        }

        public async Task ActivateAllProductsOfUserAsync(int userId)
        {
            _logger.LogInformation($"Активация всех продуктов пользователя по id={userId}...");
            if (userId < 1)
            {
                _logger.LogError($"Активация не удалась:id пользователя={userId} должно быть >= 1");
                throw new ArgumentException("некорректный id!");
            }

            var target = await _userService.GetUserAsync(userId);
            if (target == null)
            {
                _logger.LogError($"Активация всех продуктов пользователя по id={userId} прервана: пользователь с таким id не найден!");
                throw new ArgumentException("Данный пользователь не найден!");
            }

            var products = await this.GetProductsOfUserAsync(userId);

            await _productRepository.ActivateProductsAsync(products);
            _logger.LogInformation($"Активация всех продуктов пользователя по id={userId} выполнена успешно!");

        }

        public async Task AddProductAsync(Product product)
        {
            _logger.LogInformation($"Добавление продукта по id={product.ID} пользователю userId={product.UserId}...");
            await _productRepository.AddAsync(product);
            _logger.LogInformation($"Добавление продукта по id={product.ID} пользователю userId={product.UserId} упешно завершено!");
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
        {//получаем только не скрытые продукты
            _logger.LogInformation("Получение всех продуктов...");
            var products= await _productRepository.GetAllAsync();
            //softDelete
            var notHiddenProducts = products.Where(p => p.IsHidden == false);
            _logger.LogInformation("Все продукты получены успешно");
            return products;
        }

        public async Task<Product?> GetProductAsync(int id)
        {
            _logger.LogInformation($"Получение продукта по id={id}...");
            if (id < 1)
            {
                _logger.LogError($"Получение продукта по id не удалось:id продукта={id} должно быть >= 1");
                throw new ArgumentException("некорректный id!");
            }
            var product = await _productRepository.GetAsync(id);
            if(product is null)
            {
                _logger.LogWarning($"продукт с id={id} не найден,возвращение null!");
                return null;
            }
            else if (product.IsHidden == true)
            {
                _logger.LogWarning($"продукт с id={id} скрыт,возвращение null!");
                return null;
            }

                _logger.LogInformation($"продукт с id={id} получен успешно!");
            return product;
        }

        public async Task<IEnumerable<Product>> GetProductsOfUserAsync(int userId)
        {
            _logger.LogInformation($"Получение всех продуктов пользователя по id={userId}...");
            var target = await _userService.GetUserAsync(userId);
            if (target == null)
            {
                throw new ArgumentException("пользователь с таким id не найден!");
            }
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
