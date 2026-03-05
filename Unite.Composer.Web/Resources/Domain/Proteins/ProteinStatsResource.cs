using Unite.Indices.Entities.Proteins;

namespace Unite.Composer.Web.Resources.Domain.Proteins;

public class ProteinStatsResource
{
    public int Donors { get; set; }
    public int Sms { get; set; }
    public int Cnvs { get; set; }
    public int Svs { get; set; }


    public ProteinStatsResource(StatsIndex index)
    {
        Donors = index.Donors;
        Sms = index.Sms;
        Cnvs = index.Cnvs;
        Svs = index.Svs;
    }
}
