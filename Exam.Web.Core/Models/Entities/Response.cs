using System;
using System.ComponentModel.DataAnnotations.Schema;
using Exam.Web.Core.Helpers.Interfaces.Commons;

namespace Exam.Web.Core.Models.Entities
{
    public class Response : IIdentifier
    {
        public Guid Id { get; set; }
        public Guid RoundId { get; set; }
        public Round Round { get; set; }
        [Column(TypeName = "varchar(256)")]public string Value { get; set; }
        
        public int Score { get; set; }
        public Guid StudentId { get; set; }
        public Student Student { get; set; }
    }
}