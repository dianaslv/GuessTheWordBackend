using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Exam.Web.Core.Helpers.Commons;
using Exam.Web.Core.Helpers.Commons.Filters.Interfaces;
using Exam.Web.Core.Models.Entities;

namespace Exam.Web.Core.Services.Interfaces
{
    public interface IResponseService
    {
        public Task AddAsync(Response Response);
        public Task UpdateAsync(Response Response);
        public Task DeleteAsync(Guid id);
        public Task<Tuple<int, List<Response>>> SearchAsync(Pagination pagination, IFilter<Response> filter);
    }
}