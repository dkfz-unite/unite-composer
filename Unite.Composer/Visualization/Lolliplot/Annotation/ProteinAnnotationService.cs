
using Unite.Composer.Clients.Ensembl;
using Unite.Composer.Clients.Ensembl.Configuration.Options;
using Unite.Composer.Clients.Ensembl.Resources;
using Unite.Composer.Visualization.Lolliplot.Annotation.Models;

namespace Unite.Composer.Visualization.Lolliplot.Annotation;

public class ProteinAnnotationService
{
    private readonly EnsemblApiClient _ensemblApiClient;


    public ProteinAnnotationService(IEnsemblOptions ensemblOptions)
    {
        _ensemblApiClient = new EnsemblApiClient(ensemblOptions);
    }


    /// <summary>
    /// Retreives protein data with given Ensembl id from given source.
    /// </summary>
    /// <param name="ensemblId">Ensembl identifier</param>
    /// <returns>Protein data.</returns>
    public async Task<Protein> FindProtein(string ensemblId)
    {
        var proteinResource = await _ensemblApiClient.Find<ProteinResource>(ensemblId, true, true);

        if (proteinResource == null)
        {
            return null;
        }

        var protein = new Protein
        {
            Id = proteinResource.Id,
            Symbol = proteinResource.TranscriptId,
            Description = proteinResource.TranscriptId,
            Length = proteinResource.Length,

            Domains = proteinResource.Features?.Select(feature => new ProteinDomain{
                Id = feature.Name,
                Symbol = feature.Name,
                Description = feature.Description,
                Start = feature.SeqStart,
                End = feature.SeqEnd
            })
        };

        return protein;
    }
}
