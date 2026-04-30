namespace Unite.Composer.Web.Resources.Domain.Basic;

public class AnalysisDataResource
{
    public bool? Exp { get; set; }
    public bool? ExpSc { get; set; }
    public bool? Sm { get; set; }
    public bool? Cnv { get; set; }
    public bool? Sv { get; set; }
    public bool? Cnvp { get; set; }
    public bool? Meth { get; set; }
    public bool? Prot { get; set; }

    public static AnalysisDataResource CreateFrom(Indices.Entities.Basic.Analysis.SampleDataIndex index)
    {
        if (index == null)
            return null;

        return new AnalysisDataResource
        {
            Exp = index.Exp ? true : null,
            ExpSc = index.ExpSc ? true : null,
            Sm = index.Sm ? true : null,
            Cnv = index.Cnv ? true : null,
            Sv = index.Sv ? true : null,
            Cnvp = index.Cnvp ? true : null,
            Meth = index.Meth ? true : null,
            Prot = index.Prot ? true : null
        };
    }
}
