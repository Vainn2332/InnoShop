using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductService.ApplicationLayer.Interfaces;
using ProductService.CoreLayer.DTOs;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProductService.API.Controllers
{
    [Route("api/search")]
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
        public async Task<IActionResult> SearchByPriceAsync(SearchByPriceDTO searchByPriceDTO)
        {
            if (!ModelState.IsValid)
            {
                BadRequest(ModelState);
            }
            var target= await _searchService.SearchByPriceAsync(searchByPriceDTO.Price);
            if (target == null)
            {
                return NotFound("Товаров с таким именем не найдено!");
            }
            return Ok(target);
        }

        [HttpPost("searchByName")]
        [Authorize]
        public async Task<IActionResult> SearchByNameAsync(SearchByNameDTO searchByNameDTO)
        {
            if (!ModelState.IsValid)
            {
                BadRequest(ModelState);
            }

            char[] charName = searchByNameDTO.Name.ToCharArray();
            charName[0] = char.ToUpper(charName[0]);
            string modifiedName = new string(charName);//чтобы поиск и вставка были все с заглавной буквы

            var target = await _searchService.SearchByNameAsync(modifiedName);
            if (target == null)
            {
                return NotFound("Товаров с таким именем не найдено!");
            }
            return Ok(target);
        }
    }
}
