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

    public BulkExpressionStatsResource Reads { get; set; }
    public BulkExpressionStatsResource Tpm { get; set; }
    public BulkExpressionStatsResource Fpkm { get; set; }
    public GeneDataResource Data { get; set; }


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
            Reads = new BulkExpressionStatsResource(index.Reads);

        if (index.Tpm != null)
            Tpm = new BulkExpressionStatsResource(index.Tpm);

        if (index.Fpkm != null)
            Fpkm = new BulkExpressionStatsResource(index.Fpkm);

        if (index.Data != null)
            Data = new GeneDataResource(index.Data);
    }
}
