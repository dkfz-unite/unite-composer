using Unite.Indices.Entities.Variants;

namespace Unite.Composer.Web.Resources.Domain.Variants;

public class SmResource : Basic.Omics.Variants.VariantResource
{
    public string Type { get; }
    public string Ref { get; }
    public string Alt { get; }

    public VariantStatsResource Stats { get; set; }
    public VariantDataResource Data { get; set; }


    public SmResource(SmIndex index, bool includeEffects = false) : base(index, includeEffects)
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
