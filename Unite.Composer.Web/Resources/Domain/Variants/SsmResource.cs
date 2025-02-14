using Unite.Indices.Entities.Variants;

namespace Unite.Composer.Web.Resources.Domain.Variants;

public class SsmResource : Basic.Genome.Variants.VariantResource
{
    public string Type { get; }
    public string Ref { get; }
    public string Alt { get; }

    public VariantStatsResource Stats { get; set; }
    public VariantDataResource Data { get; set; }


    public SsmResource(SsmIndex index, bool includeEffects = false) : base(index, includeEffects)
    {
        Type = index.Type;
        Ref = index.Ref;
        Alt = index.Alt;

        if (index.Stats != null)
            Stats = new VariantStatsResource(index.Stats);

        if (index.Data != null)
            Data = new VariantDataResource(index.Data);
    }
}
