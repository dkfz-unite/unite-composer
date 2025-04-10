using Unite.Composer.Clients.Ensembl.Configuration.Options;
using Unite.Composer.Visualization.Lolliplot.Annotation;
using Unite.Composer.Visualization.Lolliplot.Data;
using Unite.Data.Context;
using Unite.Indices.Context.Configuration.Options;
using Unite.Indices.Entities.Variants;
using Unite.Indices.Search.Engine;
using Unite.Indices.Search.Engine.Filters;
using Unite.Indices.Search.Engine.Queries;

namespace Unite.Composer.Visualization.Lolliplot;

public class ProteinPlotDataService
{
    private readonly DomainDbContext _dbContext;
    private readonly IIndexService<SmIndex> _variantsIndexService;
    private readonly ProteinAnnotationService _proteinAnnotationService;


    public ProteinPlotDataService(
        DomainDbContext dbContext,
        IElasticOptions elasticOptions,
        IEnsemblOptions ensemblOptions)
    {
        _dbContext = dbContext;
        _variantsIndexService = new SmsIndexService(elasticOptions);
        _proteinAnnotationService = new ProteinAnnotationService(ensemblOptions);
    }


    /// <summary>
    /// Retrieves data required to build protein plot.
    /// </summary>
    /// <param name="transcriptId">Transcript identifier</param>
    /// <returns>Protein plot data.</returns>
    public async Task<ProteinPlotData> LoadData(int transcriptId)
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
    private async Task<ProteinDomain[]> GetProteinDomains(int transcriptId)
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
    private async Task<ProteinMutation[]> GetProteinMutations(int transcriptId)
    {   
        var query = new SearchQuery<SmIndex>()
            .AddPagination(0, 10000)
            .AddFilter(CreateTranscriptFilter(transcriptId))
            .AddExclusion(variant => variant.Specimens);

        var searchResult = await _variantsIndexService.Search(query);

        var proteinMutations = new List<ProteinMutation>();

        foreach (var variant in searchResult.Rows)
        {
            var affectedFeature = variant.AffectedFeatures?
                .Where(affectedFeature => affectedFeature.Transcript.ProteinChange != null)
                .FirstOrDefault(affectedFeature => affectedFeature.Transcript.Feature.Id == transcriptId);

            if (affectedFeature != null)
            {
                var effect = affectedFeature.Effects
                    .OrderBy(effect => effect.Severity)
                    .First();

                var proteinMutation = new ProteinMutation
                {
                    Id = $"{variant.Id}",
                    Effect = effect.Type,
                    Impact = effect.Impact,
                    ProteinChange = affectedFeature.Transcript.ProteinChange,
                    NumberOfDonors = variant.Stats.Donors
                };

                proteinMutations.Add(proteinMutation);
            }
        }

        return proteinMutations.ToArray();
    }


    private static IFilter<SmIndex> CreateTranscriptFilter(int transcriptId)
    {
        return new EqualityFilter<SmIndex, int>
        (
            "SM.Transcript.Id",
            variant => variant.AffectedFeatures.First().Transcript.Feature.Id,
            transcriptId
        );
    }

    private static ProteinDomain Convert(Annotation.Models.ProteinDomain domain)
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
