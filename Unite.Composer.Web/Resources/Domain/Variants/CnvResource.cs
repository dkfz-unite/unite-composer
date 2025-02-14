using Unite.Indices.Entities.Variants;

namespace Unite.Composer.Web.Resources.Domain.Variants;

public class CnvResource : Basic.Genome.Variants.VariantResource
{
    public string Type { get; set; }
    public bool? Loh { get; set; }
    public bool? Del { get; set; }
    public double? C1Mean { get; set; }
    public double? C2Mean { get; set; }
    public double? TcnMean { get; set; }
    public double? TcnRatio { get; set; }
    public int? C1 { get; set; }
    public int? C2 { get; set; }
    public int? Tcn { get; set; }
    public double? DhMax { get; set; }

    public VariantStatsResource Stats { get; set; }
    public VariantDataResource Data { get; set; }


    public CnvResource(CnvIndex index, bool includeEffects = false) : base(index, includeEffects)
    {
        Type = index.Type;
        Loh = index.Loh;
        Del = index.Del;
        C1Mean = index.C1Mean;
        C2Mean = index.C2Mean;
        TcnMean = index.TcnMean;
        C1 = index.C1;
        C2 = index.C2;
        Tcn = index.Tcn;
        TcnRatio = index.TcnRatio;

        if (index.Stats != null)
            Stats = new VariantStatsResource(index.Stats);

        if (index.Data != null)
            Data = new VariantDataResource(index.Data);
    }
}
