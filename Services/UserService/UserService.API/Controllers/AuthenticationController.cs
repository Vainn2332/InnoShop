using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using UserService.API.Validators;
using UserService.ApplicationLayer.Interfaces;
using UserService.CoreLayer.DTOs;
using UserService.CoreLayer.Entities;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UserService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private IUsersService _userService;
        private IEmailService _emailService;
        private IAuthService _authService;
        private IAdminService _adminService;
        private IPasswordService _passwordService;
        private UserValidator _userValidator;
        public AuthenticationController(IUsersService userService, IEmailService emailService,
            IAuthService authService, IAdminService adminService, IPasswordService passwordService,
            UserValidator userValidator)
        {
            _userService = userService;
            _emailService = emailService;
            _authService = authService;
            _adminService = adminService;
            _passwordService = passwordService;
            _userValidator = userValidator;
        }
        // POST api/<Users/register>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDTO userRegisterDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (await _userValidator.ExistsAsync(userRegisterDTO.EmailAddress))
            {
                return BadRequest("Такой пользователь уже существует!");
            }

            var notRegistredUser = new User(userRegisterDTO);
            notRegistredUser.Password = _passwordService.HashPassword(notRegistredUser.Password);
            await _userService.AddUserAsync(notRegistredUser);

            var confirmLink = Url.Action("ConfirmRegistration", "Authentication"
                , new
                {
                    EmailAddress = userRegisterDTO.EmailAddress,
                }, Request.Scheme);

            await _emailService.SendConfirmRegistrationEmailAsync(userRegisterDTO.EmailAddress, confirmLink);

            return Ok("Подтвердите почту для регистрации");
        }

        [HttpGet("ConfirmRegistration")]
        public async Task<IActionResult> ConfirmRegistration([EmailAddress] string EmailAddress)
        {//как будто небезопасно
            if (string.IsNullOrEmpty(EmailAddress))
            {
                return BadRequest("неверная ссылка подтверждения!");
            }

            var target = await _userService.GetUserByEmailAsync(EmailAddress);
            if (target == null)
            {
                return BadRequest("Данный пользователь не найден!");
            }
            if (target.HasVerifiedEmail == true)
            {
                return BadRequest("Вы уже подтвердили данную почту!");
            }
            await _adminService.ActivateUserAsync(target.ID);

            var encodedJWT = _authService.GenerateJWT(target.ID, target.EmailAddress);
            return Ok(encodedJWT);
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO userLoginDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var target = await _userService.GetUserByEmailAsync(userLoginDTO.EmailAddress);
            if (target == null)//если не существует
            {
                return BadRequest("Такого пользователя не существует!");
            }
            if (!_passwordService.VerifyPassword(userLoginDTO.Password, target.Password))
            {
                return BadRequest("Неправильный Пароль!");
            }
            if (target.HasVerifiedEmail == false)
            {
                var confirmLink = Url.Action("ConfirmRegistration", "Authentication"
                , new
                {
                    EmailAddress = userLoginDTO.EmailAddress,
                }, Request.Scheme);
                await _emailService.SendUserActivationEmailAsync(userLoginDTO.EmailAddress, confirmLink);
                return Ok("Ваш аккаунт был деактивирован.Для активации подтвердите почту");
            }

            var encodedJWT = _authService.GenerateJWT(target.ID, target.EmailAddress);

            return Ok(encodedJWT);
        }


        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] UserConfirmPasswordDTO userConfirmPasswordDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Введённые данные не соответствуют почте!");
            }
            await _userValidator.ValidateForgotPasswordAsync(userConfirmPasswordDTO.Password);

            string? resetPasswordLink = Url.Action("ConfirmNewPassword", "Authentication", new
            {
                Password = userConfirmPasswordDTO.Password,
                EmailAddress = userConfirmPasswordDTO.EmailAddress
            }, Request.Scheme);
            await _emailService.SendResetPasswordEmailAsync(userConfirmPasswordDTO.EmailAddress, resetPasswordLink);
            return Ok("инструкция по сбросу пароля отправлена на почту");
        }

        [HttpGet("ConfirmNewPassword")]
        public async Task<IActionResult> ConfirmNewPassword(string newPassword, [EmailAddress] string EmailAddress)
        {//тоже как будто небезопасно
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var target = await _userService.GetUserByEmailAsync(EmailAddress);
            if (target == null)
            {
                return BadRequest("Пользователь с такой почтой не найден!");
            }

            target.Password = _passwordService.HashPassword(newPassword);
            await _userService.UpdateUserAsync(target.ID, target);

            var encodedJWT = _authService.GenerateJWT(target.ID, target.EmailAddress);
            return Ok(encodedJWT);
        }
    }
}
