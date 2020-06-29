using Exam.Web.Core.Helpers.Commons;
using Exam.Web.Core.Helpers.Commons.Filters.Interfaces;
using Exam.Web.Core.Helpers.Interfaces.Commons;

namespace Exam.Web.Core.Helpers.Network.Models
{
    public class SearchRequest<TEntity,TFilter> where TEntity:IIdentifier where TFilter : IFilter<TEntity>, new()
    {
        public Pagination Pagination { get; set; } = new Pagination();
        public TFilter Filtering { get; set; } = new TFilter();
    }
}