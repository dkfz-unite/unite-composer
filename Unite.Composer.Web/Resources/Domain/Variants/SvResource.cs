using Unite.Essentials.Extensions;
using Unite.Indices.Entities.Variants;

namespace Unite.Composer.Web.Resources.Domain.Variants;

public class SvResource : Basic.Omics.Variants.VariantResource
{
    public string OtherChromosome { get; set; }
    public double? OtherStart { get; set; }
    public double? OtherEnd { get; set; }

    public string Type { get; set; }
    public bool? Inverted { get; set; }
    public string FlankingSequenceFrom { get; set; }
    public string FlankingSequenceTo { get; set; }

    public VariantStatsResource Stats { get; set; }
    public VariantDataResource Data { get; set; }
    public int[] Similars { get; set; }


    public SvResource(SvIndex index, bool includeEffects = false) : base(index, includeEffects)
    {
        OtherChromosome = index.OtherChromosome;
        OtherStart = index.OtherStart;
        OtherEnd = index.OtherEnd;

        Type = index.Type;
        Inverted = index.Inverted;

        if (index.Stats != null)
            Stats = new VariantStatsResource(index.Stats);

        if (index.Data != null)
            Data = new VariantDataResource(index.Data);

        if (index.Similars != null)
            Similars = index.Similars.Select(i => i.Id).ToArrayOrNull();
    }
}
