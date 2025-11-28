using Microsoft.AspNetCore.Mvc;
using ProductService.ApplicationLayer.Interfaces;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProductService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        // GET: api/<TestController>
        private IUserService _userService;

        public TestController(IUserService userService)
        {
            _userService = userService;
        }

       

        // GET api/<TestController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await _userService.GetUserAsync(id));
        }

       
    }
}
