using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Exam.Web.Core.Helpers.Interfaces.Commons;

namespace Exam.Web.Core.Models.Entities
{
    public class Student : IIdentifier
    {
        public Guid Id { get; set; }
        [Column(TypeName = "varchar(256)")] public string Username { get; set; }
        [Column(TypeName = "varchar(256)")] public string Password { get; set; }
        
        public List<StudentToGame> StudentToGames { get; set; }
        public List<Response> Responses { get; set; }
        
    }
}