using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Nest;

namespace Unite.Composer.Indices.Services.Extensions
{
    public static class AggregationExtensions
    {
        public static void AddTermsAggregation<T, TProp>(this ISearchRequest<T> request, string aggregationName, Expression<Func<T, TProp>> property)
        {
            if (request.Aggregations == null)
            {
                request.Aggregations = new AggregationDictionary();
            }

            var aggregation = new TermsAggregation(aggregationName)
            {
                Field = new Field(property),
            };

            request.Aggregations.Add(aggregationName, aggregation);
        }

        public static IDictionary<string, long> GetTermsAggregationData<T>(this ISearchResponse<T> response, string aggregationName)
            where T : class
        {
            return response.Aggregations.Terms(aggregationName)?.Buckets
                .Where(bucket => bucket.DocCount != null)
                .ToDictionary(bucket => bucket.Key, bucket => bucket.DocCount.Value);
        }
    }
}
