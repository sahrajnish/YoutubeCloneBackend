using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoutubeCloneBackend.Services.SmsEmailService.PublishMailEvents
{
    public interface IPublishRegisterOtpMailEvent
    {
        public Task PublishEventToSendRegisterOtp(string email, string otp);
    }
}
