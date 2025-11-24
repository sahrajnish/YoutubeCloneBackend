using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoutubeCloneBackend.Core.User;

namespace YoutubeCloneBackend.Services.SmsEmailService.PublishMailEvents
{
    public interface IPublishRegisterOtpMailEvent
    {
        public Task PublishEventToSendOtp(OtpEvent eventDetails);
    }
}
