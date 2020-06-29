using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Exam.Web.Core.Helpers.Commons;
using Exam.Web.Core.Helpers.Commons.Filters.Implementations;
using Exam.Web.Core.Helpers.Commons.Filters.Interfaces;
using Exam.Web.Core.Models.Entities;
using Exam.Web.Core.Repository.Interfaces;
using Exam.Web.Core.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Exam.Web.Core.Services.Implementations
{
    public class CategoryService : ICategoryService
    {
         private readonly ILogger<CategoryService> m_logger;
        private readonly ICategoryRepository m_repository;

        public CategoryService(ILogger<CategoryService> logger, ICategoryRepository repository)
        {
            m_logger = logger;
            m_repository = repository;
        }

        public async Task AddAsync(Category Category)
        {
            try
            {
                await m_repository.CreateAsync(Category);
            }
            catch (ValidationException e)
            {
                m_logger.LogWarning(e, "A validation failed");
                throw;
            }
            catch (Exception e) when (e.GetType() != typeof(ValidationException))
            {
                m_logger.LogCritical(e, $"Unexpected Exception while trying to create a Category with the properties : {JsonConvert.SerializeObject(Category, Formatting.Indented)}");
                throw;
            }
        }

        public async Task UpdateAsync(Category Category)
        {
            try
            {
                await m_repository.UpdateAsync(new List<Category> {Category});
            }
            catch (ValidationException e)
            {
                m_logger.LogWarning(e, "A validation failed");
                throw;
            }
            catch (Exception e) when (e.GetType() != typeof(ValidationException))
            {
                m_logger.LogCritical(e, $"Unexpected Exception while trying to update a Category with the properties : {JsonConvert.SerializeObject(Category, Formatting.Indented)}");
                throw;
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            try
            {
                var Category = await SearchAsync(new Pagination(), new SimpleFilter<Category>
                {
                    SearchTerm = id.ToString()
                });
                await m_repository.DeleteAsync(Category.Item2.First());
            }
            catch (ValidationException e)
            {
                m_logger.LogWarning(e, "A validation failed");
                throw;
            }
            catch (Exception e) when (e.GetType() != typeof(ValidationException))
            {
                m_logger.LogCritical(e, $"Unexpected Exception while trying to delete a Category for id : {id}");
                throw;
            }
        }

        public async Task<Tuple<int, List<Category>>> SearchAsync(Pagination pagination, IFilter<Category> filter)
        {
            try
            {
                return await m_repository.SearchAsync(pagination, filter);
            }
            catch (Exception e)
            {
                m_logger.LogCritical(e, "Unexpected Exception while trying to search for Categorys");
                throw;
            }
        }
    }
}