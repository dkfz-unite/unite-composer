using Unite.Composer.Web.Resources.Search.Basic.Genome;
using Unite.Indices.Entities.Genes;

namespace Unite.Composer.Web.Resources.Search.Specimens;

public class SpecimenGeneResource : GeneResource
{
    public int NumberOfDonors { get; }
    public int NumberOfMutations { get; }
    public int NumberOfCopyNumberVariants { get; }
    public int NumberOfStructuralVariants { get; }

    public GeneExpressionResource Expression { get; }


    public SpecimenGeneResource(GeneIndex index, int specimenId) : base(index)
    {
        var specimen = index.Specimens?.FirstOrDefault(specimen => specimen.Id == specimenId);

        NumberOfDonors = index.NumberOfDonors;

        NumberOfMutations = specimen?.Variants?
            .Where(variant => variant.Mutation != null)
            .Count() ?? 0;

        NumberOfCopyNumberVariants = specimen?.Variants?
            .Where(variant => variant.CopyNumberVariant != null)
            .Count() ?? 0;

        NumberOfStructuralVariants = specimen?.Variants?
            .Where(variant => variant.StructuralVariant != null)
            .Count() ?? 0;

        if (specimen?.Expression != null)
        {
            Expression = new GeneExpressionResource(specimen.Expression);
        }
    }
}
