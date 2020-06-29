using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Exam.Web.Core.Extensions;
using Exam.Web.Core.Helpers.Commons;
using Exam.Web.Core.Helpers.Commons.Filters.Interfaces;
using Exam.Web.Core.Models.Entities;
using Exam.Web.Core.Repository.Interfaces;
using Exam.Web.Infrastructure.Data.Context;
using Exam.Web.Infrastructure.IOC;

namespace Exam.Web.Infrastructure.Data.Repositories.Implementations
{
    [RegistrationKind(Type = RegistrationType.Scoped)]
    public class StudentRepository : IStudentRepository
    {
        private readonly DataContext m_dataContext;

        public StudentRepository(DataContext dataContext)
        {
            m_dataContext = dataContext;
        }

        public async Task CreateAsync(Student entity)
        {
            await m_dataContext.Students.AddAsync(entity);
            await m_dataContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(List<Student> entities)
        {
            m_dataContext.Students.UpdateRange(entities);
            await m_dataContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Student entity)
        {
            m_dataContext.Students.Remove(entity);
            await m_dataContext.SaveChangesAsync();
        }

        public async Task<Tuple<int, List<Student>>> SearchAsync(Pagination pagination, IFilter<Student> filtering)
        {
            return await filtering
                .Filter(m_dataContext.Students.AsQueryable())
                .WithPaginationAsync(pagination);
        }
    }
}