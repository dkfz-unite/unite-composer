using Unite.Indices.Entities.Donors;

namespace Unite.Composer.Web.Resources.Domain.Genes;

public class GeneDonorResource : Basic.Donors.DonorResource
{
    public int NumberOfMutations { get; }
    public int NumberOfCopyNumberVariants { get; }
    public int NumberOfStructuralVariants { get; }

    public GeneDonorResource(DonorIndex index, int geneId) : base(index)
    {
        NumberOfMutations = index.NumberOfMutations;
        NumberOfCopyNumberVariants = index.NumberOfCopyNumberVariants;
        NumberOfStructuralVariants = index.NumberOfStructuralVariants;
    }
}
