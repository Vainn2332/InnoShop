using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.CoreLayer.Entities;

namespace UserService.ApplicationLayer.Interfaces
{
    public interface IUserService
    {
        public Task<IEnumerable<User>> GetAllUsersAsync();
        public Task<User?> GetUserAsync(int id);
        public Task<User?> GetUserByEmailAsync(string email);
        public Task AddUserAsync(User user);
        public Task DeleteUserAsync(int id);
        public Task UpdateUserAsync(int id, User newUser);
    }
}
