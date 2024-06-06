using Unite.Composer.Web.Resources.Domain.Basic.Genome;
using Unite.Data.Entities.Genome.Analysis.Dna.Enums;
using Unite.Essentials.Extensions;
using Unite.Indices.Entities.Genes;

namespace Unite.Composer.Web.Resources.Domain.Images;

public class ImageGeneResource : GeneResource
{
    public int NumberOfSsms { get; set; }
    public int NumberOfCnvs { get; set; }
    public int NumberOfSvs { get; set; }

    public ImageGeneResource(GeneIndex index, int sampleId) : base(index)
    {
        var specimen = index.Specimens?.FirstOrDefault(specimen => specimen.Id == sampleId);
        var samples = new SpecimenIndex[] { specimen };

        NumberOfSsms = GeneIndex.GetNumberOfVariants(samples, VariantType.SSM.ToDefinitionString());
        NumberOfCnvs = GeneIndex.GetNumberOfVariants(samples, VariantType.CNV.ToDefinitionString());
        NumberOfSvs = GeneIndex.GetNumberOfVariants(samples, VariantType.SV.ToDefinitionString());
    }
}
