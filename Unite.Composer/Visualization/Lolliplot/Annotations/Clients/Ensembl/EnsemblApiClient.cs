using System.Threading.Tasks;
using Unite.Composer.Visualization.Lolliplot.Annotations.Clients.Ensembl.Configuration.Options;
using Unite.Composer.Visualization.Lolliplot.Annotations.Clients.Ensembl.Resources;

namespace Unite.Composer.Visualization.Lolliplot.Annotations.Clients.Ensembl
{
    public class EnsemblApiClient
    {
        private const string _xrefsOneUrl = @"/xrefs/id/{0}";

        private readonly IEnsemblOptions _options;

        public EnsemblApiClient(IEnsemblOptions options)
        {
            _options = options;
        }

        /// <summary>
        /// Searching references of Ensembl object with given identifier in databases of given types.
        /// </summary>
        /// <typeparam name="T">Object type for decerialization</typeparam>
        /// <param name="ensemblid">Ensembl object identifier</param>
        /// <param name="source">Specific type of external source of information (all by default)</param>
        /// <returns>References of the object stored in ensembl given ensembl databases if were found.</returns>
        public async Task<ReferenceResource[]> Xrefs(string ensemblid, string source = null)
        {
            using var httpClient = new JsonHttpClient(_options.Host);

            var url = string.Format(_xrefsOneUrl, ensemblid);

            if (!string.IsNullOrWhiteSpace(source))
            {
                url += $"?external_db={source}";
            }

            var acceptJson = (name: "Accept", value: "application/json");

            var resource = await httpClient.GetAsync<ReferenceResource[]>(url, acceptJson);

            return resource;
        }
    }
}
