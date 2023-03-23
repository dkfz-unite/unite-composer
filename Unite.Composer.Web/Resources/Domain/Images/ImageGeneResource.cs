using Unite.Composer.Web.Resources.Domain.Basic.Genome;
using Unite.Indices.Entities.Genes;

namespace Unite.Composer.Web.Resources.Domain.Images;

public class ImageGeneResource : GeneResource
{
    public int NumberOfDonors { get; }
    public int NumberOfMutations { get; }
    public int NumberOfCopyNumberVariants { get; }
    public int NumberOfStructuralVariants { get; }
    public GeneExpressionResource Expression { get; }

    public ImageGeneResource(GeneIndex index, int sampleId) : base(index)
    {
        var sample = index.Samples?.FirstOrDefault(sample => sample.Id == sampleId);

        NumberOfDonors = index.NumberOfDonors;

        NumberOfMutations = sample?.Variants?
            .Where(variant => variant.Mutation != null)
            .DistinctBy(variant => variant.Id)
            .Count() ?? 0;

        NumberOfCopyNumberVariants = sample?.Variants?
            .Where(variant => variant.CopyNumberVariant != null)
            .DistinctBy(variant => variant.Id)
            .Count() ?? 0;

        NumberOfStructuralVariants = sample?.Variants?
            .Where(variant => variant.StructuralVariant != null)
            .DistinctBy(variant => variant.Id)
            .Count() ?? 0;

        if (sample?.Expression != null)
        {
            Expression = new GeneExpressionResource(sample.Expression);
        }
    }
}
