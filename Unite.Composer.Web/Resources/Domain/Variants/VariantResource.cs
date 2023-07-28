using Unite.Data.Entities.Genome.Variants.Enums;
using Unite.Indices.Entities.Variants;

namespace Unite.Composer.Web.Resources.Domain.Variants;

public class VariantResource : Basic.Genome.Variants.VariantResource
{
    public int NumberOfDonors { get; }
    public int NumberOfMris { get; set; }
    public int NumberOfCts { get; set; }
    public int NumberOfTissues { get; set; }
    public int NumberOfCells { get; set; }
    public int NumberOfOrganoids { get; set; }
    public int NumberOfXenografts { get; set; }
    public int NumberOfGenes { get; set; }
    public VariantDataResource Data { get; set; }

    public VariantResource(VariantIndex index, bool includeAffectedFeatures = false) : base(index, includeAffectedFeatures)
    {
        NumberOfDonors = index.NumberOfDonors;
        NumberOfMris = index.NumberOfMris;
        NumberOfCts = index.NumberOfCts;
        NumberOfTissues = index.NumberOfTissues;
        NumberOfCells = index.NumberOfCells;
        NumberOfOrganoids = index.NumberOfOrganoids;
        NumberOfXenografts = index.NumberOfXenografts;
        NumberOfGenes = index.NumberOfGenes;

        if (index.Data != null)
        {
            var type = index.Ssm != null ? VariantType.SSM : index.Cnv != null ? VariantType.CNV : VariantType.SV;
            Data = new VariantDataResource(index.Data, type);
        }
    }
}
