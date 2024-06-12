using Unite.Composer.Web.Resources.Domain.Basic.Genome;
using Unite.Data.Entities.Genome.Analysis.Dna.Enums;
using Unite.Essentials.Extensions;
using Unite.Indices.Entities.Genes;

namespace Unite.Composer.Web.Resources.Domain.Donors;

public class DonorGeneResource : GeneResource
{
    public int NumberOfSsms { get; set; }
    public int NumberOfCnvs { get; set; }
    public int NumberOfSvs { get; set; }

    
    public DonorGeneResource(GeneIndex index, int specimenId) : base(index)
    {
        var specimen = index.Specimens?.FirstOrDefault(specimen => specimen.Id == specimenId);
        
        NumberOfSsms = GeneIndex.GetNumberOfVariants([specimen], VariantType.SSM.ToDefinitionString());
        NumberOfCnvs = GeneIndex.GetNumberOfVariants([specimen], VariantType.CNV.ToDefinitionString());
        NumberOfSvs = GeneIndex.GetNumberOfVariants([specimen], VariantType.SV.ToDefinitionString());
    }
}
