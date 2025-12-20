using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserService.ApplicationLayer.Interfaces
{
    public interface IEmailService
    {
        public Task SendEmailAsync(string receiverEmail, string subject, string body);
        public Task SendConfirmRegistrationEmailAsync(string receiverEmail, string confirmationLink);
        public Task SendResetPasswordEmailAsync(string receiverEmail, string confirmationLink);
        public Task SendUserActivationEmailAsync(string receiverEmail, string confirmationLink);
    }
}
