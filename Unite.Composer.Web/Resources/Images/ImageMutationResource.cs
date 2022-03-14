using System.Linq;
using Unite.Composer.Web.Resources.Mutations;
using Unite.Composer.Web.Resources.Specimens;
using Unite.Indices.Entities.Mutations;

namespace Unite.Composer.Web.Resources.Images
{
    public class ImageMutationResource : MutationBaseResource
    {
        /// <summary>
        /// Tumor type tissues
        /// </summary>
        public SpecimenBaseResource[] Specimens { get; }

        /// <summary>
        /// Total number of donors having this mutation in all types of specimens
        /// </summary>
        public int NumberOfDonors { get; }


        public ImageMutationResource(int imageId, MutationIndex index) : base(index)
        {
            Specimens = index.Donors
                .First(donor => donor.Images
                    .Any(image => image.Id == imageId)).Specimens?
                .Where(specimen => string.Equals(specimen.Tissue?.Type, "Tumor"))
                .Select(specimen => new SpecimenBaseResource(specimen))
                .ToArray();

            NumberOfDonors = index.NumberOfDonors;
        }
    }
}
