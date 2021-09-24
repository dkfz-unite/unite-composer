using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Unite.Composer.Visualization.Lolliplot.Annotations.Clients.Ensembl.Configuration.Options;
using Unite.Composer.Visualization.Lolliplot.Annotations.Clients.Ensembl.Resources;

namespace Unite.Composer.Visualization.Lolliplot.Annotations.Clients.Ensembl
{
    public class EnsemblApiClient
    {
        private const string _xrefsOneUrl = @"/xrefs/id/{0}";
        private const string _sequenceUrl = @"/sequence/id/{0}";

        private readonly IEnsemblOptions _options;
        private readonly ILogger _logger;


        public EnsemblApiClient(IEnsemblOptions options)
        {
            _options = options;
            _logger = NullLogger<EnsemblApiClient>.Instance;
        }

        public EnsemblApiClient(IEnsemblOptions options, ILogger<EnsemblApiClient> logger)
        {
            _options = options;
            _logger = logger;
        }


        /// <summary>
        /// Performs cross reference search for Ensembl object with given identifier in given database source.
        /// </summary>
        /// <param name="ensemblId">Ensembl object identifier</param>
        /// <param name="source">External database source source (all by default)</param>
        /// <returns>Array of reference resources.</returns>
        public async Task<ReferenceResource[]> Xrefs(string ensemblId, string source = null)
        {
            using var httpClient = new JsonHttpClient(_options.Host);

            var url = string.Format(_xrefsOneUrl, ensemblId);

            if (!string.IsNullOrWhiteSpace(source))
            {
                url += $"?external_db={source}";
            }

            var acceptJson = (name: "Accept", value: "application/json");

            try
            {
                return await httpClient.GetAsync<ReferenceResource[]>(url, acceptJson);
            }
            catch (Exception exception)
            {
                _logger.LogWarning(exception.Message);

                return null;
            }
        }

        /// <summary>
        /// Retrieves sequence for Ensembl object with given identifier.
        /// </summary>
        /// <param name="ensemblId">Ensembl object identifier</param>
        /// <returns>Sequence resource.</returns>
        public async Task<SequenceResource> Sequence(string ensemblId)
        {
            using var httpClient = new JsonHttpClient(_options.Host);

            var url = string.Format(_sequenceUrl, ensemblId);

            var acceptJson = (name: "Accept", value: "application/json");

            try
            {
                return await httpClient.GetAsync<SequenceResource>(url, acceptJson);
            }
            catch (Exception exception)
            {
                _logger.LogWarning(exception.Message);

                return null;
            }
        }
    }
}
