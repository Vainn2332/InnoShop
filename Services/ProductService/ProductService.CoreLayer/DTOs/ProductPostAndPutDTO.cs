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
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Поле Описание обязательно")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Поле Цена обязательно")]
        [Range(0, double.MaxValue)]
        [Column(TypeName = "decimal(9,2)")]
        public decimal Price { get; set; } = 0;
    }
}
