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
    public class ResponseRepository : IResponseRepository
    {
        private readonly DataContext m_dataContext;

        public ResponseRepository(DataContext dataContext)
        {
            m_dataContext = dataContext;
        }

        public async Task CreateAsync(Response entity)
        {
            await m_dataContext.Responses.AddAsync(entity);
            await m_dataContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(List<Response> entities)
        {
            m_dataContext.Responses.UpdateRange(entities);
            await m_dataContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Response entity)
        {
            m_dataContext.Responses.Remove(entity);
            await m_dataContext.SaveChangesAsync();
        }

        public async Task<Tuple<int, List<Response>>> SearchAsync(Pagination pagination, IFilter<Response> filtering)
        {
            return await filtering
                .Filter(m_dataContext.Responses.AsQueryable())
                .WithPaginationAsync(pagination);
        }
    }
}