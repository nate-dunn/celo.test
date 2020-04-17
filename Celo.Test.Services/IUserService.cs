using System;
using System.Linq;
using System.Threading.Tasks;
using Celo.Test.Data.Models;

namespace Celo.Test.Services
{
    public interface IUserService
    {
        /// <summary>
        /// Add a user.
        /// </summary>
        Task<Guid> AddUserAsync(User user);

        /// <summary>
        /// Update an existing user.
        /// </summary>
        Task UpdateUserAsync(User user);

        /// <summary>
        /// Get users.
        /// </summary>
        Task<PageResult<User>> GetUsersAsync(UserFilter filter);

        /// <summary>
        /// Delete Users.
        /// </summary>
        Task DeleteUserAsync(Guid id);
    }
}
