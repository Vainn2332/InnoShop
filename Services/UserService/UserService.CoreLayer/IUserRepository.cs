using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.CoreLayer.Entities;

namespace UserService.CoreLayer
{
    public interface IUserRepository
    {
        public Task<IEnumerable<User>> GetAllAsync();
        public Task<User?> GetAsync(int id);
        public Task AddAsync(User user);
        public Task UpdateAsync(int id, User newUser);
        public Task DeleteAsync(int id);
    }
}
