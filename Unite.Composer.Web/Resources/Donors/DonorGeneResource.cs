using Unite.Composer.Web.Resources.Genes;
using Unite.Composer.Web.Resources.Specimens;
using Unite.Indices.Entities.Genes;

namespace Unite.Composer.Web.Resources.Donors;

public class DonorGeneResource : GeneBaseResource
{
    public SpecimenBaseResource[] Specimens { get; }

    public int NumberOfDonors { get; }
    public int NumberOfMutations { get; }


    public DonorGeneResource(int donorId, GeneIndex index) : base(index)
    {
        Specimens = index.Mutations
            .SelectMany(mutation => mutation.Donors
                .Where(donor => donor.Id == donorId))
            .SelectMany(donor => donor.Specimens)
            .GroupBy(specimen => specimen.Id)
            .Select(group => group.First())
            .Select(specimen => new SpecimenBaseResource(specimen))
            .ToArray();

        NumberOfDonors = index.NumberOfDonors;
        NumberOfMutations = index.NumberOfMutations;
    }
}
