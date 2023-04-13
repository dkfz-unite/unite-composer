using Unite.Indices.Entities.Basic.Genome.Variants;

namespace Unite.Composer.Web.Resources.Domain.Basic.Genome.Variants;

public class VariantResource
{
    public string Id { get; set; }

    public string Chromosome { get; set; }
    public int Start { get; set; }
    public int End { get; set; }
    public int? Length { get; set; }

    public MutationResource Mutation { get; set; }
    public CopyNumberVariantResource CopyNumberVariant { get; set; }
    public StructuralVariantResource StructuralVariant { get; set; }
    public AffectedFeatureResource[] AffectedFeatures { get; set; }

    /// <summary>
    /// Projection of variant affected transcripts to their consequences.
    /// Has the following format:
    /// - Term - consequence term (e.g. "missense_variant")
    /// - Impact - consequence impact (e,g. "Moderate")
    /// - GenesNumber - total number of genes affected by the consequence
    /// - Genes - first 5 genes affected by the consequence
    /// -- Id - gene id
    /// -- Symbol - gene symbol
    /// -- Translations - list of unique amino acid changes caused by the consiquence in corresponding gene
    /// </summary>
    public dynamic[] TranscriptConsequences { get; }


    public VariantResource(VariantIndex index, bool includeAffectedFeatures = false)
    {
        Id = index.Id;

        if (index.Mutation != null)
        {
            Mutation = new MutationResource(index.Mutation);
            Chromosome = index.Mutation.Chromosome;
            Start = index.Mutation.Start;
            End = index.Mutation.End;
            Length = index.Mutation.Length;
        }
        else if (index.CopyNumberVariant != null)
        {
            CopyNumberVariant = new CopyNumberVariantResource(index.CopyNumberVariant);
            Chromosome = index.CopyNumberVariant.Chromosome;
            Start = index.CopyNumberVariant.Start;
            End = index.CopyNumberVariant.End;
            Length = index.CopyNumberVariant.Length;
        }
        else if (index.StructuralVariant != null)
        {
            StructuralVariant = new StructuralVariantResource(index.StructuralVariant);
            Chromosome = index.StructuralVariant.Chromosome;
            Start = index.StructuralVariant.Start;
            End = index.StructuralVariant.End;
            Length = index.StructuralVariant.Length;
        }

        if (index.GetAffectedFeatures()?.Any() == true)
        {
            if (includeAffectedFeatures)
            {
                AffectedFeatures = index.GetAffectedFeatures().Select(featureIndex => new AffectedFeatureResource(featureIndex)).ToArray();
            }
            else
            {
                TranscriptConsequences = GetTranscriptConsequences(index.GetAffectedFeatures()).ToArray();
            }
        }
    }

    

    private IEnumerable<dynamic> GetTranscriptConsequences(AffectedFeatureIndex[] affectedFeatures)
    {
        return affectedFeatures
            .Where(affectedFeature => affectedFeature.Gene != null)
            .Where(affectedFeature => affectedFeature.Transcript != null)
            .Select(affectedFeature => new
            {
                AminoAcidChange = affectedFeature.Transcript.AminoAcidChange,
                CodonChange = affectedFeature.Transcript.CodonChange,
                Gene = affectedFeature.Gene,
                Transcript = affectedFeature.Transcript.Feature,
                Consequence = affectedFeature.Consequences.OrderBy(consequence => consequence.Severity).First()
            })
            .OrderBy(affectedTranscript => affectedTranscript.Consequence.Severity)
            .GroupBy(
                affectedTranscript => affectedTranscript.Consequence.Type,
                (key, group) => new { Value = group.First().Consequence, Elements = group })
            .Select(consequenceGroup => {
                var term = consequenceGroup.Value.Type;
                var impact = consequenceGroup.Value.Impact;
                var genes = consequenceGroup.Elements
                    .GroupBy(
                        affectedTranscript => affectedTranscript.Gene.Symbol,
                        (key, group) => new { Value = group.First().Gene, Elements = group })
                    .OrderBy(geneGroup => geneGroup.Value.Start)
                    .Select(geneGroup => {
                        var id = geneGroup.Value.Id;
                        var symbol = geneGroup.Value.Symbol ?? geneGroup.Value.StableId;
                        var translations = geneGroup.Elements
                            .Where(affectedTranscript => affectedTranscript.AminoAcidChange != null)
                            .OrderBy(affectedTranscript => affectedTranscript.Transcript.Start)
                            .Select(affectedTranscript => affectedTranscript.AminoAcidChange)
                            .Distinct();
                        return new GeneGroup(id, symbol, translations);
                    });
                var genesNumber = genes.Count();
                return new ConsequenceGroup(term, impact, genesNumber, genes.Take(5));
            });
    }

    private record GeneGroup(int Id, string Symbol, IEnumerable<string> Translations);
    private record ConsequenceGroup(string Term, string Impact, int GenesNumber, IEnumerable<GeneGroup> Genes);
}
