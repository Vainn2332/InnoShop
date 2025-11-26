using ProductService.CoreLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.ApplicationLayer.Interfaces
{
    public interface IProductsService
    {
        public Task<bool> CheckPossessionAsync(int productId, int userId);
        public Task<IEnumerable<Product>> GetAllProductsAsync();
        public Task<Product?> GetProductAsync(int id);
        public Task AddProductAsync(Product product);
        public Task DeleteProductAsync(int id);
        public Task UpdateProductAsync(int id, Product newProduct);
    }
}
