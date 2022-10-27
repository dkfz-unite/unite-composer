using Unite.Composer.Web.Resources.Search.Basic.Genome;
using Unite.Composer.Web.Resources.Search.Basic.Specimens;
using Unite.Indices.Entities.Genes;

namespace Unite.Composer.Web.Resources.Search.Images;

public class ImageGeneResource : GeneResource
{
    /// <summary>
    /// Tumor type tissues
    /// </summary>
    public SpecimenResource[] Specimens { get; }

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
        Specimens = index.Specimens
            .Where(specimen => specimen.Images.Any(image => image.Id == imageId))
            .Where(specimen => string.Equals(specimen.Tissue?.Type, "Tumor"))
            .DistinctBy(specimen => specimen.Id)
            .Select(specimen => new SpecimenResource(specimen))
            .ToArray();

        NumberOfDonors = index.NumberOfDonors;
        NumberOfMutations = index.NumberOfMutations;
    }
}
