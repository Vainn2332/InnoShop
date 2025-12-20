using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserService.API.Validators;
using UserService.ApplicationLayer.Interfaces;
using UserService.CoreLayer.DTOs;
using UserService.CoreLayer.Entities;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UserService.API.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IUsersService _userService;
        private IPasswordService _passwordService;
        private UserValidator _userValidator;
        private IAuthService _authService;
        public UsersController(IUsersService userService, IPasswordService passwordService, UserValidator userValidator, IAuthService authService)
        {
            _userService = userService;
            _passwordService = passwordService;
            _userValidator = userValidator;
            _authService = authService;
        }

        // GET: api/<Users>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get()
        {
            return Ok(await _userService.GetAllUsersAsync());
        }

        // GET api/<Users>/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var target = await _userService.GetUserAsync(id);
            if (target == null) 
            {
                return NotFound("Пользователь не найден!");
            }
            return Ok(target);
        }

        // PUT api/<Users>/5
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Put(int id, [FromBody] PutUserDTO putUserDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!_userValidator.ValidatePassword(putUserDTO.Password))
            {
                return BadRequest("Неправильный формат пароля!");
            }
            if (!await _userValidator.ExistsAsync(id))
            {
                return BadRequest("Пользователь с таким id не существует!");
            }

            var target =await _userService.GetUserByEmailAsync(putUserDTO.EmailAddress);
            if (target!=null&&target.ID!=id)
            {
                return BadRequest("данный Email уже занят другим пользователем!");
            }

            var jwt = _authService.GetJWTFromHeader(Request);
            JWTInfo jwtInfo=_authService.ParseJWT(jwt);

            if (jwtInfo.UserId != target.ID)
            {
                return BadRequest("Вы не можете изменять параметры чужого пользователя");
            }

            User user = new User(putUserDTO)
            {
                Password = _passwordService.HashPassword(putUserDTO.Password),
                HasVerifiedEmail = true//впринципе же только авторизованные пользователи могут менять свои личные данные
            };
            await _userService.UpdateUserAsync(id, user);
            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var jwt = _authService.GetJWTFromHeader(Request);
            JWTInfo jwtInfo = _authService.ParseJWT(jwt);

            var target = await _userService.GetUserAsync(id);
            if (jwtInfo.UserId != target.ID)
            {
                return BadRequest("Вы не можете изменять параметры чужого пользователя");
            }

            await _userService.DeleteUserAsync(id);
            return Ok();
        }
    }
}
