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
    public class RoundRepository : IRoundRepository
    {
        private readonly DataContext m_dataContext;

        public RoundRepository(DataContext dataContext)
        {
            m_dataContext = dataContext;
        }

        public async Task CreateAsync(Round entity)
        {
            await m_dataContext.Rounds.AddAsync(entity);
            await m_dataContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(List<Round> entities)
        {
            m_dataContext.Rounds.UpdateRange(entities);
            await m_dataContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Round entity)
        {
            m_dataContext.Rounds.Remove(entity);
            await m_dataContext.SaveChangesAsync();
        }

        public async Task<Tuple<int, List<Round>>> SearchAsync(Pagination pagination, IFilter<Round> filtering)
        {
            return await filtering
                .Filter(m_dataContext.Rounds.AsQueryable())
                .WithPaginationAsync(pagination);
        }
    }
}