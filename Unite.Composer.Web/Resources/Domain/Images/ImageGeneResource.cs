using Unite.Composer.Web.Resources.Domain.Basic.Genome;
using Unite.Indices.Entities.Genes;

namespace Unite.Composer.Web.Resources.Domain.Images;

public class ImageGeneResource : GeneResource
{
    public int NumberOfSsms { get; set; }
    public int NumberOfCnvs { get; set; }
    public int NumberOfSvs { get; set; }
    public GeneExpressionStatsResource Reads { get; set; }
    public GeneExpressionStatsResource Tpm { get; set; }
    public GeneExpressionStatsResource Fpkm { get; set; }
    public GeneExpressionResource Expression { get; set; }

    public ImageGeneResource(GeneIndex index, int sampleId) : base(index)
    {
        var sample = index.Samples?.FirstOrDefault(sample => sample.Id == sampleId);
        var samples = new SampleIndex[] { sample };

        NumberOfSsms = GeneIndex.GetNumberOfSsms(samples);
        NumberOfCnvs = GeneIndex.GetNumberOfCnvs(samples);
        NumberOfSvs = GeneIndex.GetNumberOfSvs(samples);

        if (index.Reads != null)
        {
            Reads = new GeneExpressionStatsResource(index.Reads);
        }
        
        if (index.Tpm != null)
        {
            Tpm = new GeneExpressionStatsResource(index.Tpm);
        }

        if (index.Fpkm != null)
        {
            Fpkm = new GeneExpressionStatsResource(index.Fpkm);
        }

        if (sample?.Expression != null)
        {
            Expression = new GeneExpressionResource(sample.Expression);
        }
    }
}
