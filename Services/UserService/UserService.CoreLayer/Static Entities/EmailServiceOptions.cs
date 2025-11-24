using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserService.CoreLayer.Static_Entities
{
    public class EmailServiceOptions
    {
        public const string EMAIL_SENDER = "vladuk2332@gmail.com";
        public const string APP_PASSWORD = "phbl xkqp cowe txbq";
        public const string SENDER_NAME = "InnoShop";

        public const int PORT = 587;
        public const string SMTP_CLIENT = "smtp.gmail.com";



        public const string CONFIRM_REGISTRATION_MESSAGE =
            $"@<p> Для подтверждения регистрации на сайте {SENDER_NAME}, нужно подтвердить почту</p>" +
            "<p>Для этого перейдите по этой ссылке: </p>";

        public const string CONFIRM_PASSWORD_CHANGE_MESSAGE = $"<p>Для смены пароля на сайте {SENDER_NAME}, перейдите по этой ссылке:</p>";

        public const string CONFIRM_USER_ACTIVATION_MESSAGE =
            $"@<p> Для активации аккаунта на сайте {SENDER_NAME}, нужно подтвердить почту</p>" +
            "<p>Для этого перейдите по этой ссылке: </p>";
    }
}
