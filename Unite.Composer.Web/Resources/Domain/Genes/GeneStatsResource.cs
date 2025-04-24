using Unite.Indices.Entities.Genes;

namespace Unite.Composer.Web.Resources.Domain.Genes;

public class GeneStatsResource
{
    public int Donors { get; set; }
    public int Sms { get; set; }
    public int Cnvs { get; set; }
    public int Svs { get; set; }


    public GeneStatsResource(StatsIndex index)
    {
        Donors = index.Donors;
        Sms = index.Sms;
        Cnvs = index.Cnvs;
        Svs = index.Svs;
    }
}
