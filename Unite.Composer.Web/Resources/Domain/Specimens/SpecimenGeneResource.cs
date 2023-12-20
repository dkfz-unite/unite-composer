using Unite.Composer.Web.Resources.Domain.Basic.Genome;
using Unite.Data.Entities.Genome.Variants.Enums;
using Unite.Essentials.Extensions;
using Unite.Indices.Entities.Genes;

namespace Unite.Composer.Web.Resources.Domain.Specimens;

public class SpecimenGeneResource : GeneResource
{
    public int NumberOfSsms { get; set; }
    public int NumberOfCnvs { get; set; }
    public int NumberOfSvs { get; set; }
    
    public BulkExpressionStatsResource Reads { get; set; }
    public BulkExpressionStatsResource Tpm { get; set; }
    public BulkExpressionStatsResource Fpkm { get; set; }
    public BulkExpressionResource Expression { get; set;}


    public SpecimenGeneResource(GeneIndex index, int specimenId) : base(index)
    {
        var specimen = index.Specimens?.FirstOrDefault(specimen => specimen.Id == specimenId);
        var specimens = new SpecimenIndex[] { specimen };

        NumberOfSsms = GeneIndex.GetNumberOfVariants(specimens, VariantType.SSM.ToDefinitionString());
        NumberOfCnvs = GeneIndex.GetNumberOfVariants(specimens, VariantType.CNV.ToDefinitionString());
        NumberOfSvs = GeneIndex.GetNumberOfVariants(specimens, VariantType.SV.ToDefinitionString());

        if (index.Reads != null)
            Reads = new BulkExpressionStatsResource(index.Reads);
        
        if (index.Tpm != null)
            Tpm = new BulkExpressionStatsResource(index.Tpm);

        if (index.Fpkm != null)
            Fpkm = new BulkExpressionStatsResource(index.Fpkm);
        
        if (specimen?.Expression != null)
            Expression = new BulkExpressionResource(specimen.Expression);
    }
}
