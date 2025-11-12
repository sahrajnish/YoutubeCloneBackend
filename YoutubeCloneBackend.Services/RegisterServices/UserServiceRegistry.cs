using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoutubeCloneBackend.Persistence.Setting;
using YoutubeCloneBackend.Persistence.User;
using YoutubeCloneBackend.Services.PublishEvents;
using YoutubeCloneBackend.Services.User;

namespace YoutubeCloneBackend.Services.RegisterServices
{
    public static class UserServiceRegistry
    {
        public static void RegisterUserDIServices(this IServiceCollection services)
        {
            services.AddScoped<IUsers, Users>();
            services.AddScoped<ISetting, Setting>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IPublishRegisteredUserEvent, PublishRegisteredUserEvent>();
        }
    }
}
