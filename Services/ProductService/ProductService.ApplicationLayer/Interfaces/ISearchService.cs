using ProductService.CoreLayer.DTOs;
using ProductService.CoreLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.ApplicationLayer.Interfaces
{
    public interface ISearchService
    {
        public Task<IEnumerable<OutputProductDTO>?> SearchByNameAsync(string name);
        public Task<IEnumerable<OutputProductDTO>?> SearchByPriceAsync(decimal price);
        public Task<IEnumerable<OutputProductDTO>> FilterByPriceAscendingAsync();
        public Task<IEnumerable<OutputProductDTO>> FilterByPriceDescendingAsync();
        public Task<IEnumerable<OutputProductDTO>> FilterByNameAscendingAsync();
        public Task<IEnumerable<OutputProductDTO>> FilterByNameDescendingAsync();
    }
}
