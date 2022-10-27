using Unite.Indices.Entities.Variants;

namespace Unite.Composer.Web.Resources.Search.Variants;

public class VariantResource : Basic.Genome.Variants.VariantResource
{
    public int NumberOfDonors { get; }
    public int NumberOfGenes { get; }
    public int NumberOfSpecimens { get; }

    public VariantResource(VariantIndex index) : base(index)
    {
        NumberOfDonors = index.NumberOfDonors;
        NumberOfGenes = index.NumberOfGenes;
        NumberOfSpecimens = index.NumberOfSpecimens;
    }
}
