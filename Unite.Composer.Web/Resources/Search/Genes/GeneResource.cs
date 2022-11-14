using Unite.Indices.Entities.Genes;

namespace Unite.Composer.Web.Resources.Search.Genes;

public class GeneResource : Basic.Genome.GeneResource
{
    public int NumberOfDonors { get; }
    public int NumberOfMutations { get; }
    public int NumberOfCopyNumberVariants { get; }
    public int NumberOfStructuralVariants { get; }

    public GeneResource(GeneIndex index) : base(index)
    {
        NumberOfDonors = index.NumberOfDonors;
        NumberOfMutations = index.NumberOfMutations;
        NumberOfCopyNumberVariants = index.NumberOfCopyNumberVariants;
        NumberOfStructuralVariants = index.NumberOfStructuralVariants;
    }
}
