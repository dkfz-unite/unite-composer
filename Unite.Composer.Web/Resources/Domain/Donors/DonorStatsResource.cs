using Unite.Indices.Entities.Donors;

namespace Unite.Composer.Web.Resources.Domain.Donors;

public class DonorStatsResource
{
    public int Genes { get; set; }
    public int Sms { get; set; }
    public int Cnvs { get; set; }
    public int Svs { get; set; }

    public DonorStatsResource(StatsIndex index)
    {
        Genes = index.Genes;
        Sms = index.Sms;
        Cnvs = index.Cnvs;
        Svs = index.Svs;
    }
}
