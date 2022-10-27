using Unite.Composer.Web.Resources.Search.Basic.Genome;
using Unite.Composer.Web.Resources.Search.Basic.Specimens;
using Unite.Indices.Entities.Genes;

namespace Unite.Composer.Web.Resources.Search.Donors;

public class DonorGeneResource : GeneResource
{
    public SpecimenResource[] Specimens { get; }

    public int NumberOfDonors { get; }
    public int NumberOfMutations { get; }


    public DonorGeneResource(int donorId, GeneIndex index) : base(index)
    {
        Specimens = index.Specimens
            .Where(specimen => specimen.Donor.Id == donorId)
            .DistinctBy(specimen => specimen.Id)
            .Select(specimen => new SpecimenResource(specimen))
            .ToArray();

        NumberOfDonors = index.NumberOfDonors;
        NumberOfMutations = index.NumberOfMutations;
    }
}
