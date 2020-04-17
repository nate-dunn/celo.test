using System;
using System.Collections.Generic;
using System.Text;
using Celo.Test.Data.Infrastructure;
using Celo.Test.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

namespace Celo.Test.Data
{
    public class UserContext : ContextBase<UserContext>
    {
        public UserContext() : base() { }

        public UserContext(DbContextOptions<UserContext> options, ILogger<UserContext> logger) : base(options, logger) { }

        /// <summary>
        /// Get the users collection.
        /// </summary>
        public DbSet<User> Users { get; set; }

        /// <summary>
        /// Get the images collection.
        /// </summary>
        public DbSet<Image> Images { get; set; }
    }
}
