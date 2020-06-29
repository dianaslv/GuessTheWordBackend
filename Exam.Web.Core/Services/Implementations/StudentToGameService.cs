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
    public class StudentToGameService : IStudentToGameService
    {
         private readonly ILogger<StudentToGameService> m_logger;
        private readonly IStudentToGameRepository m_repository;

        public StudentToGameService(ILogger<StudentToGameService> logger, IStudentToGameRepository repository)
        {
            m_logger = logger;
            m_repository = repository;
        }

        public async Task AddAsync(StudentToGame StudentToGame)
        {
            try
            {
                await m_repository.CreateAsync(StudentToGame);
            }
            catch (ValidationException e)
            {
                m_logger.LogWarning(e, "A validation failed");
                throw;
            }
            catch (Exception e) when (e.GetType() != typeof(ValidationException))
            {
                m_logger.LogCritical(e, $"Unexpected Exception while trying to create a StudentToGame with the properties : {JsonConvert.SerializeObject(StudentToGame, Formatting.Indented)}");
                throw;
            }
        }

        public async Task UpdateAsync(StudentToGame StudentToGame)
        {
            try
            {
                await m_repository.UpdateAsync(new List<StudentToGame> {StudentToGame});
            }
            catch (ValidationException e)
            {
                m_logger.LogWarning(e, "A validation failed");
                throw;
            }
            catch (Exception e) when (e.GetType() != typeof(ValidationException))
            {
                m_logger.LogCritical(e, $"Unexpected Exception while trying to update a StudentToGame with the properties : {JsonConvert.SerializeObject(StudentToGame, Formatting.Indented)}");
                throw;
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            try
            {
                var StudentToGame = await SearchAsync(new Pagination(), new SimpleFilter<StudentToGame>
                {
                    SearchTerm = id.ToString()
                });
                await m_repository.DeleteAsync(StudentToGame.Item2.First());
            }
            catch (ValidationException e)
            {
                m_logger.LogWarning(e, "A validation failed");
                throw;
            }
            catch (Exception e) when (e.GetType() != typeof(ValidationException))
            {
                m_logger.LogCritical(e, $"Unexpected Exception while trying to delete a StudentToGame for id : {id}");
                throw;
            }
        }

        public async Task<Tuple<int, List<StudentToGame>>> SearchAsync(Pagination pagination, IFilter<StudentToGame> filter)
        {
            try
            {
                return await m_repository.SearchAsync(pagination, filter);
            }
            catch (Exception e)
            {
                m_logger.LogCritical(e, "Unexpected Exception while trying to search for StudentToGames");
                throw;
            }
        }
    }
}