using System.Linq.Expressions;
using Nest;

namespace Unite.Composer.Search.Engine.Queries;

public class GetQuery<TIndex> where TIndex : class
{
    private GetDescriptor<TIndex> _request;


    public GetQuery(string key)
    {
        _request = new GetDescriptor<TIndex>(key);
    }


    public GetQuery<TIndex> AddExclusion<TProp>(Expression<Func<TIndex, TProp>> property)
    {
        _request.SourceExcludes(property);

        return this;
    }


    public IGetRequest<TIndex> GetRequest()
    {
        return _request;
    }
}
