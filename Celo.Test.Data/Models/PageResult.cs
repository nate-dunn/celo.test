using System.Collections.Generic;
using System.Linq;

namespace Celo.Test.Data.Models
{
    /// <summary>
    /// The result of a page fetch.
    /// </summary>
    /// <typeparam name="T">The type of the items in the page.</typeparam>
    public class PageResult<T>
    {
        public int? Page { get; set; }
        public int? PageSize { get; set; }
        public IEnumerable<T> Results { get; set; }
    }
}
