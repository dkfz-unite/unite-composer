using Unite.Indices.Entities.Basic.Genome.Variants;

namespace Unite.Composer.Web.Resources.Search.Basic.Genome.Variants;

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
    /// Projection of mutation affected transcripts to their consequences.
    /// Has the following format:
    /// - Term - consequence term (e.g. "missense_variant")
    /// - Name - consequence name (e.g. "Missense")
    /// - Impact - consequence impact (e,g. "Moderate")
    /// - Genes - list of genes affected by the consequence
    /// -- Symbol - gene symbol
    /// -- EnsemblId - gene ensemble id
    /// -- Transcripts - list of unique amino acid changes caused by the consiquence in corresponding gene
    /// </summary>
    public dynamic[] TranscriptConsequences { get; }


    public VariantResource(VariantIndex index)
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
            AffectedFeatures = index.GetAffectedFeatures()
                .Select(featureIndex => new AffectedFeatureResource(featureIndex))
                .ToArray();

            TranscriptConsequences = GetTranscriptConsequences(index.GetAffectedFeatures());
        }
    }


    private dynamic[] GetTranscriptConsequences(AffectedFeatureIndex[] affectedFeatures)
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
            .Select(consequenceGroup => new
            {
                Term = consequenceGroup.Value.Type,
                Name = consequenceGroup.Value.Type,
                Impact = consequenceGroup.Value.Impact,
                Genes = consequenceGroup.Elements
                    .GroupBy(
                        affectedTranscript => affectedTranscript.Gene.Symbol,
                        (key, group) => new { Value = group.First().Gene, Elements = group })
                    .OrderBy(geneGroup => geneGroup.Value.Symbol)
                    .Select(geneGroup => new
                    {
                        Id = geneGroup.Value.Id,
                        Symbol = geneGroup.Value.Symbol,
                        EnsemblId = geneGroup.Value.StableId,
                        Transcripts = geneGroup.Elements
                            .Where(affectedTranscript => affectedTranscript.AminoAcidChange != null)
                            .Select(affectedTranscript => affectedTranscript.AminoAcidChange)
                            .OrderBy(aminoAcidChange => aminoAcidChange)
                            .Distinct()
                    })
            })
            .ToArray();
    }
}
