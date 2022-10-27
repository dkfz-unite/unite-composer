using Unite.Composer.Web.Resources.Search.Basic.Genome.Variants;
using Unite.Composer.Web.Resources.Search.Basic.Specimens;
using Unite.Indices.Entities.Variants;

namespace Unite.Composer.Web.Resources.Search.Donors;

public class DonorVariantResource : VariantResource
{
    public SpecimenResource[] Specimens { get; }

    public int NumberOfDonors { get; }
    public int NumberOfGenes { get; }


    public DonorVariantResource(int donorId, VariantIndex index) : base(index)
    {
        Specimens = index.Specimens
            .Where(specimen => specimen.Donor.Id == donorId)
            .DistinctBy(specimen => specimen.Id)
            .Select(specimen => new SpecimenResource(specimen))
            .ToArray();

        NumberOfDonors = index.NumberOfDonors;
        NumberOfGenes = index.NumberOfGenes;
    }
}
