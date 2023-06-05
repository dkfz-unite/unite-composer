using Unite.Composer.Web.Resources.Domain.Basic.Genome;
using Unite.Indices.Entities.Genes;

namespace Unite.Composer.Web.Resources.Domain.Genes;

public class GeneResource : Basic.Genome.GeneResource
{
    public int NumberOfDonors { get; }
    public int NumberOfMris { get; set; }
    public int NumberOfCts { get; set; }
    public int NumberOfTissues { get; set; }
    public int NumberOfCells { get; set; }
    public int NumberOfOrganoids { get; set; }
    public int NumberOfXenografts { get; set; }
    public int NumberOfSsms { get; }
    public int NumberOfCnvs { get; }
    public int NumberOfSvs { get; }

    public GeneExpressionStatsResource Reads { get; set; }
    public GeneExpressionStatsResource Tpm { get; set; }
    public GeneExpressionStatsResource Fpkm { get; set; }


    public GeneResource(GeneIndex index) : base(index)
    {
        NumberOfDonors = index.NumberOfDonors;
        NumberOfMris = index.NumberOfMris;
        NumberOfCts = index.NumberOfCts;
        NumberOfTissues = index.NumberOfTissues;
        NumberOfCells = index.NumberOfCells;
        NumberOfOrganoids = index.NumberOfOrganoids;
        NumberOfXenografts = index.NumberOfXenografts;
        NumberOfSsms = index.NumberOfSsms;
        NumberOfCnvs = index.NumberOfCnvs;
        NumberOfSvs = index.NumberOfSvs;

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
    }
}
