using Unite.Indices.Entities.Variants;

namespace Unite.Composer.Web.Resources.Domain.Variants;

public class VariantResource : Basic.Genome.Variants.VariantResource
{
    public int NumberOfDonors { get; }
    public int NumberOfGenes { get; }
    public int NumberOfSpecimens { get; }

    public VariantResource(VariantIndex index, bool includeAffectedFeatures = false) : base(index, includeAffectedFeatures)
    {
        NumberOfDonors = index.NumberOfDonors;
        NumberOfGenes = index.NumberOfGenes;
    }
}
