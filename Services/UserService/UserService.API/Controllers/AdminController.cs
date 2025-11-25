using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserService.ApplicationLayer.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UserService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
        private readonly IUsersService _userService;
        private readonly IAuthService _authService;
        public AdminController(IAdminService adminService, IUsersService userService, IAuthService authService)
        {
            _adminService = adminService;
            _userService = userService;
            _authService = authService;
        }

        [HttpPost("deactivate")]
        [Authorize]
        public async Task<IActionResult> DeactivateUserAsync(int id)
        {
            if (id < 1)
            {
                return BadRequest("неправильно введён id!");
            }

            var jwt = _authService.GetJWTFromHeader(Request);
            var userInfo = _authService.ParseJWT(jwt);

            var currentUser = await _userService.GetUserAsync(userInfo.UserId);//проверка прав текущего пользователя
            if (currentUser.Role != "admin")
            {
                return Unauthorized("У вас недостаточно прав");
            }

            var target = await _userService.GetUserAsync(id);
            if (target == null)
            {
                throw new ArgumentException("Пользователь не найден!");
            }
            await _adminService.DeactivateUserAsync(id);
            return Ok();
        }

        [HttpPost("activate")]
        [Authorize]
        public async Task<IActionResult> ActivateUserAsync(int id)
        {
            if (id < 1)
            {
                return BadRequest("неправильно введён id!");
            }

            var jwt = _authService.GetJWTFromHeader(Request);
            var userInfo = _authService.ParseJWT(jwt);

            var currentUser = await _userService.GetUserAsync(userInfo.UserId);//проверка прав текущего пользователя
            if (currentUser.Role != "admin")
            {
                return Unauthorized("У вас недостаточно прав");
            }

            var target = await _userService.GetUserAsync(id);
            if (target == null)
            {
                throw new ArgumentException("Пользователь не найден!");
            }
            await _adminService.ActivateUserAsync(id);
            return Ok();
        }
    }
}
