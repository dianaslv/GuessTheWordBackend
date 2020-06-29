using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Exam.Web.Core.Helpers.Commons;
using Exam.Web.Core.Helpers.Commons.Filters.Interfaces;
using Exam.Web.Core.Models.Entities;

namespace Exam.Web.Core.Services.Interfaces
{
    public interface ICorrectResponsesService
    {
        public Task AddAsync(CorrectResponses CorrectResponses);
        public Task UpdateAsync(CorrectResponses CorrectResponses);
        public Task DeleteAsync(Guid id);
        public Task<Tuple<int, List<CorrectResponses>>> SearchAsync(Pagination pagination, IFilter<CorrectResponses> filter);
    }
}