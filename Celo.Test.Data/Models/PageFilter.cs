using System;
using System.Collections.Generic;
using System.Text;

namespace Celo.Test.Data.Models
{
    public class PageFilter
    {
        /// <summary>
        /// Default page size.  Could be set in configuration somewhere.
        /// </summary>
        public const int DefaultPageSize = 100;

        /// <summary>
        /// Filter by page number if specified.
        /// </summary>
        public int? Page { get; set; }

        /// <summary>
        /// Override default page size if specified.
        /// </summary>
        public int? PageSize { get; set; }

        public int ActualPageSize => PageSize ?? DefaultPageSize;
        public int ActualPage => Page ?? 1;
        public int ActualOffset => ActualPageSize * (ActualPage - 1);

    }
}
