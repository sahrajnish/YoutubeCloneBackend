using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoutubeCloneBackend.Core.Mailjet;
using YoutubeCloneBackend.Services.ConsumeEvents.ConsumeRegistrationEvent;

namespace YoutubeCloneBackend.Services.RegisterServices
{
    public static class SmsEmailServiceRegistry
    {
        public static void RegisterSmsEmailDIServices(this IServiceCollection services)
        {
            services.AddSingleton<IConsumeUserRegistrationEvent, ConsumeUserRegistrationEvent>();
        }
    }
}
