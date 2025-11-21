using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoutubeCloneBackend.Core.User;

namespace YoutubeCloneBackend.Services.UserServices.PublishMailEvents
{
    public interface IPublishMailEvent
    {
        public Task NotifyUser(NotificationEvent eventDetails);
        public Task SendOtpToUser(OtpEvent eventDetails);
    }
}
