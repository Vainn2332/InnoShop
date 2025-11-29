using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.CoreLayer.DTOs
{
    public class SearchByNameDTO
    {
        [Required(ErrorMessage = "Поле имя обязательно")]
        [StringLength(30, ErrorMessage = "Максимальная длина названия должна быть не более 30")]
        public string Name { get; set; } = string.Empty;
    }
}
