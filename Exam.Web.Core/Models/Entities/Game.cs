using System;
using System.Collections.Generic;
using Exam.Web.Core.Enums;
using Exam.Web.Core.Helpers.Interfaces.Commons;

namespace Exam.Web.Core.Models.Entities
{
    public class Game : IIdentifier
    {
        public Guid Id { get; set; }
        public GameState State { get; set; }
        public List<StudentToGame> StudentToGames { get; set; }
        public List<Round> Rounds { get; set; }
    }
}