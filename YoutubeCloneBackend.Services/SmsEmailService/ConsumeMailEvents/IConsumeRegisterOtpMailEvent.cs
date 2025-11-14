using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoutubeCloneBackend.Services.SmsEmailService.ConsumeMailEvents
{
    public interface IConsumeRegisterOtpMailEvent
    {
        public Task ConsumeEvents(CancellationToken cancellationToken);
    }
}
