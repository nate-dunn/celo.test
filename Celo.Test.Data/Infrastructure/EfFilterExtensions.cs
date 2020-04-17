using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Celo.Test.Data.Infrastructure
{
    public static class EfFilterExtensions
    {
        /// <summary>
        /// Find and store a reference to the SetSoftDeleteFilter generic method - done only once at app domain load time.
        /// </summary>
        static readonly MethodInfo SetSoftDeleteFilterMethod = typeof(EfFilterExtensions)
            .GetMethods(BindingFlags.Public | BindingFlags.Static)
            .Single(t => t.IsGenericMethod && t.Name == nameof(SetSoftDeleteFilter));

        /// <summary>
        /// Calls the SetSoftDeleteFilter generic method for the given entity type.
        /// </summary>
        public static void SetSoftDeleteFilter(this ModelBuilder modelBuilder, Type entityType)
        {
            SetSoftDeleteFilterMethod
                .MakeGenericMethod(entityType)
                .Invoke(null, new object[] { modelBuilder });
        }

        /// <summary>
        /// Sets up a filter for the given model so that only entities retrieved with a null DeletedTime attribute will be retrieved.
        /// </summary>
        public static void SetSoftDeleteFilter<TEntity>(this ModelBuilder modelBuilder)
            where TEntity : class, ITrackableEntity
        {
            // Filter the entities retrieved to only those with a null deleted time
            modelBuilder.Entity<TEntity>().HasQueryFilter(x => x.DeletedTime == null);
        }
    }
}
