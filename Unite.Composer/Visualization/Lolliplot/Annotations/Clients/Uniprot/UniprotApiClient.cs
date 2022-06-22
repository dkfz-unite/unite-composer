using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Unite.Composer.Visualization.Lolliplot.Annotations.Clients.Uniprot.Configuration.Options;
using Unite.Composer.Visualization.Lolliplot.Annotations.Clients.Uniprot.Resources;

namespace Unite.Composer.Visualization.Lolliplot.Annotations.Clients.Uniprot;

public class UniprotApiClient
{
    private const string _proteinUrl = @"/interpro/api/protein/uniprot/{0}/entry/pfam";

    private readonly IUniprotOptions _options;
    private readonly ILogger _logger;


    public UniprotApiClient(IUniprotOptions options)
    {
        _options = options;
        _logger = NullLogger<UniprotApiClient>.Instance;
    }

    public UniprotApiClient(IUniprotOptions options, ILogger<UniprotApiClient> logger)
    {
        _options = options;
        _logger = logger;
    }


    /// <summary>
    /// Retrieves protein data from Uniprot for given accession identifier.
    /// </summary>
    /// <param name="accessionId">Accession identifier</param>
    /// <returns>Protein with domains.</returns>
    public async Task<ProteinResource> Protein(string accessionId)
    {
        using var httpClient = new JsonHttpClient(_options.Host, true);

        var url = string.Format(_proteinUrl, accessionId);

        try
        {
            return await httpClient.GetAsync<ProteinResource>(url);
        }
        catch (Exception exception)
        {
            _logger.LogWarning(exception.Message);

            return null;
        }
    }
}
