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
    public class ResponseService : IResponseService
    {
         private readonly ILogger<ResponseService> m_logger;
        private readonly IResponseRepository m_repository;

        public ResponseService(ILogger<ResponseService> logger, IResponseRepository repository)
        {
            m_logger = logger;
            m_repository = repository;
        }

        public async Task AddAsync(Response Response)
        {
            try
            {
                await m_repository.CreateAsync(Response);
            }
            catch (ValidationException e)
            {
                m_logger.LogWarning(e, "A validation failed");
                throw;
            }
            catch (Exception e) when (e.GetType() != typeof(ValidationException))
            {
                m_logger.LogCritical(e, $"Unexpected Exception while trying to create a Response with the properties : {JsonConvert.SerializeObject(Response, Formatting.Indented)}");
                throw;
            }
        }

        public async Task UpdateAsync(Response Response)
        {
            try
            {
                await m_repository.UpdateAsync(new List<Response> {Response});
            }
            catch (ValidationException e)
            {
                m_logger.LogWarning(e, "A validation failed");
                throw;
            }
            catch (Exception e) when (e.GetType() != typeof(ValidationException))
            {
                m_logger.LogCritical(e, $"Unexpected Exception while trying to update a Response with the properties : {JsonConvert.SerializeObject(Response, Formatting.Indented)}");
                throw;
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            try
            {
                var Response = await SearchAsync(new Pagination(), new SimpleFilter<Response>
                {
                    SearchTerm = id.ToString()
                });
                await m_repository.DeleteAsync(Response.Item2.First());
            }
            catch (ValidationException e)
            {
                m_logger.LogWarning(e, "A validation failed");
                throw;
            }
            catch (Exception e) when (e.GetType() != typeof(ValidationException))
            {
                m_logger.LogCritical(e, $"Unexpected Exception while trying to delete a Response for id : {id}");
                throw;
            }
        }

        public async Task<Tuple<int, List<Response>>> SearchAsync(Pagination pagination, IFilter<Response> filter)
        {
            try
            {
                return await m_repository.SearchAsync(pagination, filter);
            }
            catch (Exception e)
            {
                m_logger.LogCritical(e, "Unexpected Exception while trying to search for Responses");
                throw;
            }
        }
    }
}