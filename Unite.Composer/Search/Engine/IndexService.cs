using Nest;
using Unite.Composer.Search.Engine.Extensions;
using Unite.Composer.Search.Engine.Queries;
using Unite.Indices.Services.Configuration.Options;

namespace Unite.Composer.Search.Engine;

public abstract class IndexService<TIndex> : IIndexService<TIndex>
    where TIndex : class
{
    protected readonly IElasticClient _client;

    protected abstract string DefaultIndex { get; }


    public IndexService(IElasticOptions options)
    {
        var host = new Uri(options.Host);

        var settings = new ConnectionSettings(host)
            .BasicAuthentication(options.User, options.Password)
            .DisableAutomaticProxyDetection()
            .DefaultIndex(DefaultIndex);

        _client = new ElasticClient(settings);
    }


    public virtual async Task<TIndex> GetAsync(GetQuery<TIndex> query)
    {
        var request = query.GetRequest();

        var response = await _client.GetAsync<TIndex>(request);

        if (response.IsValid)
        {
            return response.Source;
        }
        else
        {
            return null;
        }
    }

    public virtual async Task<SearchResult<TIndex>> SearchAsync(SearchQuery<TIndex> query)
    {
        var request = query.GetRequest();

        var response = await _client.SearchAsync<TIndex>(request);

        if (response.IsValid)
        {
            return new SearchResult<TIndex>()
            {
                Total = response.Total,
                Rows = response.Documents,
                Aggregations = query.Aggregations.ToDictionary(
                    name => name, 
                    name => response.GetTermsAggregationData<TIndex>(name)
                )
            };
        }
        else
        {
            return new SearchResult<TIndex>();
        }
    }

    public virtual async Task DeleteAsync()
    {
        var response = await _client.Indices.DeleteAsync(DefaultIndex);

        if (!response.IsValid)
        {
            throw new Exception(response.DebugInformation);
        }
    }
}
