using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoutubeCloneBackend.Services.SmsEmailService.ConsumeMailEvents
{
    public interface IConsumeMailEvent
    {
        // Consume Event for Sending OTP for new registeration
        public Task ConsumeRegisterOtpEvents(CancellationToken cancellationToken);

        // Consume Event for Sending Welcome Email to new user
        public Task ConsumeNewUserEvents(CancellationToken cancellationToken);
    }
}
