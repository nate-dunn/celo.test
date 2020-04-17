using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Celo.Test.Data;
using Celo.Test.Data.Models;
using Celo.Test.Data.Repositories;
using Celo.Test.Services.Exceptions;
using Celo.Test.Services.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Celo.Test.Services
{
    public class UserService : IUserService
    {

        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Guid> AddUserAsync(User user)
        {
            user.Id = Guid.NewGuid();
            _userRepository.Add(user);

            await _userRepository.SaveAsync();
            return user.Id;
        }

        public async Task DeleteUserAsync(Guid id)
        {
            var existing = await _userRepository
                .Get()
                .Where(u => u.Id == id)
                .FirstOrDefaultAsync();
            if (existing == null)
            {
                throw new NotFoundException();
            }

            _userRepository.Delete(existing);
            await _userRepository.SaveAsync();
        }

        public async Task<PageResult<User>> GetUsersAsync(UserFilter filter)
        {
            var query = _userRepository.Get();
            if (filter?.Id != null)
            {
                // Filter by ID
                query = query.Where(u => u.Id == filter.Id);
            }
            if (!string.IsNullOrWhiteSpace(filter?.FirstNameEquals))
            {
                // Case insensitive match on first name
                query = query.Where(u => u.FirstName.Equals(filter.FirstNameEquals, StringComparison.InvariantCultureIgnoreCase));
            }
            if (!string.IsNullOrWhiteSpace(filter?.LastNameEquals))
            {
                // Case insensitive match on last name
                query = query.Where(u => u.LastName.Equals(filter.LastNameEquals, StringComparison.InvariantCultureIgnoreCase));
            }
            if (!string.IsNullOrWhiteSpace(filter?.NameContains))
            {
                // Combined name e.g. Nathan Dunn contains filter criteria
                query = query.Where(u => ((u.FirstName ?? "") + " " + (u.LastName ?? "")).ToLower().Trim().Contains(filter.NameContains.ToLower()));
            }

            var result = await query
                .OrderBy(u => u.FirstName)
                .ThenBy(u => u.LastName)
                .GetPage(filter)
                .MaterializeAsync();
            return result;
        }

        public async Task UpdateUserAsync(User user)
        {
            var existing = await _userRepository
                .Get()
                .Where(u => u.Id == user.Id)
                .FirstOrDefaultAsync();
            if (existing == null)
            {
                throw new NotFoundException();
            }

            // This cheats by copying all properties across to the data object.
            // In real life, I would use a model for the API that is separate from the storage entity,
            // and provide some automated mapping code between them.
            existing.CopyFrom(user);
            await _userRepository.SaveAsync();
        }
    }
}
