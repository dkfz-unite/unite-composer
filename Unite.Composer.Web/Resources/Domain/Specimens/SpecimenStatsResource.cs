using Unite.Indices.Entities.Specimens;

namespace Unite.Composer.Web.Resources.Domain.Specimens;

public class SpecimenStatsResource
{
    public int Genes { get; set; }
    public int Ssms { get; set; }
    public int Cnvs { get; set; }
    public int Svs { get; set; }

    public SpecimenStatsResource(StatsIndex index)
    {
        Genes = index.Genes;
        Ssms = index.Ssms;
        Cnvs = index.Cnvs;
        Svs = index.Svs;
    }
}
