using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoutubeCloneBackend.Services.SmsEmailService.MailingService.MailingOtps
{
    public interface IMailOtp
    {
        public Task<bool> SendRegisterOtp(string email, string otp);
    }
}
