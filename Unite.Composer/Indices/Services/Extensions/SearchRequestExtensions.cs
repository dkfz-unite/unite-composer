using System;
using System.Linq.Expressions;
using Nest;

namespace Unite.Composer.Indices.Services.Extensions
{
    internal static class SearchRequestExtensions
    {
        public static void AddLimits<T>(this ISearchRequest<T> request, int? from, int? size)
        {
            request.From = from;
            request.Size = size;
        }

        public static void Include<T>(this ISearchRequest<T> request,
            params Expression<Func<T, object>>[] fields)
            where T : class
        {
            request.Source = new SourceFilterDescriptor<T>().Includes(x => x.Fields(fields));
        }

        public static void Exclude<T>(this ISearchRequest<T> request,
            params Expression<Func<T, object>>[] fields)
            where T : class
        {
            request.Source = new SourceFilterDescriptor<T>().Excludes(x => x.Fields(fields));
        }
    }
}
