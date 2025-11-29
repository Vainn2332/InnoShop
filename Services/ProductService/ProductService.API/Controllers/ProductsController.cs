using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductService.ApplicationLayer.Interfaces;
using ProductService.CoreLayer.DTOs;
using ProductService.CoreLayer.Entities;

namespace ProductService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        // GET: api/<ProductsController>
        private IProductsService _productService;
        private IAuthService _authService;
        private IUserService _userService;
        public ProductsController(IProductsService productService, IAuthService authService, IUserService userService)
        {
            _productService = productService;
            _authService = authService;
            _userService = userService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get()
        {
            var products = await _productService.GetAllProductsAsync();

            var targets = products.Select(p => new OutputProductDTO(p));//избавляемся от св-ва isHidden
            return Ok(targets);
        }

        // GET api/<ProductsController>/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> Get(int id)
        {
            var product = await _productService.GetProductAsync(id);
            if (product == null)
            {
                return NotFound("Продукт с таким id не найден!");
            }
            var target = new OutputProductDTO(product);
            return Ok(target);
        }

        [Authorize]
        [HttpGet("ProductsOfUser")]
        public async Task<IActionResult> ShowProductsOfUser(int userId)
        {
            var products = await _productService.GetProductsOfUserAsync(userId);

            var targets = products.Select(p => new OutputProductDTO(p));//избавляемся от св-ва isHidden
            return Ok(targets);
        }

        // POST api/<ProductsController>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Post([FromBody] ProductPostAndPutDTO productDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var jwt = _authService.GetJWTFromHeader(Request);
            JWTInfo userInfo = _authService.ParseJWT(jwt);//получаем данные пользователя(email и userID) из JWT

            Product product = new Product(productDTO)
            {
                UserId = userInfo.UserId//проверить работоспособность
            };
            await _productService.AddProductAsync(product);
            return Ok();
        }

        // PUT api/<ProductsController>/5
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Put(int id, [FromBody] ProductPostAndPutDTO newProductPutDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var jwt = _authService.GetJWTFromHeader(Request);
            JWTInfo userInfo = _authService.ParseJWT(jwt);//получаем данные пользователя(email и userID) из JWT

            //если данный продукт не принадлежит текущему пользователю
            if (!await _productService.CheckPossessionAsync(id, userInfo.UserId))
            {
                return Unauthorized("Вы не можете изменять чужой продукт!");
            }

            Product newProduct = new Product(newProductPutDTO)
            {
                UserId = userInfo.UserId
            };
            await _productService.UpdateProductAsync(id, newProduct);
            return Ok();
        }


        // DELETE api/<ProductsController>/5
        [HttpDelete("{productId}")]
        [Authorize]
        public async Task<IActionResult> Delete(int productId)
        {
            var jwt = _authService.GetJWTFromHeader(Request);
            JWTInfo userInfo = _authService.ParseJWT(jwt);//получаем данные пользователя(email и userID) из JWT

            var user = await _userService.GetUserAsync(userInfo.UserId);
            if (user.Role == "admin")
            {
                await _productService.DeleteProductAsync(productId);//админ имеет право удалять чужие продукты
                return Ok();
            }
            //если данный продукт не принадлежит текущему пользователю
            if (!await _productService.CheckPossessionAsync(productId, userInfo.UserId))
            {
                return Unauthorized("Вы не можете удалить чужой продукт!");
            }
            await _productService.DeleteProductAsync(productId);
            return Ok();
        }

        [Authorize]
        [HttpPost("DeactivateProductsOfUser")]
        public async Task<IActionResult> DeactivateProductsOfUser([FromBody]int userId)
        {
            await _productService.DeactivateAllProductsOfUserAsync(userId);
            return Ok();
        }

        [Authorize]
        [HttpPost("ActivateProductsOfUser")]
        public async Task<IActionResult> ActivateProductsOfUser([FromBody]int userId)
        {
            await _productService.ActivateAllProductsOfUserAsync(userId);
            return Ok();
        }       
    }
}
