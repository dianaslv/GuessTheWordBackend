using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Exam.Web.Core.Helpers.Commons;
using Exam.Web.Core.Helpers.Commons.Filters.Interfaces;
using Exam.Web.Core.Models.Entities;

namespace Exam.Web.Core.Services.Interfaces
{
    public interface IStudentToGameService
    {
        public Task AddAsync(StudentToGame StudentToGame);
        public Task UpdateAsync(StudentToGame StudentToGame);
        public Task DeleteAsync(Guid id);
        public Task<Tuple<int, List<StudentToGame>>> SearchAsync(Pagination pagination, IFilter<StudentToGame> filter);
    }
}