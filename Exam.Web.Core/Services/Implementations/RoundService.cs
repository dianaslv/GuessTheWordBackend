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
    public class RoundService : IRoundService

    {
    private readonly ILogger<RoundService> m_logger;
    private readonly IRoundRepository m_repository;

    public RoundService(ILogger<RoundService> logger, IRoundRepository repository)
    {
        m_logger = logger;
        m_repository = repository;
    }

    public async Task AddAsync(Round Round)
    {
        try
        {
            await m_repository.CreateAsync(Round);
        }
        catch (ValidationException e)
        {
            m_logger.LogWarning(e, "A validation failed");
            throw;
        }
        catch (Exception e) when (e.GetType() != typeof(ValidationException))
        {
            m_logger.LogCritical(e, $"Unexpected Exception while trying to create a Round with the properties : {JsonConvert.SerializeObject(Round, Formatting.Indented)}");
            throw;
        }
    }

    public async Task UpdateAsync(Round Round)
    {
        try
        {
            await m_repository.UpdateAsync(new List<Round> {Round});
        }
        catch (ValidationException e)
        {
            m_logger.LogWarning(e, "A validation failed");
            throw;
        }
        catch (Exception e) when (e.GetType() != typeof(ValidationException))
        {
            m_logger.LogCritical(e, $"Unexpected Exception while trying to update a Round with the properties : {JsonConvert.SerializeObject(Round, Formatting.Indented)}");
            throw;
        }
    }

    public async Task DeleteAsync(Guid id)
    {
        try
        {
            var Round = await SearchAsync(new Pagination(), new SimpleFilter<Round>
            {
                SearchTerm = id.ToString()
            });
            await m_repository.DeleteAsync(Round.Item2.First());
        }
        catch (ValidationException e)
        {
            m_logger.LogWarning(e, "A validation failed");
            throw;
        }
        catch (Exception e) when (e.GetType() != typeof(ValidationException))
        {
            m_logger.LogCritical(e, $"Unexpected Exception while trying to delete a Round for id : {id}");
            throw;
        }
    }

    public async Task<Tuple<int, List<Round>>> SearchAsync(Pagination pagination, IFilter<Round> filter)
    {
        try
        {
            return await m_repository.SearchAsync(pagination, filter);
        }
        catch (Exception e)
        {
            m_logger.LogCritical(e, "Unexpected Exception while trying to search for Rounds");
            throw;
        }
    }
    }
}