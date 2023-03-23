using Unite.Indices.Entities.Basic.Genome.Variants;

namespace Unite.Composer.Web.Resources.Domain.Basic.Genome.Variants;

public class CopyNumberVariantResource
{
    public long Id { get; set; }

    public string Chromosome { get; set; }
    public int Start { get; set; }
    public int End { get; set; }
    public int Length { get; set; }

    public string Type { get; set; }
    public bool? Loh { get; set; }
    public bool? HomoDel { get; set; }
    public double? C1Mean { get; set; }
    public double? C2Mean { get; set; }
    public double? TcnMean { get; set; }
    public double? TcnRatio { get; set; }
    public int? C1 { get; set; }
    public int? C2 { get; set; }
    public int? Tcn { get; set; }
    public double? DhMax { get; set; }


    public CopyNumberVariantResource(CopyNumberVariantIndex index)
    {
        Id = index.Id;

        Chromosome = index.Chromosome;
        Start = index.Start;
        End = index.End;
        Length = index.Length;

        Type = index.Type;
        Loh = index.Loh;
        HomoDel = index.HomoDel;
        C1Mean = index.C1Mean;
        C2Mean = index.C2Mean;
        TcnMean = index.TcnMean;
        C1 = index.C1;
        C2 = index.C2;
        Tcn = index.Tcn;
        TcnRatio = index.TcnRatio;
    }
}
