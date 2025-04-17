using Unite.Indices.Entities.Images;

namespace Unite.Composer.Web.Resources.Domain.Images;

public class ImageStatsResource
{
    public int Genes { get; set; }
    public int Sms { get; set; }
    public int Cnvs { get; set; }
    public int Svs { get; set; }

    public ImageStatsResource(StatsIndex index)
    {
        Genes = index.Genes;
        Sms = index.Sms;
        Cnvs = index.Cnvs;
        Svs = index.Svs;
    }
}
