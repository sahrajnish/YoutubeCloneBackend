using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoutubeCloneBackend.Services.SmsEmailService.MailingService.Mails
{
    public interface IMail
    {
        public Task<bool> SendRegisterOtp(string email, string otp);
        public Task<bool> SendWelcomeEmail(string email);
    }
}
