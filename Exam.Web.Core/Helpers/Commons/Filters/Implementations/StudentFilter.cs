﻿using System;
using System.Collections.Generic;
using System.Linq;
using Exam.Web.Core.Extensions;
using Exam.Web.Core.Helpers.Commons.Filters.Interfaces;
using Exam.Web.Core.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Exam.Web.Core.Helpers.Commons.Filters.Implementations
{
    public class StudentFilter : IFilter<Student>
    {
        public string SearchTerm { get; set; }
        public List<Guid> Ids { get; set; }

        public IQueryable<Student> Filter(IQueryable<Student> filterQuery)
        {
            if (string.IsNullOrEmpty(SearchTerm))
                return filterQuery;

            filterQuery = Guid.TryParse(SearchTerm, out var guid)
                ? filterQuery.Where(p => p.Id.Equals(guid))
                : filterQuery.Where(p => EF.Functions.Like(p.Username, SearchTerm.ToMySqlLikeSyntax()));

            if (Ids == null || !Ids.Any())
                return filterQuery;
            

            Ids.ForEach(t => { filterQuery = filterQuery.Where(p => p.Id.Equals(t)); });
            return filterQuery;
        }
    }
}