using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Celo.Test.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Celo.Test.Services.Helpers
{
    public static class PageExtensions
    {
        /// <summary>
        /// Gets a page with result query for delayed execution.
        /// </summary>
        /// <param name="query">
        /// This should be preferably be a query and not a materialized set so that the pagination
        /// happens at the store level.
        /// </param>
        /// <param name="filter">The page options.</param>
        public static PageResult<T> GetPage<T>(this IQueryable<T> query, PageFilter filter)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }
            var pageResult = new PageResult<T>
            {
                Page = filter.ActualPage,
                PageSize = filter.ActualPageSize,
                Results = query
            };

            if (filter?.ActualPage > 0)
            {
                // Get the required page
                pageResult.Results = query
                    .Skip(filter.ActualOffset)
                    .Take(filter.ActualPageSize);
            }
            else if (filter != null)
            {
                // Invalid negative page number, return 0 results
                pageResult.Results = query.Take(0);
            }
            return pageResult;
        }

        /// <summary>
        /// Materialize a page result.
        /// </summary>
        public static async Task<PageResult<T>> MaterializeAsync<T>(this PageResult<T> pageResult)
        {
            pageResult.Results = await pageResult.Results.AsQueryable().ToListAsync();
            return pageResult;
        }
    }
}
