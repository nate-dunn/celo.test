using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Celo.Test.Data.Infrastructure;
using Celo.Test.Data.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Celo.Test.Data.Repositories
{
    public class ImageRepoository : RepositoryBase<Image>, IImageRepository
    {
        public ImageRepoository(UserContext context) : base(context)
        {
        }
    }
}
