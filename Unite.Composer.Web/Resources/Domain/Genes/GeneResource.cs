using Unite.Indices.Entities.Genes;

namespace Unite.Composer.Web.Resources.Domain.Genes;

public class GeneResource : Basic.Genome.GeneResource
{
    public int NumberOfDonors { get; }
    public int NumberOfMris { get; set; }
    public int NumberOfCts { get; set; }
    public int NumberOfMaterials { get; set; }
    public int NumberOfLines { get; set; }
    public int NumberOfOrganoids { get; set; }
    public int NumberOfXenografts { get; set; }
    public int NumberOfSsms { get; }
    public int NumberOfCnvs { get; }
    public int NumberOfSvs { get; }

    public GeneDataResource Data { get; set; }


    public GeneResource(GeneIndex index) : base(index)
    {
        NumberOfDonors = index.NumberOfDonors;
        NumberOfMris = index.NumberOfMris;
        NumberOfCts = index.NumberOfCts;
        NumberOfMaterials = index.NumberOfMaterials;
        NumberOfLines = index.NumberOfLines;
        NumberOfOrganoids = index.NumberOfOrganoids;
        NumberOfXenografts = index.NumberOfXenografts;
        NumberOfSsms = index.NumberOfSsms;
        NumberOfCnvs = index.NumberOfCnvs;
        NumberOfSvs = index.NumberOfSvs;

        if (index.Data != null)
            Data = new GeneDataResource(index.Data);
    }
}
