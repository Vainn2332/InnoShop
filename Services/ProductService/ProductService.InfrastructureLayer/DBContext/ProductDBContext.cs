using Microsoft.EntityFrameworkCore;
using ProductService.CoreLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.InfrastructureLayer.DBContext
{
    public class ProductDBContext:DbContext
    {
        private DbContextOptions<ProductDBContext> _options;

        public ProductDBContext(DbContextOptions<ProductDBContext> options):base(options)
        {
            _options = options;
        }

        public DbSet<Product> Products { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
        }
    }
}
