using System.Linq;
using System.Threading.Tasks;
using Unite.Composer.Visualization.Lolliplot.Annotations.Clients.Ensembl;
using Unite.Composer.Visualization.Lolliplot.Annotations.Clients.Ensembl.Configuration.Options;
using Unite.Composer.Visualization.Lolliplot.Annotations.Clients.Ensembl.Models.Constants;
using Unite.Composer.Visualization.Lolliplot.Annotations.Clients.Ensembl.Resources;
using Unite.Composer.Visualization.Lolliplot.Annotations.Clients.Pfam;
using Unite.Composer.Visualization.Lolliplot.Annotations.Clients.Pfam.Configuration.Options;
using Unite.Composer.Visualization.Lolliplot.Annotations.Clients.Uniprot;
using Unite.Composer.Visualization.Lolliplot.Annotations.Clients.Uniprot.Configuration.Options;
using Unite.Composer.Visualization.Lolliplot.Annotations.Services.Models;
using Unite.Composer.Visualization.Lolliplot.Annotations.Services.Models.Enums;

namespace Unite.Composer.Visualization.Lolliplot.Annotations.Services
{
    public class ProteinAnnotationService
    {
        private readonly EnsemblApiClient _ensemblApiClient;
        private readonly UniprotApiClient _uniprotApiClient;
        private readonly PfamApiClient _pfamApiClient;


        public ProteinAnnotationService(
            IEnsemblOptions ensemblOptions,
            IUniprotOptions uniprotOptions,
            IPfamOptions pfamOptions)
        {
            _ensemblApiClient = new EnsemblApiClient(ensemblOptions);
            _uniprotApiClient = new UniprotApiClient(uniprotOptions);
            _pfamApiClient = new PfamApiClient(pfamOptions);
        }


        /// <summary>
        /// Retreives protein data with given Ensembl id from given source.
        /// </summary>
        /// <param name="ensemblId">Ensembl identifier</param>
        /// <param name="source">Protein data source</param>
        /// <returns>Protein data.</returns>
        public async Task<Protein> FindProtein(string ensemblId, AnnotationSource source)
        {
            var references = await FindReferences(ensemblId);

            if (references == null)
            {
                return null;
            }

            if (source == AnnotationSource.Pfam)
            {
                return await FindPfamProtein(references[0].Id);
            }
            else
            {
                return await FindUniprotProtein(references[0].Id);
            }
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
        /// Retrieves protein data from Pfam public API.
        /// </summary>
        /// <param name="accessionId">Accession identifier</param>
        /// <returns>Protein data.</returns>
        private async Task<Protein> FindPfamProtein(string accessionId)
        {
            var proteinResource = await _pfamApiClient.Protein(accessionId);

            if (proteinResource == null)
            {
                return null;
            }

            var protein = new Protein
            {
                Id = proteinResource.Id,
                Symbol = proteinResource.Symbol,
                Description = proteinResource.Description,
                Length = proteinResource.Sequence.Length,

                Domains = proteinResource.Domains?
                    .Select(domainResource => new ProteinDomain
                    {
                        Id = domainResource.Id,
                        Symbol = domainResource.Symbol,
                        Description = null,
                        Start = domainResource.Location.Start,
                        End = domainResource.Location.End
                    })
            };

            return protein;
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
                    .SelectMany(domainResource => domainResource.Locations
                        .SelectMany(locationResource => locationResource.Fragments)
                        .Select(fragmentResource => new ProteinDomain
                        {
                            Id = domainResource.Id,
                            Symbol = null,
                            Description = null,
                            Start = fragmentResource.Start,
                            End = fragmentResource.End
                        })
                    )
            };

            return protein;
        }
    }
}
