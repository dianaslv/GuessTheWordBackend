using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Exam.Web.Core.Helpers.Commons;
using Exam.Web.Core.Helpers.Commons.Filters.Interfaces;
using Exam.Web.Core.Models.Entities;

namespace Exam.Web.Core.Services.Interfaces
{
    public interface IGameService
    {
        public Task AddAsync(Game Game);
        public Task UpdateAsync(Game Game);
        public Task DeleteAsync(Guid id);
        public Task<Tuple<int, List<Game>>> SearchAsync(Pagination pagination, IFilter<Game> filter);
    }
}