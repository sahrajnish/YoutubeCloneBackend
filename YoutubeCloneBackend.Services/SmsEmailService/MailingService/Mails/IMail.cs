using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoutubeCloneBackend.Core.User;

namespace YoutubeCloneBackend.Services.SmsEmailService.MailingService.Mails
{
    public interface IMail
    {
        public Task<bool> SendOtpEmail(OtpPurpose purpose, string email, string otp);
        public Task<bool> SendNotificationEmail(NotificationPurpose purpose, string email);
    }
}
