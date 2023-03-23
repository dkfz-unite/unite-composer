using Unite.Indices.Entities.Images;

namespace Unite.Composer.Web.Resources.Domain.Images;

public class ImageResource : Basic.Images.ImageResource
{
    public int DonorId { get; set; }

    public int NumberOfGenes { get; set; }
    public int NumberOfMutations { get; set; }
    public int NumberOfCopyNumberVariants { get; set; }
    public int NumberOfStructuralVariants { get; set; }
    public bool HasGeneExpressions { get; set; }

    public ImageResource(ImageIndex index) : base(index)
    {
        DonorId = index.Donor.Id;

        NumberOfGenes = index.NumberOfGenes;
        NumberOfMutations = index.NumberOfMutations;
        NumberOfCopyNumberVariants = index.NumberOfCopyNumberVariants;
        NumberOfStructuralVariants = index.NumberOfStructuralVariants;
        HasGeneExpressions = index.HasGeneExpressions;
    }
}
