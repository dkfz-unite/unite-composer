using Unite.Indices.Entities.Genes;

namespace Unite.Composer.Web.Resources.Genes
{
    public class GeneResource : GeneBaseResource
    {
        public int NumberOfDonors { get; }
        public int NumberOfMutations { get; }

        public GeneResource(GeneIndex index) : base(index)
        {
            NumberOfDonors = index.NumberOfDonors;
            NumberOfMutations = index.NumberOfMutations;
        }
    }
}
