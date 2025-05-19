using Unite.Indices.Entities.Genes;

namespace Unite.Composer.Web.Resources.Domain.Genes;

public class GeneResource : Basic.Omics.GeneResource
{
    public GeneStatsResource Stats { get; set; }
    public GeneDataResource Data { get; set; }


    public GeneResource(GeneIndex index) : base(index)
    {
        if (index.Stats != null)
            Stats = new GeneStatsResource(index.Stats);

        if (index.Data != null)
            Data = new GeneDataResource(index.Data);
    }
}
