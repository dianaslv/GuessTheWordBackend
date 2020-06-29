using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Exam.Web.Core.Helpers.Commons;
using Exam.Web.Core.Helpers.Commons.Filters.Interfaces;

namespace Exam.Web.Core.Helpers.Interfaces.Commons
{
    public interface IRepository<TEntity> where TEntity : IIdentifier
    {
        Task CreateAsync(TEntity entity);
        Task UpdateAsync(List<TEntity> entities);
        Task DeleteAsync(TEntity entity);
        Task<Tuple<int, List<TEntity>>> SearchAsync(Pagination pagination, IFilter<TEntity> filtering);
    }
}