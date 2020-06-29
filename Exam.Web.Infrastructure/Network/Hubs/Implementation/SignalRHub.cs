using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Exam.Web.Core.Enums;
using Exam.Web.Core.Helpers.Commons;
using Exam.Web.Core.Helpers.Commons.Filters.Implementations;
using Exam.Web.Core.Models.Entities;
using Exam.Web.Core.Network.Hub.Interfaces;
using Exam.Web.Core.Services.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NLog;

namespace Exam.Web.Infrastructure.Network.Hubs.Implementation
{
    public class SignalRHub : Hub
    {
        private readonly ILogger<SignalRHub> m_logger;
        private readonly IStudentService m_studentService;
        private readonly ICategoryService m_categoryService;
        private readonly ICorrectResponsesService m_correctResponsesService;
        private readonly IGameService m_gameService;
        private readonly IResponseService m_responseService;
        private readonly IRoundService m_roundService;
        private readonly IStudentToGameService m_studentToGameService;

        private readonly Dictionary<string, List<string>> listOfCategories = new Dictionary<string, List<string>>()
        {
            {"fruits", new List<string>() {"apples", "grapes"}},
            {"cities", new List<string>() {"paris", "bucharest"}},
            {"countries", new List<string>() {"romania"}}
        };
        

        public SignalRHub(ILogger<SignalRHub> logger, IStudentService studentService, ICategoryService categoryService, ICorrectResponsesService correctResponsesService,
            IGameService gameService, IResponseService responseService, IRoundService roundService, IStudentToGameService studentToGameService)
        {
            m_logger = logger;
            m_studentService = studentService;
            m_categoryService = categoryService;
            m_correctResponsesService = correctResponsesService;
            m_gameService = gameService;
            m_responseService = responseService;
            m_roundService = roundService;
            m_studentToGameService = studentToGameService;
        }

        public async Task Start(string data)
        {
            var getStudent = await m_studentService.SearchAsync(new Pagination(), new StudentFilter
            {
                SearchTerm = data
            });

            var student = getStudent.Item2.FirstOrDefault();
            var game = await CreateGameIfNotExistsAsync();

            var registries = await m_studentToGameService.SearchAsync(new Pagination(), new SimpleFilter<StudentToGame>());
            var gameRegistries = registries.Item2.Where(t => t.GameId.Equals(game.Id)).ToList();
            if (gameRegistries.Count >= 3) await Clients.All.SendCoreAsync("start", new object[] {"game is already full"});
            if (gameRegistries.Count < 3)
            {
                await m_studentToGameService.AddAsync(new StudentToGame
                {
                    GameId = game.Id,
                    StudentId = student.Id
                });
            }

            if (gameRegistries.Count.Equals(3))
            {
                var students = await m_studentService.SearchAsync(new Pagination(), new SimpleFilter<Student>());
                var usernames = new List<string>();
                foreach (var s in students.Item2)
                {
                    usernames.Add(s.Username);
                }

                await Clients.All.SendCoreAsync("setStudentsUsernames", new object[] {usernames});

                var roundCategoryValue = await CreateRoundAsync();
                await Clients.All.SendCoreAsync("getStatusGame", new object[] {"gameStarted", 1, roundCategoryValue});
            }
            else
                await Clients.All.SendCoreAsync("getStatusGame", new object[] {"Waiting for other players"});
        }


        public async Task GetNumberOfPlayers()
        {
            var (count, games) = await m_gameService.SearchAsync(new Pagination(), new SimpleFilter<Game>());
            await Clients.All.SendCoreAsync("getNumberOfPlayers", new object[] {count});
        }

        private async Task<Game> CreateGameIfNotExistsAsync()
        {
            var game = new Game {Id = Guid.NewGuid()};
            var (count, games) = await m_gameService.SearchAsync(new Pagination(), new SimpleFilter<Game>());
            if (count == 0)
                await m_gameService.AddAsync(game);
            else
            {
                game = games.First(t => t.State.Equals(GameState.Pending));
            }

            return game;
        }

        private async Task<string> CreateRoundAsync()
        {
            var (_, games) = await m_gameService.SearchAsync(new Pagination(), new SimpleFilter<Game>());
            var game = games.First();
            var round = new Round {Id = Guid.NewGuid(), GameId = game.Id};
            await m_roundService.AddAsync(round);
            var rand = new Random();
            var categoryValue = listOfCategories.ElementAt(rand.Next(0, listOfCategories.Count)).Key;
            var category = new Category {Id = Guid.NewGuid(), RoundId = round.Id, Value = categoryValue};
            await m_categoryService.AddAsync(category);
            foreach (KeyValuePair<string, List<string>> entry in listOfCategories)
            {
                if (entry.Key.Equals(categoryValue))
                {
                    foreach (string value in entry.Value)
                    {
                        await m_correctResponsesService.AddAsync(new CorrectResponses {Id = Guid.NewGuid(), CategoryId = category.Id, Values = value});
                    }
                }
            }

            return categoryValue;
        }

        public async Task SetAnswerForPlayer(string username, int noRound, string answerValue)
        {
            var (_, games) = await m_gameService.SearchAsync(new Pagination(), new SimpleFilter<Game>());
            var game = games.First();

            var rounds = await m_roundService.SearchAsync(new Pagination(), new SimpleFilter<Round>());
            //change with noRound
             var round = rounds.Item2.ElementAt(noRound-1);
             var getStudent = await m_studentService.SearchAsync(new Pagination(), new StudentFilter
            {
                SearchTerm = username
            });

            var student = getStudent.Item2.FirstOrDefault();
            var response = new Response {Id = Guid.NewGuid(), RoundId = round.Id, StudentId = student.Id, Value = answerValue};

            var validateResponse = await ValidateResponse(response);
            await m_responseService.AddAsync(validateResponse);

            await Clients.All.SendCoreAsync("setScoreForResponse", new object[] {student.Username, validateResponse.Score});

            var (count, _) = await m_responseService.SearchAsync(new Pagination(), new SimpleFilter<Response>());
            if (count % 3 == 0 && count<9)
            {
                var category = await CreateRoundAsync();
                noRound += 1;
                await Clients.All.SendCoreAsync("getStatusGame", new object[] {"gameStarted", noRound, category});
            }
            if (count == 9 )
            {
                await Clients.All.SendCoreAsync("getStatusGame", new object[] {"gameOver"});
            }
        }

        public async Task<Response> ValidateResponse(Response response)
        {
            var (count, categories) = await m_categoryService.SearchAsync(new Pagination(), new SimpleFilter<Category>());
            string categoryValue = "";
            foreach (var category in categories)
            {
                if (category.RoundId == response.RoundId)
                {
                    categoryValue = category.Value;
                    break;
                }
            }

            if (!listOfCategories[categoryValue].Contains(response.Value) || response.Value.Equals(""))
            {
                response.Score = 0;
            }

            if (listOfCategories[categoryValue].Contains(response.Value))
            {
                response.Score = 5;
            }

            return response;
        }
    }
}