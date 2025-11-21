using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoutubeCloneBackend.Services.UserServices.PublishMailEvents
{
    public interface IPublishMailEvent
    {
        public Task SendWelcomeEmailToUser(string email);
        public Task SendOtpToUser(string purpose, string email, string otp);
    }
}
