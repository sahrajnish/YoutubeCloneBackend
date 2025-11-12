using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoutubeCloneBackend.Services.ConsumeEvents.ConsumeRegistrationEvent
{
    public interface IConsumeUserRegistrationEvent
    {
        public Task ConsumeEvents(CancellationToken cancellationToken);
    }
}
