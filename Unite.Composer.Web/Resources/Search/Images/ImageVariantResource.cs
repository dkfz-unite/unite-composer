using Unite.Composer.Web.Resources.Search.Basic.Genome.Variants;
using Unite.Composer.Web.Resources.Search.Basic.Specimens;
using Unite.Indices.Entities.Variants;

namespace Unite.Composer.Web.Resources.Search.Images;

public class ImageVariantResource : VariantResource
{
    /// <summary>
    /// Tumor type tissues
    /// </summary>
    public SpecimenResource[] Specimens { get; }

    /// <summary>
    /// Total number of donors having this mutation in all types of specimens
    /// </summary>
    public int NumberOfDonors { get; }


    public ImageVariantResource(int imageId, VariantIndex index) : base(index)
    {
        Specimens = index.Specimens
            .Where(specimen => specimen.Images.Any(image => image.Id == imageId))
            .Where(specimen => string.Equals(specimen.Tissue?.Type, "Tumor"))
            .Select(specimen => new SpecimenResource(specimen))
            .ToArray();

        NumberOfDonors = index.NumberOfDonors;
    }
}
