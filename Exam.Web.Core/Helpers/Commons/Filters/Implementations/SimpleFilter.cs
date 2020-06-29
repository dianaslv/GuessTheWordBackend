using System;
using System.Collections.Generic;
using System.Linq;
using Exam.Web.Core.Helpers.Commons.Filters.Interfaces;
using Exam.Web.Core.Helpers.Interfaces.Commons;

namespace Exam.Web.Core.Helpers.Commons.Filters.Implementations
{
    public class SimpleFilter<TAny> : IFilter<TAny> where TAny : IIdentifier
    {
        public string SearchTerm { get; set; }
        public List<Guid> Ids { get; set; }

        public IQueryable<TAny> Filter(IQueryable<TAny> filterQuery)
        {
            if (string.IsNullOrEmpty(SearchTerm))
                return filterQuery;

            filterQuery = Guid.TryParse(SearchTerm, out var guid)
                ? filterQuery.Where(p => p.Id.Equals(guid))
                : filterQuery;

            if (Ids == null || !Ids.Any())
                return filterQuery;

            Ids.ForEach(t => { filterQuery = filterQuery.Where(p => p.Id.Equals(t)); });
            return filterQuery;
        }
    }
}