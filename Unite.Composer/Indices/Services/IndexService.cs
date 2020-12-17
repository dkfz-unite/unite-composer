using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nest;
using Unite.Composer.Indices.Criteria;
using Unite.Composer.Indices.Services.Extensions;
using Unite.Indices.Services.Configuration.Options;

namespace Unite.Composer.Indices.Services
{
    public abstract class IndexService<T> : IIndexService<T>
        where T : class
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


        public virtual async Task<T> FindAsync(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return default;
            }

            var response = await _client.GetAsync<T>(key);

            if (response.IsValid)
            {
                return response.Source;
            }
            else
            {
                return null;
            }
        }

        public virtual async Task<SearchResult<T>> FindAllAsync(SearchCriteria searchCriteria = null)
        {
            var criteria = searchCriteria ?? new SearchCriteria();

            var request = CreateRequest(criteria);

            var response = await _client.SearchAsync<T>(request);

            return CreateResult(response);
        }


        public virtual T Find(string key)
        {
            return FindAsync(key).GetAwaiter().GetResult();
        }

        public virtual SearchResult<T> FindAll(SearchCriteria searchCriteria = null)
        {
            return FindAllAsync(searchCriteria).GetAwaiter().GetResult();
        }


        protected virtual ISearchRequest<T> CreateRequest(SearchCriteria searchCriteria)
        {
            var request = new SearchDescriptor<T>();

            request.AddLimits(searchCriteria.From, searchCriteria.Size);

            request.AddMultiMatchQuery(searchCriteria.Term);

            return request;
        }

        protected virtual SearchResult<T> CreateResult(ISearchResponse<T> response)
        {
            if (!response.IsValid)
            {
                return new SearchResult<T>();
            }

            var result = new SearchResult<T>();

            result.Total = response.Total;
            result.Rows = response.Documents;

            return result;
        }


        protected IEnumerable<int> SafeCast(IEnumerable<string> values)
        {
            if (values != null && values.Any())
            {
                foreach (var value in values)
                {
                    if (int.TryParse(value, out int number))
                    {
                        yield return number;
                    }
                }
            }
        }
    }
}
