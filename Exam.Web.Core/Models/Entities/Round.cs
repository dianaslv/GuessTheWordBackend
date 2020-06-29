using System;
using System.Collections.Generic;
using Exam.Web.Core.Helpers.Interfaces.Commons;

namespace Exam.Web.Core.Models.Entities
{
    public class Round : IIdentifier
    {
        public Guid Id { get; set; }
        public Guid GameId { get; set; }
        public Game Game { get; set; }
        public List<Response> Responses { get; set; }
        public Category Category { get; set; }
        
    }
}