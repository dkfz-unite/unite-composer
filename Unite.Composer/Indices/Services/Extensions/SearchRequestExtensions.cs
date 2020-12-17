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
    }
}
