namespace Unite.Composer.Web.Resources.Domain.Basic;

public class AnalysisResource
{
    public string Type { get; set; }
    public int? Day { get; set; }

    public AnalysisDataResource Data { get; set; }
    public FileResource[] Files { get; set; }


    public static AnalysisResource CreateFrom(Indices.Entities.Basic.Analysis.SampleIndex index)
    {
        if (index == null)
            return null;

        return new AnalysisResource
        {
            Type = index.AnalysisType,
            Day = index.AnalysisDay
        };
    }
}
