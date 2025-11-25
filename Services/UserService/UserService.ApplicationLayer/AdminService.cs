using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.ApplicationLayer.Interfaces;

namespace UserService.ApplicationLayer
{
    public class AdminService : IAdminService
    {
        private readonly IUserService _userService;

        public AdminService(IUserService userService)
        {
            _userService = userService;
        }

        public async Task ActivateUserAsync(int id)
        {
            var user = await _userService.GetUserAsync(id);
            user.HasVerifiedEmail = true;
            await _userService.UpdateUserAsync(id, user);
        }

        public async Task DeactivateUserAsync(int id)
        {
            var user = await _userService.GetUserAsync(id);
            user.HasVerifiedEmail = false;
            await _userService.UpdateUserAsync(id, user);
        }
    }
}
