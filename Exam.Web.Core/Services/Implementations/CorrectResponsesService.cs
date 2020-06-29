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
    public class CorrectResponsesService : ICorrectResponsesService
    {
         private readonly ILogger<CorrectResponsesService> m_logger;
        private readonly ICorrectResponsesRepository m_repository;

        public CorrectResponsesService(ILogger<CorrectResponsesService> logger, ICorrectResponsesRepository repository)
        {
            m_logger = logger;
            m_repository = repository;
        }

        public async Task AddAsync(CorrectResponses CorrectResponses)
        {
            try
            {
                await m_repository.CreateAsync(CorrectResponses);
            }
            catch (ValidationException e)
            {
                m_logger.LogWarning(e, "A validation failed");
                throw;
            }
            catch (Exception e) when (e.GetType() != typeof(ValidationException))
            {
                m_logger.LogCritical(e, $"Unexpected Exception while trying to create a CorrectResponses with the properties : {JsonConvert.SerializeObject(CorrectResponses, Formatting.Indented)}");
                throw;
            }
        }

        public async Task UpdateAsync(CorrectResponses CorrectResponses)
        {
            try
            {
                await m_repository.UpdateAsync(new List<CorrectResponses> {CorrectResponses});
            }
            catch (ValidationException e)
            {
                m_logger.LogWarning(e, "A validation failed");
                throw;
            }
            catch (Exception e) when (e.GetType() != typeof(ValidationException))
            {
                m_logger.LogCritical(e, $"Unexpected Exception while trying to update a CorrectResponses with the properties : {JsonConvert.SerializeObject(CorrectResponses, Formatting.Indented)}");
                throw;
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            try
            {
                var CorrectResponses = await SearchAsync(new Pagination(), new SimpleFilter<CorrectResponses>
                {
                    SearchTerm = id.ToString()
                });
                await m_repository.DeleteAsync(CorrectResponses.Item2.First());
            }
            catch (ValidationException e)
            {
                m_logger.LogWarning(e, "A validation failed");
                throw;
            }
            catch (Exception e) when (e.GetType() != typeof(ValidationException))
            {
                m_logger.LogCritical(e, $"Unexpected Exception while trying to delete a CorrectResponses for id : {id}");
                throw;
            }
        }

        public async Task<Tuple<int, List<CorrectResponses>>> SearchAsync(Pagination pagination, IFilter<CorrectResponses> filter)
        {
            try
            {
                return await m_repository.SearchAsync(pagination, filter);
            }
            catch (Exception e)
            {
                m_logger.LogCritical(e, "Unexpected Exception while trying to search for CorrectResponsess");
                throw;
            }
        }
    }
}