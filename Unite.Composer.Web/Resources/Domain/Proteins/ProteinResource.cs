using Unite.Indices.Entities.Proteins;

namespace Unite.Composer.Web.Resources.Domain.Proteins;

public class ProteinResource : Basic.Omics.ProteinResource
{
    public ProteinStatsResource Stats { get; set; }
    public ProteinDataResource Data { get; set; }


    public ProteinResource(ProteinIndex index) : base(index)
    {
        if (index.Stats != null)
            Stats = new ProteinStatsResource(index.Stats);

        if (index.Data != null)
            Data = new ProteinDataResource(index.Data);
    }
}
