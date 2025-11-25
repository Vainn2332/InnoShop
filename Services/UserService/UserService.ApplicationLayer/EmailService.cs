using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using UserService.ApplicationLayer.Interfaces;
using UserService.CoreLayer.Static_Entities;

namespace UserService.ApplicationLayer
{
    public class EmailService : IEmailService
    {
        private readonly ILogger<EmailService> _logger;

        public EmailService(ILogger<EmailService> logger)
        {
            _logger = logger;
        }


        public async Task SendEmailAsync(string receiverEmail, string subject, string body)
        {
            _logger.LogInformation($"отправка почты адресату {receiverEmail}");
            var mail = new MailMessage();
            mail.IsBodyHtml = true;
            mail.From = new MailAddress("testAuthentication@example.com", "AutoPoster");
            mail.To.Add(receiverEmail);
            mail.Subject = subject;//было "Проверка пароля";
            mail.Body = body;
            using SmtpClient client = new SmtpClient(EmailServiceOptions.SMTP_CLIENT);

            client.Port = EmailServiceOptions.PORT;
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential(EmailServiceOptions.EMAIL_SENDER, EmailServiceOptions.APP_PASSWORD);
            await client.SendMailAsync(mail);
            _logger.LogInformation($"Почта успешно отправлена!");
        }
        public async Task SendConfirmRegistrationEmailAsync(string receiverEmail, string confirmationLink)
        {
            _logger.LogInformation($"отправка подтверждения почты адресату {receiverEmail}");
            var mail = new MailMessage();
            mail.IsBodyHtml = true;
            mail.From = new MailAddress("testAuthentication@example.com", EmailServiceOptions.SENDER_NAME);
            mail.To.Add(receiverEmail);
            mail.Subject = "Подтверждение почты";//было "Проверка пароля";
            mail.Body = EmailServiceOptions.CONFIRM_REGISTRATION_MESSAGE + $"<a href={confirmationLink}>Подтвердить регистрацию</a>";
            using SmtpClient client = new SmtpClient(EmailServiceOptions.SMTP_CLIENT);

            client.Port = EmailServiceOptions.PORT;
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential(EmailServiceOptions.EMAIL_SENDER, EmailServiceOptions.APP_PASSWORD);
            await client.SendMailAsync(mail);
            _logger.LogInformation($"Почта успешно отправлена!");
        }

        public async Task SendResetPasswordEmailAsync(string receiverEmail, string confirmationLink)
        {
            _logger.LogInformation($"отправка почты смены пароля адресату {receiverEmail}");
            var mail = new MailMessage();
            mail.IsBodyHtml = true;
            mail.From = new MailAddress("testAuthentication@example.com", EmailServiceOptions.SENDER_NAME);
            mail.To.Add(receiverEmail);
            mail.Subject = "Смена пароля";
            mail.Body = EmailServiceOptions.CONFIRM_PASSWORD_CHANGE_MESSAGE + $"<a href={confirmationLink}>Сменить пароль</a>";
            using SmtpClient client = new SmtpClient(EmailServiceOptions.SMTP_CLIENT);

            client.Port = EmailServiceOptions.PORT;
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential(EmailServiceOptions.EMAIL_SENDER, EmailServiceOptions.APP_PASSWORD);
            await client.SendMailAsync(mail);
            _logger.LogInformation($"Почта успешно отправлена!");
        }
        public async Task SendUserActivationEmailAsync(string receiverEmail, string confirmationLink)
        {
            _logger.LogInformation($"отправка почты активации аккаунта адресату {receiverEmail}");
            var mail = new MailMessage();
            mail.IsBodyHtml = true;
            mail.From = new MailAddress("testAuthentication@example.com", EmailServiceOptions.SENDER_NAME);
            mail.To.Add(receiverEmail);
            mail.Subject = "Активация аккаунта";
            mail.Body = EmailServiceOptions.CONFIRM_USER_ACTIVATION_MESSAGE + $"<a href={confirmationLink}>Сменить пароль</a>";
            using SmtpClient client = new SmtpClient(EmailServiceOptions.SMTP_CLIENT);

            client.Port = EmailServiceOptions.PORT;
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential(EmailServiceOptions.EMAIL_SENDER, EmailServiceOptions.APP_PASSWORD);
            await client.SendMailAsync(mail);
            _logger.LogInformation($"Почта успешно отправлена!");
        }
    }
}
