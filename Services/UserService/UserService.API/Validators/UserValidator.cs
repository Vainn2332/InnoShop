using UserService.ApplicationLayer.Interfaces;
using UserService.CoreLayer.DTOs;

namespace UserService.API.Validators
{
    public class UserValidator
    {
        private IUsersService _userService;

        public UserValidator(IUsersService userService)
        {
            _userService = userService;
        }

        public async Task ValidateForgotPasswordAsync(string emailAddress)
        {
            var target = await _userService.GetUserByEmailAsync(emailAddress);
            if (target == null)
            {
                throw new ArgumentException("Такого пользователя не существует!");
            }
            if (target.HasVerifiedEmail == false)
            {
                throw new ArgumentException("Аккаунт ещё не авторизован!");
            }            
        }

        public async Task<bool> ExistsAsync(string emailAddress)
        {
            var target = await _userService.GetUserByEmailAsync(emailAddress);
            if (target == null)
            {
                return false;
            }
            return true;
        }
    }
}
