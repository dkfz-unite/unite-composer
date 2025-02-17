using Unite.Indices.Entities.Images;

namespace Unite.Composer.Web.Resources.Domain.Images;

public class ImageStatsResource
{
    public int Genes { get; set; }
    public int Ssms { get; set; }
    public int Cnvs { get; set; }
    public int Svs { get; set; }

    public ImageStatsResource(StatsIndex index)
    {
        Genes = index.Genes;
        Ssms = index.Ssms;
        Cnvs = index.Cnvs;
        Svs = index.Svs;
    }
}
