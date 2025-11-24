using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoutubeCloneBackend.Core.Mailjet;
using YoutubeCloneBackend.Persistence.SendOtps;
using YoutubeCloneBackend.Persistence.Setting;
using YoutubeCloneBackend.Services.SmsEmailService.ConsumeMailEvents;
using YoutubeCloneBackend.Services.SmsEmailService.MailingService.Mails;
using YoutubeCloneBackend.Services.SmsEmailService.PublishMailEvents;
using YoutubeCloneBackend.Services.SmsEmailService.RegisterOtp;

namespace YoutubeCloneBackend.Services.RegisterServices
{
    public static class SmsEmailServiceRegistry
    {
        public static void RegisterSmsEmailDIServices(this IServiceCollection services)
        {
            services.AddSingleton<IConsumeMailEvent, ConsumeMailEvent>();
            services.AddSingleton<ISetting, Setting>();
            services.AddSingleton<ISendOtp, SendOtp>();
            services.AddSingleton<IMail, Mail>();
            services.AddScoped<IPublishRegisterOtpMailEvent, PublishRegisterOtpMailEvent>();
            services.AddScoped<IGenerateRegisterOtp, GenerateRegisterOtp>();
        }
    }
}
