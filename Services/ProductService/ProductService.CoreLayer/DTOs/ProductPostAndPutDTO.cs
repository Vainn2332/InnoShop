using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.CoreLayer.DTOs
{
    public class ProductPostAndPutDTO
    {
        [Required(ErrorMessage = "Поле имя обязательно")]
        [StringLength(30,ErrorMessage ="Максимальная длина названия должна быть не более 30")]
        public string Name { get; set; } = string.Empty;


        [Required(ErrorMessage = "Поле Описание обязательно")]
        [StringLength(100, ErrorMessage = "Максимальная длина описания товара должна быть не более 30")]
        public string Description { get; set; } = string.Empty;


        [Required(ErrorMessage = "Поле Цена обязательно")]
        [Range(0.01, 9e+007,ErrorMessage ="Цена должна быть от 0.01 до 9*10^7.00")]
        [RegularExpression(@"^\d{1,8}(\.\d{1,2})?$",ErrorMessage ="число знаков после запятой не больше 2")]
        public decimal Price { get; set; } = 0;
    }
}
