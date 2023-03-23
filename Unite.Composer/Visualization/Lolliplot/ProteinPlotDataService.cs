using Unite.Composer.Clients.Ensembl.Configuration.Options;
using Unite.Composer.Search.Engine;
using Unite.Composer.Search.Engine.Filters;
using Unite.Composer.Search.Engine.Queries;
using Unite.Composer.Visualization.Lolliplot.Annotations.Services;
using Unite.Composer.Visualization.Lolliplot.Data;
using Unite.Data.Services;
using Unite.Indices.Entities.Variants;
using Unite.Indices.Services.Configuration.Options;

namespace Unite.Composer.Visualization.Lolliplot;

public class ProteinPlotDataService
{
    private readonly DomainDbContext _dbContext;
    private readonly IIndexService<VariantIndex> _variantsIndexService;
    private readonly ProteinAnnotationService _proteinAnnotationService;


    public ProteinPlotDataService(
        DomainDbContext dbContext,
        IElasticOptions elasticOptions,
        IEnsemblOptions ensemblOptions)
    {
        _dbContext = dbContext;
        _variantsIndexService = new VariantsIndexService(elasticOptions);
        _proteinAnnotationService = new ProteinAnnotationService(ensemblOptions);
    }


    /// <summary>
    /// Retrieves data required to build protein plot.
    /// </summary>
    /// <param name="transcriptId">Transcript identifier</param>
    /// <returns>Protein plot data.</returns>
    public async Task<ProteinPlotData> LoadData(long transcriptId)
    {
        var data = new ProteinPlotData
        {
            Domains = await GetProteinDomains(transcriptId),
            Mutations = await GetProteinMutations(transcriptId)
        };

        return data;
    }


    /// <summary>
    /// Retrieves protein data from Pfam database for given protein coding transcript.
    /// </summary>
    /// <param name="transcriptId">Transcript identifier</param>
    /// <returns>Protein data.</returns>
    private async Task<ProteinDomain[]> GetProteinDomains(long transcriptId)
    {
        var protein = _dbContext.Set<Unite.Data.Entities.Genome.Protein>()
            .FirstOrDefault(protein => protein.TranscriptId == transcriptId);

        var proteinInfo = await _proteinAnnotationService.FindProtein(protein.StableId);

        return proteinInfo?.Domains?.Select(Convert).ToArray();
    }

    /// <summary>
    /// Retrieves protein mutations for given protein coding transcript.
    /// </summary>
    /// <param name="transcriptId">Transcript identifier</param>
    /// <returns>Array of protein mutations.</returns>
    private async Task<ProteinMutation[]> GetProteinMutations(long transcriptId)
    {   
        var query = new SearchQuery<VariantIndex>()
            .AddPagination(0, 10000)
            .AddFilter(CreateVariantTypeFilter())
            .AddFilter(CreateTranscriptFilter(transcriptId))
            .AddExclusion(variant => variant.Samples);

        var searchResult = await _variantsIndexService.SearchAsync(query);

        var proteinMutations = new List<ProteinMutation>();

        foreach (var variant in searchResult.Rows)
        {
            var affectedFeature = variant.Mutation.AffectedFeatures?
                .Where(affectedFeature => affectedFeature.Transcript.AminoAcidChange != null)
                .FirstOrDefault(affectedFeature => affectedFeature.Transcript.Feature.Id == transcriptId);

            if (affectedFeature != null)
            {
                var consequence = affectedFeature.Consequences
                    .OrderBy(consequence => consequence.Severity)
                    .First();

                var proteinMutation = new ProteinMutation
                {
                    Id = variant.Mutation.Id,
                    Consequence = consequence.Type,
                    Impact = consequence.Impact,
                    AminoAcidChange = affectedFeature.Transcript.AminoAcidChange,
                    NumberOfDonors = variant.NumberOfDonors
                };

                proteinMutations.Add(proteinMutation);
            }
        }

        return proteinMutations.ToArray();
    }


    private static IFilter<VariantIndex> CreateVariantTypeFilter()
    {
        return new NotNullFilter<VariantIndex, Indices.Entities.Basic.Genome.Variants.MutationIndex>
        (
            "Variant.Mutation",
            variant => variant.Mutation
        );
    }

    private static IFilter<VariantIndex> CreateTranscriptFilter(long transcriptId)
    {
        return new EqualityFilter<VariantIndex, long>
        (
            "Transcript.Id",
            variant => variant.Mutation.AffectedFeatures.First().Transcript.Feature.Id,
            transcriptId
        );
    }

    private static ProteinDomain Convert(Annotations.Services.Models.ProteinDomain domain)
    {
        return new ProteinDomain
        {
            Id = domain.Id,
            Symbol = domain.Symbol,
            Description = domain.Description,
            Start = domain.Start,
            End = domain.End
        };
    }
}
