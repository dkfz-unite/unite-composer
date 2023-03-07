using Unite.Composer.Web.Resources.Search.Basic.Genome;
using Unite.Indices.Entities.Genes;

namespace Unite.Composer.Web.Resources.Search.Donors;

public class DonorGeneResource : GeneResource
{
    public int NumberOfDonors { get; }
    public int NumberOfMutations { get; }
    public int NumberOfCopyNumberVariants { get; }
    public int NumberOfStructuralVariants { get; }
    public GeneSpecimenResource[] Specimens { get; }

    public DonorGeneResource(GeneIndex index, int donorId) : base(index)
    {
        var specimens = index.Specimens?.Where(specimen => specimen.Donor.Id == donorId);

        NumberOfDonors = index.NumberOfDonors;

        NumberOfMutations = specimens?
            .SelectMany(specimen => specimen.Variants)
            .Where(variant => variant.Mutation != null)
            .DistinctBy(variant => variant.Id)
            .Count() ?? 0;

        NumberOfCopyNumberVariants = specimens?
            .SelectMany(specimen => specimen.Variants)
            .Where(variant => variant.CopyNumberVariant != null)
            .DistinctBy(variant => variant.Id)
            .Count() ?? 0;

        NumberOfStructuralVariants = specimens?
            .SelectMany(specimen => specimen.Variants)
            .Where(variant => variant.StructuralVariant != null)
            .DistinctBy(variant => variant.Id)
            .Count() ?? 0;

        Specimens = specimens?
            .Select(specimen => new GeneSpecimenResource(specimen, specimen.Expression))
            .ToArray();
    }
}
