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
    public class GameService : IGameService
    {
         private readonly ILogger<GameService> m_logger;
        private readonly IGameRepository m_repository;

        public GameService(ILogger<GameService> logger, IGameRepository repository)
        {
            m_logger = logger;
            m_repository = repository;
        }

        public async Task AddAsync(Game game)
        {
            try
            {
                await m_repository.CreateAsync(game);
            }
            catch (ValidationException e)
            {
                m_logger.LogWarning(e, "A validation failed");
                throw;
            }
            catch (Exception e) when (e.GetType() != typeof(ValidationException))
            {
                m_logger.LogCritical(e, $"Unexpected Exception while trying to create a Game with the properties : {JsonConvert.SerializeObject(game, Formatting.Indented)}");
                throw;
            }
        }

        public async Task UpdateAsync(Game game)
        {
            try
            {
                await m_repository.UpdateAsync(new List<Game> {game});
            }
            catch (ValidationException e)
            {
                m_logger.LogWarning(e, "A validation failed");
                throw;
            }
            catch (Exception e) when (e.GetType() != typeof(ValidationException))
            {
                m_logger.LogCritical(e, $"Unexpected Exception while trying to update a Game with the properties : {JsonConvert.SerializeObject(game, Formatting.Indented)}");
                throw;
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            try
            {
                var game = await SearchAsync(new Pagination(), new SimpleFilter<Game>
                {
                    SearchTerm = id.ToString()
                });
                await m_repository.DeleteAsync(game.Item2.First());
            }
            catch (ValidationException e)
            {
                m_logger.LogWarning(e, "A validation failed");
                throw;
            }
            catch (Exception e) when (e.GetType() != typeof(ValidationException))
            {
                m_logger.LogCritical(e, $"Unexpected Exception while trying to delete a Game for id : {id}");
                throw;
            }
        }

        public async Task<Tuple<int, List<Game>>> SearchAsync(Pagination pagination, IFilter<Game> filter)
        {
            try
            {
                return await m_repository.SearchAsync(pagination, filter);
            }
            catch (Exception e)
            {
                m_logger.LogCritical(e, "Unexpected Exception while trying to search for Games");
                throw;
            }
        }
    }
}