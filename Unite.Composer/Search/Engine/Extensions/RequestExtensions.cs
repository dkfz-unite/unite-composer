using System.Linq.Expressions;
using Nest;

namespace Unite.Composer.Search.Engine.Extensions;

internal static class RequestExtensions
{
    /// <summary>
    /// Adds limits to given request if limits are set.
    /// </summary>
    /// <typeparam name="TIndex">Index type</typeparam>
    /// <param name="request">Sourse request</param>
    /// <param name="from">Start position</param>
    /// <param name="size">Number of rows</param>
    public static void AddLimits<TIndex>(this ISearchRequest<TIndex> request, int? from, int? size)
    {
        request.From = from;
        request.Size = size;
    }


    /// <summary>
    /// Includes filed to given request results.
    /// </summary>
    /// <typeparam name="TIndex">Index type</typeparam>
    /// <typeparam name="TProp">Field type</typeparam>
    /// <param name="request">Source request</param>
    /// <param name="property">Property to include</param>
    public static void Include<TIndex, TProp>(this ISearchRequest<TIndex> request,
        Expression<Func<TIndex, TProp>> property)
        where TIndex : class
    {
        request.Source = new SourceFilterDescriptor<TIndex>().Includes(x => x.Field(property));
    }

    /// <summary>
    /// Includes fields to given request results.
    /// </summary>
    /// <typeparam name="TIndex">Index type</typeparam>
    /// <param name="request">Source request</param>
    /// <param name="properties">Properties to include</param>
    public static void Include<TIndex>(this ISearchRequest<TIndex> request,
        params Expression<Func<TIndex, object>>[] properties)
        where TIndex : class
    {
        request.Source = new SourceFilterDescriptor<TIndex>().Includes(x => x.Fields(properties));
    }


    /// <summary>
    /// Excludes field from given request results.
    /// </summary>
    /// <typeparam name="TIndex">Index type</typeparam>
    /// <typeparam name="TProp">Property type</typeparam>
    /// <param name="request">Source request</param>
    /// <param name="property">Property to exclude</param>
    public static void Exclude<TIndex, TProp>(this ISearchRequest<TIndex> request,
        Expression<Func<TIndex, TProp>> property)
        where TIndex : class
    {
        request.Source = new SourceFilterDescriptor<TIndex>().Excludes(x => x.Field(property));
    }

    /// <summary>
    /// Excludes properties from given request results.
    /// </summary>
    /// <typeparam name="TIndex">Index type</typeparam>
    /// <param name="request">Source request</param>
    /// <param name="properties">Properties to exclude</param>
    public static void Exclude<TIndex>(this ISearchRequest<TIndex> request,
        params Expression<Func<TIndex, object>>[] properties)
        where TIndex : class
    {
        request.Source = new SourceFilterDescriptor<TIndex>().Excludes(x => x.Fields(properties));
    }


    /// <summary>
    /// Adds ordering to given request results.
    /// </summary>
    /// <typeparam name="TIndex">Index type</typeparam>
    /// <typeparam name="TProp">Property type</typeparam>
    /// <param name="request">Source request</param>
    /// <param name="property">Ordering property</param>
    /// <param name="order">Ordering type</param>
    public static void OrderBy<TIndex, TProp>(this ISearchRequest<TIndex> request,
        Expression<Func<TIndex, TProp>> property,
        SortOrder order = SortOrder.Ascending)
        where TIndex : class
    {
        var sort = new FieldSortDescriptor<TIndex>()
            .Field(property)
            .Order(order);

        if (request.Sort != null)
        {
            request.Sort.Add(sort);
        }
        else
        {
            request.Sort = new List<ISort>
            {
                sort
            };
        }
    }
}
