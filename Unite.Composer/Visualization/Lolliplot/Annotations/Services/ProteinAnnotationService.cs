using Unite.Composer.Visualization.Lolliplot.Annotations.Clients.Ensembl;
using Unite.Composer.Visualization.Lolliplot.Annotations.Clients.Ensembl.Configuration.Options;
using Unite.Composer.Visualization.Lolliplot.Annotations.Clients.Ensembl.Models.Constants;
using Unite.Composer.Visualization.Lolliplot.Annotations.Clients.Ensembl.Resources;
using Unite.Composer.Visualization.Lolliplot.Annotations.Clients.Uniprot;
using Unite.Composer.Visualization.Lolliplot.Annotations.Clients.Uniprot.Configuration.Options;
using Unite.Composer.Visualization.Lolliplot.Annotations.Services.Models;

namespace Unite.Composer.Visualization.Lolliplot.Annotations.Services;

public class ProteinAnnotationService
{
    private readonly EnsemblApiClient _ensemblApiClient;
    private readonly UniprotApiClient _uniprotApiClient;


    public ProteinAnnotationService(
        IEnsemblOptions ensemblOptions,
        IUniprotOptions uniprotOptions)
    {
        _ensemblApiClient = new EnsemblApiClient(ensemblOptions);
        _uniprotApiClient = new UniprotApiClient(uniprotOptions);
    }


    /// <summary>
    /// Retreives protein data with given Ensembl id from given source.
    /// </summary>
    /// <param name="ensemblId">Ensembl identifier</param>
    /// <param name="source">Protein data source</param>
    /// <returns>Protein data.</returns>
    public async Task<Protein> FindProtein(string ensemblId)
    {
        var references = await FindReferences(ensemblId);

        if (references?.Length < 1)
        {
            return null;
        }

        return await FindUniprotProtein(references[0].Id);
    }


    /// <summary>
    /// Retrieves Ensembl protein references in Uniprot/Swissprot or Uniprot/Sptrembl.
    /// </summary>
    /// <param name="ensemblId">Ensembl protein identifier</param>
    /// <returns>Array of protein references.</returns>
    private async Task<ReferenceResource[]> FindReferences(string ensemblId)
    {
        var references = await _ensemblApiClient.Xrefs(ensemblId, ProteinDataSources.UniprotSwissprot);

        if (references?.Length < 1)
        {
            references = await _ensemblApiClient.Xrefs(ensemblId, ProteinDataSources.UniprotSptrembl);
        }

        return references;
    }


    /// <summary>
    /// Retrieves protein data from Uniprot public API.
    /// </summary>
    /// <param name="accessionId">Accession identifier</param>
    /// <returns>Protein data.</returns>
    private async Task<Protein> FindUniprotProtein(string accessionId)
    {
        var proteinResource = await _uniprotApiClient.Protein(accessionId);

        if (proteinResource == null)
        {
            return null;
        }

        var protein = new Protein
        {
            Id = proteinResource.Metadata.Id,
            Symbol = proteinResource.Metadata.Symbol,
            Description = proteinResource.Metadata.Name,
            Length = proteinResource.Metadata.Length,

            Domains = proteinResource.Domains?
                .SelectMany(domain => domain.Locations
                    .SelectMany(location => location.Fragments)
                    .Select(fragment => new ProteinDomain
                    {
                        Id = domain.Id,
                        Symbol = null,
                        Description = null,
                        Start = fragment.Start,
                        End = fragment.End
                    })
                )
        };

        return protein;
    }
}
