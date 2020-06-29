using System;
using Exam.Web.Core.Helpers.Interfaces.Commons;

namespace Exam.Web.Core.Models.Entities
{
    public class StudentToGame : IIdentifier
    {
        public Guid Id { get; set; }
        
        public Guid StudentId  { get; set; }
        public Guid GameId  { get; set; }
        
        public Student Student  { get; set; }
        public Game Game  { get; set; }
        
    }
}