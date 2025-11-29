using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.CoreLayer.DTOs
{
    public class SearchByPriceDTO
    {
        [Required(ErrorMessage = "Поле Цена обязательно")]
        [Range(0.01, 9e+007, ErrorMessage = "Цена должна быть от 0.01 до 9*10^7.00")]
        [RegularExpression(@"^\d{1,8}(\.\d{1,2})?$", ErrorMessage = "число знаков после запятой не больше 2")]
        public decimal Price { get; set; } = 0;
    }
}
