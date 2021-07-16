using System;
using System.Collections.Generic;
using System.Linq;

namespace Unite.Composer.Common.Extensions
{
    public static class CollectionExtensions
    {
        public static IEnumerable<T> DistinctBy<T, TProp>(this IEnumerable<T> source, Func<T, TProp> property)
        {
            return source
                .GroupBy(property)
                .Select(group => group.First());
        }
    }
}
