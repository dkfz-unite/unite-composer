using Unite.Indices.Entities.Variants;

namespace Unite.Composer.Web.Resources.Domain.Variants;

public class VariantStatsResource
{
    public int Donors { get; set; }
    public int Genes { get; set; }


    public VariantStatsResource(StatsIndex index)
    {
        Donors = index.Donors;
        Genes = index.Genes;
    }
}
