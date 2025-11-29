using Microsoft.EntityFrameworkCore;
using ProductService.CoreLayer;
using ProductService.CoreLayer.Entities;
using ProductService.InfrastructureLayer.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.InfrastructureLayer
{
    public class ProductRepository : IProductRepository
    {
        private ProductDBContext _context;

        public ProductRepository(ProductDBContext context)
        {
            _context = context;
        }

        public async Task DeactivateProductsAsync(IEnumerable<Product> products)
        {
            var productsId = products.Select(p => p.ID);
            await _context.Products.Where(p=>productsId.Contains(p.ID))
                .ExecuteUpdateAsync(s => s
                .SetProperty(p => p.IsHidden, true));
        }
        public async Task ActivateProductsAsync(IEnumerable<Product> products)
        {
            var productsId = products.Select(p => p.ID);
            await _context.Products.Where(p => productsId.Contains(p.ID))
                .ExecuteUpdateAsync(s => s
                .SetProperty(p => p.IsHidden, false));
        }
        public async Task AddAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            _context.SaveChanges();
        }

        public async Task DeleteAsync(int id)
        {
            await _context.Products.Where(p => p.ID == id)
                .ExecuteDeleteAsync();
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product?> GetAsync(int id)
        {
            return await _context.Products.FirstOrDefaultAsync(p => p.ID == id);
        }

        public async Task UpdateAsync(int id, Product newProduct)
        {
            await _context.Products.Where(p => p.ID == id)
                .ExecuteUpdateAsync(s =>s
                .SetProperty(p => p.Price, newProduct.Price)
                .SetProperty(p => p.Name, newProduct.Name)
                .SetProperty(p=>p.Description,newProduct.Description)
                .SetProperty(p=>p.IsHidden,newProduct.IsHidden));
        }
    }
}
