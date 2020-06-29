using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Exam.Web.Core.Helpers.Commons;
using Exam.Web.Core.Helpers.Commons.Filters.Interfaces;
using Exam.Web.Core.Models.Entities;

namespace Exam.Web.Core.Services.Interfaces
{
    public interface IRoundService
    {
        public Task AddAsync(Round Round);
        public Task UpdateAsync(Round Round);
        public Task DeleteAsync(Guid id);
        public Task<Tuple<int, List<Round>>> SearchAsync(Pagination pagination, IFilter<Round> filter);
    }
}