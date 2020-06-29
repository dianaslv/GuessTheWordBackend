using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Exam.Web.Core.Helpers.Interfaces.Commons;

namespace Exam.Web.Core.Models.Entities
{
    public class Category : IIdentifier
    {
        public Guid Id { get; set; }
        public Round Round { get; set; }
        public Guid RoundId { get; set; }
        public List<CorrectResponses> CorrectResponses { get; set; }
        [Column(TypeName = "varchar(256)")]public string Value { get; set; }
    }
}