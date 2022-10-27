using Unite.Indices.Entities.Donors;

namespace Unite.Composer.Web.Resources.Search.Donors;

public class DonorResource : Basic.Donors.DonorResource
{
    public int NumberOfImages { get; set; }
    public int NumberOfSpecimens { get; set; }
    public int NumberOfGenes { get; set; }
    public int NumberOfMutations { get; set; }
    public int NumberOfCopyNumberVariants { get; set; }
    public int NumberOfStructuralVariants { get; set; }


    public DonorResource(DonorIndex index) : base(index)
    {
        NumberOfImages = index.NumberOfImages;
        NumberOfSpecimens = index.NumberOfSpecimens;
        NumberOfGenes = index.NumberOfGenes;
        NumberOfMutations = index.NumberOfMutations;
        NumberOfCopyNumberVariants = index.NumberOfCopyNumberVariants;
        NumberOfStructuralVariants = index.NumberOfStructuralVariants;
    }
}
