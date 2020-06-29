using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Exam.Web.Core.Helpers.Commons;
using Exam.Web.Core.Helpers.Commons.Filters.Interfaces;
using Exam.Web.Core.Models.Entities;

namespace Exam.Web.Core.Services.Interfaces
{
    public interface IStudentService
    {
        public Task AddAsync(Student Student);
        public Task UpdateAsync(Student Student);
        public Task DeleteAsync(Guid id);
        public Task<Tuple<int, List<Student>>> SearchAsync(Pagination pagination, IFilter<Student> filter);
    }
}