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
    public class StudentToGameRepository : IStudentToGameRepository
    {
        private readonly DataContext m_dataContext;

        public StudentToGameRepository(DataContext dataContext)
        {
            m_dataContext = dataContext;
        }

        public async Task CreateAsync(StudentToGame entity)
        {
            await m_dataContext.StudentToGames.AddAsync(entity);
            await m_dataContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(List<StudentToGame> entities)
        {
            m_dataContext.StudentToGames.UpdateRange(entities);
            await m_dataContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(StudentToGame entity)
        {
            m_dataContext.StudentToGames.Remove(entity);
            await m_dataContext.SaveChangesAsync();
        }

        public async Task<Tuple<int, List<StudentToGame>>> SearchAsync(Pagination pagination, IFilter<StudentToGame> filtering)
        {
            return await filtering
                .Filter(m_dataContext.StudentToGames.AsQueryable())
                .WithPaginationAsync(pagination);
        }
    }
}