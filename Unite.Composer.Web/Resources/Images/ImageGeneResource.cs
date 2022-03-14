using System.Linq;
using Unite.Composer.Web.Resources.Genes;
using Unite.Composer.Web.Resources.Specimens;
using Unite.Indices.Entities.Genes;

namespace Unite.Composer.Web.Resources.Images
{
    public class ImageGeneResource : GeneBaseResource
    {
        /// <summary>
        /// Tumor type tissues
        /// </summary>
        public SpecimenBaseResource[] Specimens { get; }

        /// <summary>
        /// Total number of donors having mutations in this gene in all types of specimens
        /// </summary>
        public int NumberOfDonors { get; }

        /// <summary>
        /// Total number of mutations affected by this gene across all donors and specimens
        /// </summary>
        public int NumberOfMutations { get; }


        public ImageGeneResource(int imageId, GeneIndex index) : base(index)
        {
            Specimens = index.Mutations
                .SelectMany(mutation => mutation.Donors
                    .Where(donor => donor.Images
                        .Any(image => image.Id == imageId)))
                .SelectMany(donor => donor.Specimens)
                .Where(specimen => string.Equals(specimen.Tissue?.Type, "Tumor"))
                .GroupBy(specimen => specimen.Id)
                .Select(group => group.First())
                .Select(specimen => new SpecimenBaseResource(specimen))
                .ToArray();

            NumberOfDonors = index.NumberOfDonors;
            NumberOfMutations = index.NumberOfMutations;
        }
    }
}
