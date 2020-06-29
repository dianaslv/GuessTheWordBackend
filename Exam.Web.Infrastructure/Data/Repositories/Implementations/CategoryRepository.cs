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
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DataContext m_dataContext;

        public CategoryRepository(DataContext dataContext)
        {
            m_dataContext = dataContext;
        }

        public async Task CreateAsync(Category entity)
        {
            await m_dataContext.Categories.AddAsync(entity);
            await m_dataContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(List<Category> entities)
        {
            m_dataContext.Categories.UpdateRange(entities);
            await m_dataContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Category entity)
        {
            m_dataContext.Categories.Remove(entity);
            await m_dataContext.SaveChangesAsync();
        }

        public async Task<Tuple<int, List<Category>>> SearchAsync(Pagination pagination, IFilter<Category> filtering)
        {
            return await filtering
                .Filter(m_dataContext.Categories.AsQueryable())
                .WithPaginationAsync(pagination);
        }
    }
}