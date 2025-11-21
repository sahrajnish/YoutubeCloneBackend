using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoutubeCloneBackend.Services.SmsEmailService.ConsumeMailEvents
{
    public interface IConsumeMailEvent
    {
        // Consume Event for Sending Welcome Email to new user
        public Task ConsumeNewUserEvents(CancellationToken cancellationToken);

        // Consume Event to send OTP Based on Purpose - "register", "reset"
        public Task ConsumeOtpEvents(CancellationToken cancellationToken);
    }
}
