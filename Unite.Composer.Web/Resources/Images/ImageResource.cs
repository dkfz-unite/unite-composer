using Unite.Indices.Entities.Images;

namespace Unite.Composer.Web.Resources.Images
{
    public class ImageResource : ImageBaseResource
    {
        public int DonorId { get; set; }

        public int NumberOfGenes { get; set; }
        public int NumberOfMutations { get; set; }

        public ImageResource(ImageIndex index) : base(index)
        {
            DonorId = index.Donor.Id;

            NumberOfGenes = index.NumberOfGenes;
            NumberOfMutations = index.NumberOfMutations;
        }
    }
}
