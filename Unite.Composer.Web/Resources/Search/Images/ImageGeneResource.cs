using Unite.Composer.Web.Resources.Search.Basic.Genome;
using Unite.Indices.Entities.Genes;

namespace Unite.Composer.Web.Resources.Search.Images;

public class ImageGeneResource : GeneResource
{
    public int NumberOfDonors { get; }
    public int NumberOfMutations { get; }
    public int NumberOfCopyNumberVariants { get; }
    public int NumberOfStructuralVariants { get; }
    public GeneExpressionResource Expression { get; }

    public ImageGeneResource(GeneIndex index, int sampleId) : base(index)
    {
        var specimen = index.Specimens?.FirstOrDefault(specimen => specimen.Id == sampleId);

        NumberOfDonors = index.NumberOfDonors;

        NumberOfMutations = specimen?.Variants?
            .Where(variant => variant.Mutation != null)
            .DistinctBy(variant => variant.Id)
            .Count() ?? 0;

        NumberOfCopyNumberVariants = specimen?.Variants?
            .Where(variant => variant.CopyNumberVariant != null)
            .DistinctBy(variant => variant.Id)
            .Count() ?? 0;

        NumberOfStructuralVariants = specimen?.Variants?
            .Where(variant => variant.StructuralVariant != null)
            .DistinctBy(variant => variant.Id)
            .Count() ?? 0;

        if (specimen?.Expression != null)
        {
            Expression = new GeneExpressionResource(specimen.Expression);
        }
    }
}
