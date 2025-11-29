using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductService.ApplicationLayer.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProductService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly ISearchService _searchService;

        public SearchController(ISearchService searchService)
        {
            _searchService = searchService;
        }

        [HttpGet("filterByNameAscending")]
        [Authorize]
        public async Task<IActionResult> FilterByNameAscendingAsync()
        {
            return Ok(await _searchService.FilterByNameAscendingAsync());
        }

        [HttpGet("filterByNameDescending")]
        [Authorize]
        public async Task<IActionResult> FilterByNameDescendingAsync()
        {
            return Ok(await _searchService.FilterByNameDescendingAsync());
        }

        [HttpGet("filterByPriceAscending")]
        [Authorize]
        public async Task<IActionResult> FilterByPriceAscendingAsync()
        {
            return Ok(await _searchService.FilterByPriceAscendingAsync());
        }

        [HttpGet("filterByPriceDescending")]
        [Authorize]
        public async Task<IActionResult> FilterByPriceDescendingAsync()
        {
            return Ok(await _searchService.FilterByPriceDescendingAsync());
        }

        [HttpPost("searchByPrice")]
        [Authorize]
        public async Task<IActionResult> SearchByPriceAsync(decimal price)
        {
            if (price <= 0)
            {
                BadRequest("Некорректно введена цена!");
            }
            return Ok(await _searchService.SearchByPriceAsync(decimal.Round(price, 2)));
        }

        [HttpPost("searchByName")]
        [Authorize]
        public async Task<IActionResult> SearchByNameAsync(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                BadRequest("Некорректно введена цена!");
            }

            return Ok(await _searchService.SearchByNameAsync(name));
        }
    }
}
