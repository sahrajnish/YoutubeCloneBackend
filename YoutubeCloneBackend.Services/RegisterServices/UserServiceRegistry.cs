using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoutubeCloneBackend.Persistence.SendOtps;
using YoutubeCloneBackend.Persistence.Setting;
using YoutubeCloneBackend.Persistence.User;
using YoutubeCloneBackend.Persistence.ValidateOtps;
using YoutubeCloneBackend.Services.UserServices.OtpValidation;
using YoutubeCloneBackend.Services.UserServices.PublishMailEvents;
using YoutubeCloneBackend.Services.UserServices.User;
using YoutubeCloneBackend.Services.UserServices.UserToSmsEmail;

namespace YoutubeCloneBackend.Services.RegisterServices
{
    public static class UserServiceRegistry
    {
        public static void RegisterUserDIServices(this IServiceCollection services)
        {
            services.AddScoped<IUsers, Users>();
            services.AddScoped<ISetting, Setting>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ISmsEmailClient, SmsEmailClient>();
            services.AddScoped<IValidateOtp, ValidateOtp>();
            services.AddScoped<IOtpValidations, OtpValidations>();
            services.AddScoped<IPublishMailEvent, PublishMailEvent>();
            services.AddScoped<ISendOtp, SendOtp>();
        }
    }
}
