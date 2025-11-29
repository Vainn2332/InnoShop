using ProductService.ApplicationLayer.Interfaces;
using ProductService.CoreLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.ApplicationLayer
{
    public class SearchService : ISearchService
    {
        private IProductsService _productService;

        public SearchService(IProductsService productsService)
        {
            _productService = productsService;
        }

        public async Task<IEnumerable<OutputProductDTO>> FilterByNameAscendingAsync()
        {
            var products = await _productService.GetAllProductsAsync();
            var orderedProducts = products.OrderBy(p => p.Name)
                .Select(p => new OutputProductDTO(p));

            return orderedProducts;
        }

        public async Task<IEnumerable<OutputProductDTO>> FilterByNameDescendingAsync()
        {
            var products = await _productService.GetAllProductsAsync();
            var orderedProducts = products.OrderByDescending(p => p.Name)
                .Select(p => new OutputProductDTO(p));

            return orderedProducts;
        }

        public async Task<IEnumerable<OutputProductDTO>> FilterByPriceAscendingAsync()
        {
            var products = await _productService.GetAllProductsAsync();
            var orderedProducts = products.OrderBy(p => p.Price)
                .Select(p => new OutputProductDTO(p));

            return orderedProducts;
        }

        public async Task<IEnumerable<OutputProductDTO>> FilterByPriceDescendingAsync()
        {
            var products = await _productService.GetAllProductsAsync();
            var orderedProducts = products.OrderByDescending(p => p.Price)
                .Select(p => new OutputProductDTO(p));

            return orderedProducts;
        }

        public async Task<IEnumerable<OutputProductDTO>?> SearchByNameAsync(string name)
        {           
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Строка поиска не может быть пустой");
            }

            var products = await _productService.GetAllProductsAsync();
            var target = products.Where(p => p.Name == name)
                .Select(p => new OutputProductDTO(p));

            if (target == null)
            {
                throw new ArgumentException("Продуктов с таким именем не найдено!");
            }
            return target;
        }

        public async Task<IEnumerable<OutputProductDTO>?> SearchByPriceAsync(decimal price)
        {
            var products = await _productService.GetAllProductsAsync();
            var target = products.Where(p => p.Price >= (int)(price - 1) && p.Price <= (int)(price + 1))//диапазон +-1
                .Select(p => new OutputProductDTO(p));
            if (target == null)
            {
                throw new ArgumentException("Продукты с такой ценой не найдены!");
            }
            return target;
        }
    }
}
