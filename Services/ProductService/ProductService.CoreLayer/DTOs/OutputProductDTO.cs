using ProductService.CoreLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.CoreLayer.DTOs
{
    public class OutputProductDTO
    {
        public int ID { get; set; } = 0;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; } = 0;
        public DateTime DateOfCreation { get; set; } = DateTime.UtcNow;
        public int UserId { get; set; } = 0;

        public OutputProductDTO(Product product)
        {
            this.ID = product.ID;
            this.Name = product.Name;
            this.Description = product.Description;
            this.Price = product.Price;
            this.DateOfCreation = product.DateOfCreation;
            this.UserId = product.UserId;
        }
        public OutputProductDTO()
        {
                
        }
    }
}
