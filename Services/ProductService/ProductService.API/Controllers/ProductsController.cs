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
        public ProductsController(IProductsService productService, IAuthService authService)
        {
            _productService = productService;
            _authService = authService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get()
        {
            return Ok(await _productService.GetAllProductsAsync());
        }

        // GET api/<ProductsController>/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await _productService.GetProductAsync(id));
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
        public async Task<IActionResult> DeactivateProductsOfUser(int userId)
        {
            await _productService.DeactivateAllProductsOfUserAsync(userId);
            return Ok();
        }

        [Authorize]
        [HttpPost("ActivateProductsOfUser")]
        public async Task<IActionResult> ActivateProductsOfUser(int userId)
        {
            await _productService.ActivateAllProductsOfUserAsync(userId);
            return Ok();
        }
    }
}
