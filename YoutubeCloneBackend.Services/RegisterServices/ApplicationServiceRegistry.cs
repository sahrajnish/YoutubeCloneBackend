using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoutubeCloneBackend.Persistence.Setting;
using YoutubeCloneBackend.Persistence.User;
using YoutubeCloneBackend.Services.User;

namespace YoutubeCloneBackend.Services.RegisterServices
{
    public static class ApplicationServiceRegistry
    {
        public static void RegisterDIServices(this IServiceCollection services)
        {
            services.AddScoped<IUsers, Users>();
            services.AddScoped<ISetting, Setting>();
            services.AddScoped<IUserService, UserService>();
        }
    }
}
