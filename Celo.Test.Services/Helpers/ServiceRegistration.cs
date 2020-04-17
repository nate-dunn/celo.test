using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace Celo.Test.Services.Helpers
{
    public static class ServiceRegistration
    {
        /// <summary>
        /// Register services.
        /// </summary>
        public static IServiceCollection AddServiceTierServices(this IServiceCollection services)
        {
            services.AddTransient(typeof(IUserService), typeof(UserService));
            services.AddTransient(typeof(IImageService), typeof(ImageService));
            return services;
        }
    }
}
