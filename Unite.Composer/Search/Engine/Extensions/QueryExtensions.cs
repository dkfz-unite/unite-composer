using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Nest;

namespace Unite.Composer.Search.Engine.Extensions
{
    public static class QueryExtensions
    {
        /// <summary>
        /// Adds 'MultyMatch' query to given request if filter value is set.
        /// Creates new query or adds query to existing request query with logical 'AND' operator.
        /// </summary>
        /// <typeparam name="TIndex">Index type</typeparam>
        /// <param name="request">Source request</param>
        /// <param name="value">Filter value</param>
        public static void AddMultiMatchQuery<TIndex>(this ISearchRequest<TIndex> request,
            string value)
            where TIndex : class
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return;
            }

            var query = Query<TIndex>.MultiMatch(d => d
                .Query(value)
            );

            request.Query = SetOrAdd(request.Query, query);
        }


        /// <summary>
        /// Adds 'Match' query to given request if filter values are set.
        /// Creates new query or adds query to existing request query with logical 'AND' operator.
        /// All separate filter values are combined with logical 'OR' operator.
        /// </summary>
        /// <typeparam name="TIndex">Index type</typeparam>
        /// <typeparam name="TProp">Property type</typeparam>
        /// <param name="request">Source request</param>
        /// <param name="property">Filter property</param>
        /// <param name="values">Filter values</param>
        public static void AddMatchQuery<TIndex, TProp>(this ISearchRequest<TIndex> request,
            Expression<Func<TIndex, TProp>> property,
            IEnumerable<string> values)
            where TIndex : class
        {
            if (values == null || !values.Any())
            {
                return;
            }

            var query = values
                .Select(value =>
                    Query<TIndex>.Match(match => match
                        .Field(property)
                        .Query(value)
                        .Operator(Operator.And)
                    )
                )
                .Aggregate((left, right) => left || right);

            request.Query = SetOrAdd(request.Query, query);
        }

        /// <summary>
        /// Adds 'Match' query to given request if filter values are set.
        /// Creates new query or adds query to existing request query with logical 'AND' operator.
        /// Property queries are combined with logical 'OR' operator.
        /// </summary>
        /// <typeparam name="TIndex">Index type</typeparam>
        /// <typeparam name="TProp">Property type</typeparam>
        /// <param name="request">Source request</param>
        /// <param name="property1">First filter property</param>
        /// <param name="property2">Second filter property</param>
        /// <param name="values">Filter values</param>
        public static void AddMatchQuery<TIndex, TProp>(this ISearchRequest<TIndex> request,
            Expression<Func<TIndex, TProp>> property1,
            Expression<Func<TIndex, TProp>> property2,
            IEnumerable<string> values)
            where TIndex : class
        {
            if (values == null || !values.Any())
            {
                return;
            }

            var query1 = values
                .Select(value =>
                    Query<TIndex>.Match(match => match
                        .Field(property1)
                        .Query(value)
                        .Operator(Operator.And)
                    )
                )
                .Aggregate((left, right) => left || right);

            var query2 = values
                .Select(value =>
                    Query<TIndex>.Match(match => match
                        .Field(property2)
                        .Query(value)
                        .Operator(Operator.And)
                    )
                )
                .Aggregate((left, right) => left || right);

            var query = query1 || query2;

            request.Query = SetOrAdd(request.Query, query);
        }


        /// <summary>
        /// Adds 'Terms' query to given request if filter values are set.
        /// Creates new query or adds query to existing request query with logical 'AND' operator.
        /// </summary>
        /// <typeparam name="TIndex">Index type</typeparam>
        /// <typeparam name="TProp">Property type</typeparam>
        /// <param name="request">Source request</param>
        /// <param name="property">Filter property</param>
        /// <param name="values">Filter values</param>
        public static void AddTermsQuery<TIndex, TProp>(this ISearchRequest<TIndex> request,
            Expression<Func<TIndex, TProp>> property,
            IEnumerable<TProp> values)
            where TIndex : class
        {
            if (values == null || !values.Any())
            {
                return;
            }

            var query = Query<TIndex>.Terms(d => d
                .Field(property)
                .Terms(values)
            );

            request.Query = SetOrAdd(request.Query, query);
        }

        /// <summary>
        /// Adds 'Terms' query to given request if filter values are set.
        /// Creates new query or adds query to existing request query with logical 'AND' operator.
        /// Property queries are combined with logical 'OR' operator.
        /// </summary>
        /// <typeparam name="TIndex">Index type</typeparam>
        /// <typeparam name="TProp">Property type</typeparam>
        /// <param name="request">Source request</param>
        /// <param name="property1">First filter property</param>
        /// <param name="property2">Second filter property</param>
        /// <param name="values">Filter values</param>
        public static void AddTermsQuery<TIndex, TProp>(this ISearchRequest<TIndex> request,
            Expression<Func<TIndex, TProp>> property1,
            Expression<Func<TIndex, TProp>> property2,
            IEnumerable<TProp> values)
            where TIndex : class
        {
            if (values == null || !values.Any())
            {
                return;
            }

            var query1 = Query<TIndex>.Terms(d => d
                .Field(property1)
                .Terms(values)
            );

            var query2 = Query<TIndex>.Terms(d => d
                .Field(property2)
                .Terms(values)
            );

            var query = query1 || query2;

            request.Query = SetOrAdd(request.Query, query);
        }


        /// <summary>
        /// Adds boolean 'Terms' query to given request if values is set.
        /// Creates new query or adds query to existing request query with logical 'AND' operator.
        /// </summary>
        /// <typeparam name="TIndex">Index type</typeparam>
        /// <param name="request">Source request</param>
        /// <param name="property">Filter property</param>
        /// <param name="value">Filter value</param>
        public static void AddBoolQuery<TIndex>(this ISearchRequest<TIndex> request,
            Expression<Func<TIndex, bool?>> property,
            bool? value)
            where TIndex : class
        {
            if (!value.HasValue)
            {
                return;
            }

            var query = Query<TIndex>.Term(term => term
                .Field(property)
                .Value(value)
            );

            request.Query = SetOrAdd(request.Query, query);
        }


        /// <summary>
        /// Adds 'Range' query to given request if any of range bounds is set.
        /// Creates new query or adds query to existing request query with logical 'AND' operator.
        /// </summary>
        /// <typeparam name="TIndex">Index type</typeparam>
        /// <typeparam name="TProp">Property type</typeparam>
        /// <param name="request">Source request</param>
        /// <param name="property">Filter property</param>
        /// <param name="from">Filter range left bound value</param>
        /// <param name="to">Filter range right bound value</param>
        public static void AddRangeQuery<TIndex, TProp>(this ISearchRequest<TIndex> request,
            Expression<Func<TIndex, TProp>> property,
            double? from,
            double? to)
            where TIndex : class
        {
            var query = Query<TIndex>.Range(d => d
                .Field(property)
                .GreaterThanOrEquals(from)
                .LessThanOrEquals(to)
            );

            request.Query = SetOrAdd(request.Query, query);
        }

        /// <summary>
        /// Adds 'Range' query to given request if any or gange bounds is set.
        /// Creates new query or adds query to existing request query with logical 'AND' operator.
        /// </summary>
        /// <typeparam name="TIndex">Index type</typeparam>
        /// <typeparam name="TProp">Property type</typeparam>
        /// <param name="request">Source request</param>
        /// <param name="propertyFrom">Filter property from</param>
        /// <param name="propertyTo">Filter property to</param>
        /// <param name="from">Filter range left bound value</param>
        /// <param name="to">Filter range right bound value</param>
        public static void AddRangeFilter<TIndex, TProp>(this ISearchRequest<TIndex> request,
            Expression<Func<TIndex, TProp>> propertyFrom,
            Expression<Func<TIndex, TProp>> propertyTo,
            double? from,
            double? to)
            where TIndex : class
        {
            var fromQuery = Query<TIndex>.Range(d => d
                .Field(propertyFrom)
                .GreaterThanOrEquals(from)
            );

            var toQuery = Query<TIndex>.Range(d => d
                .Field(propertyTo)
                .LessThanOrEquals(to)
            );

            var query = fromQuery && toQuery;

            request.Query = SetOrAdd(request.Query, query);
        }

        /// <summary>
        /// Adds 'Range' query to given request if any of range bounds is set.
        /// Creates new query or adds query to existing request query with logical 'AND' operator.
        /// Property queries are combined with logical 'OR' operator.
        /// </summary>
        /// <typeparam name="TIndex">Index type</typeparam>
        /// <typeparam name="TProp">Property type</typeparam>
        /// <param name="request">Source request</param>
        /// <param name="property">Filter property</param>
        /// <param name="from">Filter range left bound value</param>
        /// <param name="to">Filter range right bound value</param>
        public static void AddRangeQuery<TIndex, TProp>(this ISearchRequest<TIndex> request,
            Expression<Func<TIndex, TProp>> property1,
            Expression<Func<TIndex, TProp>> property2,
            double? from,
            double? to)
            where TIndex : class
        {
            var query1 = Query<TIndex>.Range(d => d
                .Field(property1)
                .GreaterThanOrEquals(from)
                .LessThanOrEquals(to)
            );

            var query2 = Query<TIndex>.Range(d => d
                .Field(property2)
                .GreaterThanOrEquals(from)
                .LessThanOrEquals(to)
            );

            var query = query1 || query2;

            request.Query = SetOrAdd(request.Query, query);
        }

        /// <summary>
        /// Adds 'Exists' query to check that given property is set.
        /// Creates new query or adds query to existing request query with logical 'AND' operator.
        /// </summary>
        /// <typeparam name="TIndex">Index type</typeparam>
        /// <typeparam name="TProp">Property type</typeparam>
        /// <param name="request">Source request</param>
        /// <param name="property">Filter property</param>
        public static void AddExistsQuery<TIndex, TProp>(this ISearchRequest<TIndex> request,
            Expression<Func<TIndex, TProp>> property)
            where TIndex : class
        {
            var query = Query<TIndex>.Exists(d => d
                .Field(property)
            );

            request.Query = SetOrAdd(request.Query, query);
        }


        private static QueryContainer SetOrAdd(QueryContainer sourceQuery, QueryContainer newQuery)
        {
            if (sourceQuery == null)
            {
                return newQuery;
            }
            else
            {
                return sourceQuery && newQuery;
            }
        }
    }
}
