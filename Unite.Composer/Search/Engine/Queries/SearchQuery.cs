using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Nest;
using Unite.Composer.Search.Engine.Extensions;
using Unite.Composer.Search.Engine.Filters;

namespace Unite.Composer.Search.Engine.Queries
{
    public class SearchQuery<TIndex> where TIndex : class
    {
        private SearchDescriptor<TIndex> _request;
        private List<IFilter<TIndex>> _filters;

        public IEnumerable<IFilter<TIndex>> Filters => _filters;


        public SearchQuery()
        {
            _request = new SearchDescriptor<TIndex>();
            _filters = new List<IFilter<TIndex>>();

            _request.TrackTotalHits();
        }


        public SearchQuery<TIndex> AddPagination(int from, int size)
        {
            _request.AddLimits(from, size);

            return this;
        }

        public SearchQuery<TIndex> AddFullTextSearch(string query)
        {
            _request.AddMultiMatchQuery(query);

            return this;
        }

        public SearchQuery<TIndex> AddFilter(IFilter<TIndex> filter)
        {
            _filters.Add(filter);

            return this;
        }

        public SearchQuery<TIndex> AddFilters(IEnumerable<IFilter<TIndex>> filters)
        {
            _filters.AddRange(filters);

            return this;
        }

        public SearchQuery<TIndex> AddOrdering<TProp>(Expression<Func<TIndex, TProp>> property, bool ascending = false)
        {
            _request.OrderBy(property, ascending ? SortOrder.Ascending : SortOrder.Descending);

            return this;
        }

        public SearchQuery<TIndex> AddExclusion<TProp>(Expression<Func<TIndex, TProp>> property)
        {
            _request.Exclude(property);

            return this;
        }


        public ISearchRequest<TIndex> GetRequest()
        {
            _filters.ForEach(filter => filter.Apply(_request));

            return _request;
        }
    }
}
