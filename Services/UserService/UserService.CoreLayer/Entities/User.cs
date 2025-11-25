using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.CoreLayer.DTOs;

namespace UserService.CoreLayer.Entities
{
    public class User
    {
        public int ID { get; set; } = 0;
        public string Name { get; set; } = String.Empty;
        public string EmailAddress { get; set; } = String.Empty;
        public string Role { get; set; } = "user";
        public string Password { get; set; }
        public bool HasVerifiedEmail { get; set; } = false;
        public User(UserRegisterDTO userRegisterDTO)
        {
            Name = userRegisterDTO.Name;
            EmailAddress = userRegisterDTO.EmailAddress;
            Password = userRegisterDTO.Password;
        }
        public User(PutUserDTO putUserDTO)
        {
            Name = putUserDTO.Name;
            EmailAddress = putUserDTO.EmailAddress;
            Password = putUserDTO.Password;
        }
        public User()
        {

        }
    }
}
