using System.Threading.Tasks;
using Unite.Composer.Visualization.Lolliplot.Annotations.Clients.Pfam.Configuration.Options;
using Unite.Composer.Visualization.Lolliplot.Annotations.Clients.Pfam.Resources;

namespace Unite.Composer.Visualization.Lolliplot.Annotations.Clients.Pfam
{
    public class PfamApiClient
    {
        private const string _proteinUrl = @"/protein/{0}?output=xml";

        private readonly IPfamOptions _options;


        public PfamApiClient(IPfamOptions options)
        {
            _options = options;
        }


        public async Task<ProteinResource> Protein(string accessionId)
        {
            using var httpClient = new XmlHttpClient(_options.Host);

            var url = string.Format(_proteinUrl, accessionId);

            var resource = await httpClient.GetAsync<PfamResource<ProteinResource>>(url);

            return resource?.Entry;
        }
    }
}
