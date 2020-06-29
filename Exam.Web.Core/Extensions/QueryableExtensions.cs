using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Exam.Web.Core.Helpers.Commons;
using Microsoft.EntityFrameworkCore;

namespace Exam.Web.Core.Extensions
{
    public static class QueryableExtensions
    {
        public static async Task<Tuple<int, List<TSource>>> WithPaginationAsync<TSource>(this IQueryable<TSource> source, Pagination paging)
        {
            return new Tuple<int, List<TSource>>(
                await source.CountAsync(),
                await source.Skip(paging.Offset).Take(paging.Take)
                    .AsQueryable()
                    .ToListAsync());
        }
    }
}