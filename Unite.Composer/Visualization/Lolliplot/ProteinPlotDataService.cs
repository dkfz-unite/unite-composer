using Unite.Composer.Search.Engine;
using Unite.Composer.Search.Engine.Filters;
using Unite.Composer.Search.Engine.Queries;
using Unite.Composer.Visualization.Lolliplot.Annotations.Clients.Ensembl.Configuration.Options;
using Unite.Composer.Visualization.Lolliplot.Annotations.Clients.Uniprot.Configuration.Options;
using Unite.Composer.Visualization.Lolliplot.Annotations.Services;
using Unite.Composer.Visualization.Lolliplot.Data;
using Unite.Indices.Services.Configuration.Options;

using GeneIndex = Unite.Indices.Entities.Genes.GeneIndex;
using MutationIndex = Unite.Indices.Entities.Mutations.MutationIndex;

namespace Unite.Composer.Visualization.Lolliplot;

public class ProteinPlotDataService
{
    private readonly IIndexService<GeneIndex> _genesIndexService;
    private readonly IIndexService<MutationIndex> _mutationsIndexService;
    private readonly ProteinAnnotationService _proteinAnnotationService;


    public ProteinPlotDataService(
        IElasticOptions elasticOptions,
        IEnsemblOptions ensemblOptions,
        IUniprotOptions uniprotOptions)
    {
        _genesIndexService = new GenesIndexService(elasticOptions);
        _mutationsIndexService = new MutationsIndexService(elasticOptions);
        _proteinAnnotationService = new ProteinAnnotationService(ensemblOptions, uniprotOptions);
    }

    /// <summary>
    /// Retreives protein coding transcripts affected by any mutation in given gene.
    /// </summary>
    /// <param name="geneId">Gene identifier</param>
    /// <returns>Array of transcripts</returns>
    public async Task<Transcript[]> GetGeneTranscripts(int geneId)
    {
        var query = new GetQuery<GeneIndex>(geneId.ToString())
            .AddExclusion(gene => gene.Mutations.First().Donors);

        var index = await _genesIndexService.GetAsync(query);

        var transcripts = index.Mutations?
            .Where(mutation => mutation.AffectedTranscripts != null)
            .SelectMany(mutation => mutation.AffectedTranscripts)
            .Where(affectedTranscript => affectedTranscript.Transcript.Protein != null)
            .Where(affectedTranscript => affectedTranscript.AminoAcidChange != null)
            .GroupBy(affectedTranscript => affectedTranscript.Transcript.Id)
            .Select(group => group.First())
            .OrderBy(affectedTranscript => affectedTranscript.Transcript.Symbol)
            .Select(affectedTranscript => new Transcript(affectedTranscript.Transcript))
            .ToArray();

        return transcripts;
    }

    /// <summary>
    /// Retreives protein coding transcripts affected by the mutation.
    /// </summary>
    /// <param name="mutationId">Mutation identifier</param>
    /// <returns>Array of transcripts.</returns>
    public async Task<Transcript[]> GetMutationTranscripts(long mutationId)
    {
        var query = new GetQuery<MutationIndex>(mutationId.ToString())
            .AddExclusion(mutation => mutation.Donors);

        var index = await _mutationsIndexService.GetAsync(query);

        var transcripts = index.AffectedTranscripts?
            .Where(affectedTranscript => affectedTranscript.Transcript.Protein != null)
            .Where(affectedTranscript => affectedTranscript.AminoAcidChange != null)
            .OrderBy(affectedTranscript => affectedTranscript.Transcript.Symbol)
            .Select(affectedTranscript => new Transcript(affectedTranscript.Transcript))
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
        var query = new SearchQuery<MutationIndex>()
            .AddPagination(0, 1)
            .AddFilter(CreateTranscriptFilter(transcriptId))
            .AddExclusion(mutation => mutation.Donors);

        var searchResult = await _mutationsIndexService.SearchAsync(query);

        var protein = searchResult.Rows
            .First().AffectedTranscripts
            .First(affectedTranscript => affectedTranscript.Transcript.Id == transcriptId)
            .Transcript
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
        var query = new SearchQuery<MutationIndex>()
            .AddPagination(0, 10000)
            .AddFilter(CreateTranscriptFilter(transcriptId))
            .AddExclusion(mutation => mutation.Donors);

        var searchResult = await _mutationsIndexService.SearchAsync(query);

        var proteinMutations = new List<ProteinMutation>();

        foreach (var mutation in searchResult.Rows)
        {
            var affectedTranscript = mutation.AffectedTranscripts?
                .Where(affectedTranscript => affectedTranscript.Transcript.Biotype != null)
                .Where(affectedTranscript => affectedTranscript.AminoAcidChange != null)
                .FirstOrDefault(affectedTranscript => affectedTranscript.Transcript.Id == transcriptId);

            if (affectedTranscript != null)
            {
                var consequence = affectedTranscript.Consequences
                    .OrderBy(consequence => consequence.Severity)
                    .First();

                var proteinMutation = new ProteinMutation
                {
                    Id = mutation.Id,
                    Code = mutation.Code,
                    Consequence = consequence.Type,
                    Impact = consequence.Impact,
                    AminoAcidChange = affectedTranscript.AminoAcidChange,
                    NumberOfDonors = mutation.NumberOfDonors
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


    private static IFilter<MutationIndex> CreateTranscriptFilter(long transcriptId)
    {
        return new EqualityFilter<MutationIndex, long>
        (
            "Transcript.Id",
            mutation => mutation.AffectedTranscripts.First().Transcript.Id,
            transcriptId
        );
    }
}
