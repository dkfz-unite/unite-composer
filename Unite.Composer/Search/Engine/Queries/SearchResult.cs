namespace Unite.Composer.Search.Engine.Queries;

public class SearchResult<TIndex> where TIndex : class
{
    public long Total { get; set; }
    public IEnumerable<TIndex> Rows { get; set; }
    public IDictionary<string, IDictionary<string, long>> Aggregations { get; set; }

    public SearchResult()
    {
        Total = 0;
        Rows = Enumerable.Empty<TIndex>();
        Aggregations = new Dictionary<string, IDictionary<string, long>>();
    }
}
