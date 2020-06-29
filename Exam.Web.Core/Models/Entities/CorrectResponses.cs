using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Exam.Web.Core.Helpers.Interfaces.Commons;

namespace Exam.Web.Core.Models.Entities
{
    public class CorrectResponses : IIdentifier
    {
        public Guid Id { get; set; }
        public Guid CategoryId { get; set; }
        public Category Category { get; set; }
        public string Values { get; set; }

    }
}