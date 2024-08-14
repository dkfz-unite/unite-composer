namespace Unite.Composer.Web.Resources.Domain.Basic;

public class AnalysisDataResource
{
    public bool? Ssm { get; set; }
    public bool? Cnv { get; set; }
    public bool? Sv { get; set; }
    public bool? Exp { get; set; }


    public static AnalysisDataResource CreateFrom(Indices.Entities.Basic.Analysis.SampleDataIndex index)
    {
        if (index == null)
            return null;

        return new AnalysisDataResource
        {
            Ssm = index.Ssm ? true : null,
            Cnv = index.Cnv ? true : null,
            Sv = index.Sv ? true : null,
            Exp = index.Exp ? true : null
        };
    }
}
