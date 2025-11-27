using ProductService.CoreLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.ApplicationLayer.Interfaces
{
    public interface IUserService
    {
        public Task<User?> GetUserAsync(int id);
        public Task<IEnumerable<User>> GetAllUsersAsync();
    }
}
