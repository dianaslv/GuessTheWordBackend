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
    public class GameRepository : IGameRepository
    {
        private readonly DataContext m_dataContext;

        public GameRepository(DataContext dataContext)
        {
            m_dataContext = dataContext;
        }

        public async Task CreateAsync(Game entity)
        {
            await m_dataContext.Games.AddAsync(entity);
            await m_dataContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(List<Game> entities)
        {
            m_dataContext.Games.UpdateRange(entities);
            await m_dataContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Game entity)
        {
            m_dataContext.Games.Remove(entity);
            await m_dataContext.SaveChangesAsync();
        }

        public async Task<Tuple<int, List<Game>>> SearchAsync(Pagination pagination, IFilter<Game> filtering)
        {
            return await filtering
                .Filter(m_dataContext.Games.AsQueryable())
                .WithPaginationAsync(pagination);
        }
    }
}