using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.CoreLayer.Entities
{
    public class User
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string EmailAddress { get; set; } 
        public string Role { get; set; }
        public string Password { get; set; }
        public bool HasVerifiedEmail { get; set; }
    }
}
