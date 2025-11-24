using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserService.CoreLayer.DTOs
{
    public class UserRegisterDTO
    {
        [Required(ErrorMessage = "Не указано имя пользователя!")]
        public string Name { get; set; } = String.Empty;

        [Required(ErrorMessage = "Не указан адрес")]
        [EmailAddress]
        public string EmailAddress { get; set; } = String.Empty;

        [Required(ErrorMessage = "Не указан пароль")]
        public string Password { get; set; }
    }
}
