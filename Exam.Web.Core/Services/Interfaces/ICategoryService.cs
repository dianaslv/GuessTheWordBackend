using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Exam.Web.Core.Helpers.Commons;
using Exam.Web.Core.Helpers.Commons.Filters.Interfaces;
using Exam.Web.Core.Models.Entities;

namespace Exam.Web.Core.Services.Interfaces
{
    public interface ICategoryService
    {
        public Task AddAsync(Category Category);
        public Task UpdateAsync(Category Category);
        public Task DeleteAsync(Guid id);
        public Task<Tuple<int, List<Category>>> SearchAsync(Pagination pagination, IFilter<Category> filter);
    }
}