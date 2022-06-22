using System.Linq.Expressions;
using Nest;
using Unite.Composer.Search.Engine.Extensions;
using Unite.Composer.Search.Engine.Filters;

namespace Unite.Composer.Search.Engine.Queries;

public class SearchQuery<TIndex> where TIndex : class
{
    private SearchDescriptor<TIndex> _request;
    private List<IFilter<TIndex>> _filters;
    private List<Expression<Func<TIndex, object>>> _exclusions;

    public IEnumerable<IFilter<TIndex>> Filters => _filters;
    public IEnumerable<Expression<Func<TIndex, object>>> Exclusions => _exclusions;


    public SearchQuery()
    {
        _request = new SearchDescriptor<TIndex>();
        _filters = new List<IFilter<TIndex>>();
        _exclusions = new List<Expression<Func<TIndex, object>>>();

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

    public SearchQuery<TIndex> AddExclusion(Expression<Func<TIndex, object>> property)
    {
        _exclusions.Add(property);

        return this;
    }


    public ISearchRequest<TIndex> GetRequest()
    {
        if (_filters.Any())
        {
            _filters.ForEach(filter => filter.Apply(_request));
        }

        if (_exclusions.Any())
        {
            _request.Exclude(_exclusions.ToArray());
        }

        return _request;
    }
}
