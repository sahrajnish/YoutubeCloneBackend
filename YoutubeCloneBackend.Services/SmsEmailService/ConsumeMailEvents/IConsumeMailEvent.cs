using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoutubeCloneBackend.Services.SmsEmailService.ConsumeMailEvents
{
    public interface IConsumeMailEvent
    {
        public Task ConsumeRegisterOtpEvents(CancellationToken cancellationToken);
        public Task ConsumeNewUserEvents(CancellationToken cancellationToken);
    }
}
