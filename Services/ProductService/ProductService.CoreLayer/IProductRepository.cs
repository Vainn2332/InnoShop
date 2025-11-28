using ProductService.CoreLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.CoreLayer
{
    public interface IProductRepository
    {
        public  Task ActivateProductsAsync(IEnumerable<Product> products);
        public Task DeactivateProductsAsync(IEnumerable<Product> products);
        public Task<IEnumerable<Product>> GetAllAsync();
        public Task<Product?> GetAsync(int id);
        public Task AddAsync(Product product);
        public Task UpdateAsync(int id, Product newProduct);
        public Task DeleteAsync(int id);
    }
}
