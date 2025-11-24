using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.CoreLayer;
using UserService.CoreLayer.Entities;
using UserService.InfrastrucureLayer.DBcontext;
using Microsoft.EntityFrameworkCore;
namespace UserService.InfrastrucureLayer
{
    public class UserRepository : IUserRepository
    {
        private UserDBContext _userDBContext;

        public UserRepository(UserDBContext userDBContext)
        {
            _userDBContext = userDBContext;
        }

        public async Task AddAsync(User user)
        {
            await _userDBContext.Users.AddAsync(user);
            _userDBContext.SaveChanges();
        }

        public async Task DeleteAsync(int id)
        {
            var user = await _userDBContext.Users
                .Where(u => u.ID == id)
                .ExecuteDeleteAsync();
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _userDBContext.Users
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<User?> GetAsync(int id)
        {
            return await _userDBContext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.ID == id);
        }

        public async Task UpdateAsync(int id, User newUser)
        {
            await _userDBContext.Users
                .Where(u => u.ID == id)
                .ExecuteUpdateAsync(u=>u
                .SetProperty(p=>p.EmailAddress,newUser.EmailAddress)
                .SetProperty(p=>p.Name,newUser.Name)
                .SetProperty(p=>p.Password,newUser.Password)
                .SetProperty(p=>p.HasVerifiedEmail,newUser.HasVerifiedEmail));
        }
    }
}
