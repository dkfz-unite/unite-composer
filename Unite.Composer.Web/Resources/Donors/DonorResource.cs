using Unite.Indices.Entities.Donors;

namespace Unite.Composer.Web.Resources.Donors
{
    public class DonorResource : DonorBaseResource
    {
        public int NumberOfSpecimens { get; set; }
        public int NumberOfMutations { get; set; }
        public int NumberOfGenes { get; set; }


        public DonorResource(DonorIndex index) : base(index)
        {
            NumberOfSpecimens = index.NumberOfSpecimens;
            NumberOfMutations = index.NumberOfMutations;
            NumberOfGenes = index.NumberOfGenes;
        }
    }
}
