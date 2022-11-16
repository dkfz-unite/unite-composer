using Unite.Composer.Web.Resources.Search.Basic.Genome;
using Unite.Indices.Entities.Genes;

namespace Unite.Composer.Web.Resources.Search.Images;

public class ImageGeneResource : GeneResource
{
    public int NumberOfDonors { get; }
    public int NumberOfMutations { get; }
    public int NumberOfCopyNumberVariants { get; }
    public int NumberOfStructuralVariants { get; }

    public ImageGeneResource(GeneIndex index, int imageId) : base(index)
    {
        NumberOfDonors = index.NumberOfDonors;

        var specimens = index.Specimens?.Where(specimen => specimen.Images.Any(image => image.Id == imageId));

        NumberOfMutations = specimens?
            .SelectMany(specimen => specimen.Variants)
            .Where(variant => variant.Mutation != null)
            .DistinctBy(variant => variant.Id)
            .Count() ?? 0;

        NumberOfCopyNumberVariants = specimens?
            .SelectMany(specimen => specimen.Variants)
            .Where(variant => variant.CopyNumberVariant != null)
            .DistinctBy(variant => variant.Id)
            .Count() ?? 0;

        NumberOfStructuralVariants = specimens?
            .SelectMany(specimen => specimen.Variants)
            .Where(variant => variant.StructuralVariant != null)
            .DistinctBy(variant => variant.Id)
            .Count() ?? 0;
    }
}
