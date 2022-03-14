using System.Linq;
using Unite.Composer.Web.Resources.Mutations;
using Unite.Composer.Web.Resources.Specimens;
using Unite.Indices.Entities.Mutations;

namespace Unite.Composer.Web.Resources.Donors
{
    public class DonorMutationResource : MutationBaseResource
    {
        public SpecimenBaseResource[] Specimens { get; }

        public int NumberOfDonors { get; }


        public DonorMutationResource(int donorId, MutationIndex index) : base(index)
        {
            Specimens = index.Donors
                .First(donor => donor.Id == donorId).Specimens?
                .Select(specimen => new SpecimenBaseResource(specimen))
                .ToArray();

            NumberOfDonors = index.NumberOfDonors;
        }
    }
}
