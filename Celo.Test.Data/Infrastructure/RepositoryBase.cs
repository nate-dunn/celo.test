using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Celo.Test.Data.Infrastructure;

namespace Celo.Test.Data.Infrastructure
{
    /// <summary>
    /// Provides fundamental data operations on an entity collection.
    /// This implementation which talks to a DB context, can be swapped out for a mock version.
    /// </summary>
    /// <typeparam name="T">The entity type.</typeparam>
    public class RepositoryBase<T> : IRepository<T> where T : class, ITrackableEntity
    {
        private readonly DbContext _context;

        protected RepositoryBase(DbContext context)
        {
            _context = context;
        }

        public IQueryable<T> Get()
        {
            return _context.Set<T>();
        }

        public virtual EntityEntry<T> Add(T entity)
        {
            return _context.Set<T>().Add(entity);
        }

        public virtual async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _context.Set<T>().AddRangeAsync(entities);
        }

        public virtual EntityEntry<T> Delete(T entity)
        {
            return _context.Set<T>().Remove(entity);
        }

        public virtual void Edit(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }

        public virtual void Detach(T entity)
        {
            _context.Entry(entity).State = EntityState.Detached;
        }


        public virtual void Save()
        {
            _context.SaveChanges();
        }

        public virtual Task<int> SaveAsync()
        {
            return _context.SaveChangesAsync();
        }
    }
}
