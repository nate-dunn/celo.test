using System;
using System.Collections.Generic;
using System.Text;
using Celo.Test.Data.Infrastructure;
using Celo.Test.Data.Models;

namespace Celo.Test.Data.Repositories
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(UserContext context): base(context)
        {
        }
    }
}
