using Unite.Indices.Entities.Donors;

namespace Unite.Composer.Web.Resources.Search.Genes;

public class GeneDonorResource : Basic.Donors.DonorResource
{
    public int NumberOfMutations { get; }
    public int NumberOfCopyNumberVariants { get; }
    public int NumberOfStructuralVariants { get; }

    public GeneDonorResource(DonorIndex index, int geneId) : base(index)
    {
        NumberOfMutations = index.Specimens?
            .Where(specimen => specimen.Variants != null)
            .SelectMany(specimen => specimen.Variants)
            .Where(variant => variant.Mutation?.AffectedFeatures != null)
            .Where(variant => variant.Mutation.AffectedFeatures.Any(feature => feature.Gene?.Id == geneId))
            .DistinctBy(variant => variant.Id)
            .Count() ?? 0;

        NumberOfCopyNumberVariants = index.Specimens?
            .Where(specimen => specimen.Variants != null)
            .SelectMany(specimen => specimen.Variants)
            .Where(variant => variant.CopyNumberVariant?.AffectedFeatures != null)
            .Where(variant => variant.CopyNumberVariant.AffectedFeatures.Any(feature => feature.Gene?.Id == geneId))
            .DistinctBy(variant => variant.Id)
            .Count() ?? 0;

        NumberOfStructuralVariants = index.Specimens?
            .Where(specimen => specimen.Variants != null)
            .SelectMany(specimen => specimen.Variants)
            .Where(variant => variant.StructuralVariant?.AffectedFeatures != null)
            .Where(variant => variant.StructuralVariant.AffectedFeatures.Any(feature => feature.Gene?.Id == geneId))
            .DistinctBy(variant => variant.Id)
            .Count() ?? 0;
    }
}
