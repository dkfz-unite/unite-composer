using Unite.Composer.Search.Engine;
using Unite.Composer.Search.Engine.Filters;
using Unite.Composer.Search.Engine.Queries;
using Unite.Composer.Visualization.Lolliplot.Annotations.Clients.Ensembl.Configuration.Options;
using Unite.Composer.Visualization.Lolliplot.Annotations.Clients.Uniprot.Configuration.Options;
using Unite.Composer.Visualization.Lolliplot.Annotations.Services;
using Unite.Composer.Visualization.Lolliplot.Data;
using Unite.Indices.Services.Configuration.Options;

using GeneIndex = Unite.Indices.Entities.Genes.GeneIndex;
using VariantIndex = Unite.Indices.Entities.Variants.VariantIndex;

namespace Unite.Composer.Visualization.Lolliplot;

public class ProteinPlotDataService
{
    private readonly IIndexService<GeneIndex> _genesIndexService;
    private readonly IIndexService<VariantIndex> _variantsIndexService;
    private readonly ProteinAnnotationService _proteinAnnotationService;


    public ProteinPlotDataService(
        IElasticOptions elasticOptions,
        IEnsemblOptions ensemblOptions,
        IUniprotOptions uniprotOptions)
    {
        _genesIndexService = new GenesIndexService(elasticOptions);
        _variantsIndexService = new VariantsIndexService(elasticOptions);
        _proteinAnnotationService = new ProteinAnnotationService(ensemblOptions, uniprotOptions);
    }

    /// <summary>
    /// Retreives protein coding transcripts affected by any variant in given gene.
    /// </summary>
    /// <param name="geneId">Gene identifier</param>
    /// <returns>Array of transcripts</returns>
    public async Task<Transcript[]> GetGeneTranscripts(int geneId)
    {
        var query = new SearchQuery<VariantIndex>()
            .AddPagination(0, 10000)
            .AddFilter(new NotNullFilter<VariantIndex, object>("Variant.IsMutation", variant => variant.Mutation))
            .AddFilter(new EqualityFilter<VariantIndex, int>("Gene.Id", variant => variant.Mutation.AffectedFeatures.First().Gene.Id, geneId))
            .AddExclusion(variant => variant.Specimens);

        var results = await _variantsIndexService.SearchAsync(query);

        var indices = results.Rows;

        var transcripts = indices.SelectMany(variant => variant.Mutation.AffectedFeatures)?
            .Where(affectedFeature => affectedFeature.Transcript != null)
            .Where(affectedFeature => affectedFeature.Transcript.AminoAcidChange != null)
            .Where(affectedFeature => affectedFeature.Transcript.Feature.Protein != null)
            .Select(affectedFeature => affectedFeature.Transcript.Feature)
            .OrderBy(transcript => transcript.Symbol)
            .DistinctBy(transcript => transcript.Id)
            .Select(transcript => new Transcript(transcript))
            .ToArray();

        return transcripts;
    }

    /// <summary>
    /// Retreives protein coding transcripts affected by the variant.
    /// </summary>
    /// <param name="variantId">Variant identifier</param>
    /// <returns>Array of transcripts.</returns>
    public async Task<Transcript[]> GetMutationTranscripts(string variantId)
    {
        var query = new GetQuery<VariantIndex>(variantId)
            .AddExclusion(variant => variant.Specimens);

        var index = await _variantsIndexService.GetAsync(query);

        var transcripts = index.Mutation.AffectedFeatures?
            .Where(affectedFeature => affectedFeature.Transcript != null)
            .Where(affectedFeature => affectedFeature.Transcript.AminoAcidChange != null)
            .Where(affectedFeature => affectedFeature.Transcript.Feature.Protein != null)
            .OrderBy(affectedFeature => affectedFeature.Transcript.Feature.Symbol)
            .Select(affectedFeature => new Transcript(affectedFeature.Transcript.Feature))
            .ToArray();

        return transcripts;
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
        var query = new SearchQuery<VariantIndex>()
            .AddPagination(0, 1)
            .AddFilter(CreateVariantTypeFilter())
            .AddFilter(CreateTranscriptFilter(transcriptId))
            .AddExclusion(variant => variant.Specimens);

        var searchResult = await _variantsIndexService.SearchAsync(query);

        var protein = searchResult.Rows
            .First().Mutation.AffectedFeatures
            .First(affectedFeature => affectedFeature.Transcript.Feature.Id == transcriptId)
            .Transcript
            .Feature
            .Protein;

        var proteinInfo = await _proteinAnnotationService.FindProtein(protein.EnsemblId);

        var proteinDomains = proteinInfo?.Domains?
            .Select(domain => new ProteinDomain
            {
                Id = domain.Id,
                Symbol = domain.Symbol,
                Description = domain.Description,
                Start = domain.Start,
                End = domain.End
            });

        return proteinDomains?.ToArray();
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
            .AddExclusion(variant => variant.Specimens);

        var searchResult = await _variantsIndexService.SearchAsync(query);

        var proteinMutations = new List<ProteinMutation>();

        foreach (var variant in searchResult.Rows)
        {
            var affectedFeature = variant.Mutation.AffectedFeatures?
                .Where(affectedFeature => affectedFeature.Transcript.Feature.Biotype != null)
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
            else
            {
                continue;
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
}
