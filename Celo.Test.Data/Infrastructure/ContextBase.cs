using System;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Celo.Test.Data.Infrastructure
{
    public abstract class ContextBase<TContext> : DbContext where TContext : DbContext
    {
        protected ILogger Logger { get; private set; }

        public ContextBase() : base() { }

        public ContextBase(DbContextOptions<TContext> options, ILogger logger) : base(options)
        {
            logger?.LogDebug("Instantiating ContextBase");
            Logger = logger;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                //// Dont pluralize table names
                //entity.SetTableName(entity.DisplayName());

                Logger?.LogDebug("OnModelCreating...");
                if (typeof(ITrackableEntity).IsAssignableFrom(entity.ClrType))
                {
                    modelBuilder.SetSoftDeleteFilter(entity.ClrType);
                }
            }

            // Runs all configurations found in this library
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(TContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            ModifyNewAndChangedEntities();
            return base.SaveChanges();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            ModifyNewAndChangedEntities();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken())
        {
            ModifyNewAndChangedEntities();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        /// <summary>
        /// Set the timestamps for any created or modified entities.
        /// </summary>
        private void ModifyNewAndChangedEntities()
        {
            // Find all entities that adhere to our trackable infrastructure
            var modifiedEntries = ChangeTracker
                .Entries()
                .Where(x => x.Entity is ITrackableEntity && (x.State == EntityState.Added || x.State == EntityState.Modified));

            foreach (var entry in modifiedEntries)
            {
                if (entry.Entity is ITrackableEntity trackableEntity)
                {
                    var now = DateTimeOffset.Now;
                    if (entry.State == EntityState.Added)
                    {
                        // Only overwrite the date if not explicitly set
                        if (trackableEntity.CreatedTime == default)
                            trackableEntity.CreatedTime = now;
                    }
                    else
                    {
                        Entry(trackableEntity).Property(x => x.CreatedTime).IsModified = false;
                    }

                    trackableEntity.ModifiedTime = now;
                }
            }
        }
    }
}
