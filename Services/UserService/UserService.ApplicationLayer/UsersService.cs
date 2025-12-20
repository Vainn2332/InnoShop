using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.ApplicationLayer.Interfaces;
using UserService.CoreLayer;
using UserService.CoreLayer.Entities;

namespace UserService.ApplicationLayer
{
    public class UsersService:IUsersService
    {
        private IUserRepository _userRepository;

        public UsersService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }


        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllAsync();
        }
     
        public async Task<User?> GetUserAsync(int id)
        {
            if (id < 1)
                throw new ArgumentException("id не может быть <1");
            return await _userRepository.GetAsync(id);
        }
        public async Task AddUserAsync(User user)
        {
            await _userRepository.AddAsync(user);
        }
        public async Task DeleteUserAsync(int id)
        {
            if (id < 1)
            {
                throw new ArgumentException("id не может быть <1");
            }

            var target = await _userRepository.GetAsync(id);
            if (target == null)
            {
                throw new ArgumentException("пользователь с таким id не найден");
            }
            await _userRepository.DeleteAsync(id);
        }
        public async Task UpdateUserAsync(int id, User newUser)
        {
            if (id < 1)
                throw new ArgumentException("id не может быть <1");
            await _userRepository.UpdateAsync(id, newUser);
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {          
            var users = await _userRepository.GetAllAsync();

            var target = users.FirstOrDefault(u => u.EmailAddress == email);

            return target;
        }
    }
}
