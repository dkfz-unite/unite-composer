using Unite.Indices.Entities.Basic.Genome.Transcriptomics;

namespace Unite.Composer.Web.Resources.Domain.Basic.Genome;

public class GeneExpressionStatsResource
{
    public double Min { get; set; }
    public double Max { get; set; }
    public double Mean { get; set; }
    public double Median { get; set; }


    public GeneExpressionStatsResource(GeneExpressionStatsIndex index)
    {
        Min = index.Min;
        Max = index.Max;
        Mean = index.Mean;
        Median = index.Median;
    }
}