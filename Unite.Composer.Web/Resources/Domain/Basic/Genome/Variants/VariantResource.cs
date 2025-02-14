using Unite.Essentials.Extensions;
// using Unite.Indices.Entities.Basic.Genome.Dna;
using Unite.Indices.Entities.Basic.Genome.Variants;

namespace Unite.Composer.Web.Resources.Domain.Basic.Genome.Variants;

public abstract class VariantResource
{
    public int Id { get; set; }

    public string Chromosome { get; set; }
    public int Start { get; set; }
    public int End { get; set; }
    public int? Length { get; set; }

    // public SsmResource Ssm { get; set; }
    // public CnvResource Cnv { get; set; }
    // public SvResource Sv { get; set; }
    
    public AffectedFeatureResource[] AffectedFeatures { get; set; }

    /// <summary>
    /// Projection of variant affected transcripts to their effects.
    /// Has the following format:
    /// - Term - effect term (e.g. "missense_variant")
    /// - Impact - effect impact (e,g. "Moderate")
    /// - GenesNumber - total number of genes affected by the effect
    /// - Genes - first 5 genes affected by the effect
    /// -- Id - gene id
    /// -- Symbol - gene symbol
    /// -- Translations - list of unique amino acid changes caused by the consiquence in corresponding gene
    /// </summary>
    public dynamic[] TranscriptEffects { get; }


    public VariantResource(VariantBaseIndex index, bool includeAffectedFeatures = false)
    {
        Id = index.Id;

        Chromosome = index.Chromosome;
        Start = index.Start;
        End = index.End;
        Length = index.Length;

        // if (index.Ssm != null)
        // {
        //     Ssm = new SsmResource(index.Ssm);
        //     Chromosome = index.Ssm.Chromosome;
        //     Start = index.Ssm.Start;
        //     End = index.Ssm.End;
        //     Length = index.Ssm.Length;
        // }
        // else if (index.Cnv != null)
        // {
        //     Cnv = new CnvResource(index.Cnv);
        //     Chromosome = index.Cnv.Chromosome;
        //     Start = index.Cnv.Start;
        //     End = index.Cnv.End;
        //     Length = index.Cnv.Length;
        // }
        // else if (index.Sv != null)
        // {
        //     Sv = new SvResource(index.Sv);
        //     Chromosome = index.Sv.Chromosome;
        //     Start = index.Sv.Start;
        //     End = index.Sv.End;
        //     Length = index.Sv.Length;
        // }

        
        var affectedFeatures = index.AffectedFeatures;

        if (includeAffectedFeatures && affectedFeatures.IsNotEmpty())
        {
            AffectedFeatures = affectedFeatures
                .Select(featureIndex => new AffectedFeatureResource(featureIndex))
                .ToArray();
        }
        else if (!includeAffectedFeatures && affectedFeatures.IsNotEmpty())
        {
            TranscriptEffects =
                GetTranscriptEffects(affectedFeatures)
                .ToArray();
        }
    }

    

    private static IEnumerable<dynamic> GetTranscriptEffects(AffectedFeatureIndex[] affectedFeatures)
    {
        return affectedFeatures
            .Where(affectedFeature => affectedFeature.Gene != null)
            .Where(affectedFeature => affectedFeature.Transcript != null)
            .Select(affectedFeature => new
            {
                ProteinChange = affectedFeature.Transcript.ProteinChange,
                CodonChange = affectedFeature.Transcript.CodonChange,
                Gene = affectedFeature.Gene,
                Transcript = affectedFeature.Transcript.Feature,
                Effect = affectedFeature.Effects.OrderBy(effect => effect.Severity).First()
            })
            .OrderBy(affectedTranscript => affectedTranscript.Effect.Severity)
            .GroupBy(
                affectedTranscript => affectedTranscript.Effect.Type,
                (key, group) => new { Value = group.First().Effect, Elements = group })
            .Select(effectGroup => {
                var term = effectGroup.Value.Type;
                var impact = effectGroup.Value.Impact;
                var genes = effectGroup.Elements
                    .GroupBy(
                        affectedTranscript => affectedTranscript.Gene.Symbol,
                        (key, group) => new { Value = group.First().Gene, Elements = group })
                    .OrderBy(geneGroup => geneGroup.Value.Start)
                    .Select(geneGroup => {
                        var id = geneGroup.Value.Id;
                        var symbol = geneGroup.Value.Symbol ?? geneGroup.Value.StableId;
                        var translations = geneGroup.Elements
                            .Where(affectedTranscript => affectedTranscript.ProteinChange != null)
                            .OrderBy(affectedTranscript => affectedTranscript.Transcript.Start)
                            .Select(affectedTranscript => affectedTranscript.ProteinChange)
                            .Distinct();
                        return new GeneGroup(id, symbol, translations);
                    });
                var genesNumber = genes.Count();
                return new EffectGroup(term, impact, genesNumber, genes.Take(5));
            });
    }

    private record GeneGroup(int Id, string Symbol, IEnumerable<string> Translations);
    private record EffectGroup(string Term, string Impact, int GenesNumber, IEnumerable<GeneGroup> Genes);
}
