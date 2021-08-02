using Unite.Indices.Entities.Mutations;

namespace Unite.Composer.Web.Resources.Mutations
{
    public class MutationResource : MutationBaseResource
    {
        public int NumberOfDonors { get; }
        public int NumberOfSpecimens { get; }

        public MutationResource(MutationIndex index) : base(index)
        {
            NumberOfDonors = index.NumberOfDonors;
            NumberOfSpecimens = index.NumberOfSpecimens;
        }
    }
}
