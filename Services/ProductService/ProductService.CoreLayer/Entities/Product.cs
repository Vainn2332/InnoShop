using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.CoreLayer.Entities
{
    public class Product
    {
        public int ID { get; set; } = 0;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; } = 0;
        public DateTime DateOfCreation { get; set; } = DateTime.UtcNow;

        public bool IsHidden { get; set; }


        public int UserId { get; set; } = 0;
    }
}
