using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Celo.Test.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Celo.Test.Data
{
    public static class ServiceRegistration
    {
        /// <summary>
        /// Register repositories and contexts.
        /// </summary>
        public static IServiceCollection AddDataTierServices(this IServiceCollection services, IConfiguration configuration)
        {
            var localDbConnectionString = configuration.GetConnectionString("UserContext");
            services.AddDbContext<UserContext>(options => options.UseSqlServer(localDbConnectionString));
            services.AddTransient(typeof(IUserRepository), typeof(UserRepository));
            services.AddTransient(typeof(IImageRepository), typeof(ImageRepoository));
            return services;
        }
    }
}
