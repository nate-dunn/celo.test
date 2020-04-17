using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Celo.Test.Data.Infrastructure
{
    /// <summary>
    /// Provides an interface through which standard data operations on an entity collection can be performed.
    /// </summary>
    /// <typeparam name="T">The entity type.</typeparam>
    public interface IRepository<T> where T : class, ITrackableEntity
    {
        /// <summary>
        /// Add an entity.
        /// </summary>
        EntityEntry<T> Add(T entity);

        /// <summary>
        /// Add a range of entities.
        /// </summary>
        Task AddRangeAsync(IEnumerable<T> entities);

        /// <summary>
        /// Delete entities.
        /// </summary>
        EntityEntry<T> Delete(T entity);

        /// <summary>
        /// Edit an entity.
        /// </summary>
        /// <param name="entity"></param>
        void Edit(T entity);

        /// <summary>
        /// Detach an entity.
        /// </summary>
        void Detach(T entity);

        /// <summary>
        /// Query the collection.
        /// </summary>
        IQueryable<T> Get();

        /// <summary>
        /// Save the changes in the collection.
        /// </summary>
        void Save();

        /// <summary>
        /// Save the changes in the collection.
        /// </summary>
        Task<int> SaveAsync();
    }
}
