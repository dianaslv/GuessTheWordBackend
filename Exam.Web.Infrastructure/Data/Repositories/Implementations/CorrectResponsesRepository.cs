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
    public class CorrectResponsesRepository : ICorrectResponsesRepository
    {
        private readonly DataContext m_dataContext;

        public CorrectResponsesRepository(DataContext dataContext)
        {
            m_dataContext = dataContext;
        }

        public async Task CreateAsync(CorrectResponses entity)
        {
            await m_dataContext.CorrectResponses.AddAsync(entity);
            await m_dataContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(List<CorrectResponses> entities)
        {
            m_dataContext.CorrectResponses.UpdateRange(entities);
            await m_dataContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(CorrectResponses entity)
        {
            m_dataContext.CorrectResponses.Remove(entity);
            await m_dataContext.SaveChangesAsync();
        }

        public async Task<Tuple<int, List<CorrectResponses>>> SearchAsync(Pagination pagination, IFilter<CorrectResponses> filtering)
        {
            return await filtering
                .Filter(m_dataContext.CorrectResponses.AsQueryable())
                .WithPaginationAsync(pagination);
        }
    }
}