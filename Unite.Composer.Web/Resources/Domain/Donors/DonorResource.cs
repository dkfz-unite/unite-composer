using Unite.Composer.Data.Donors.Models;
using Unite.Indices.Entities.Donors;

namespace Unite.Composer.Web.Resources.Domain.Donors;

public class DonorResource : Basic.Donors.DonorResource
{
    public int NumberOfImages { get; set; }
    public int NumberOfSpecimens { get; set; }
    public int NumberOfGenes { get; set; }
    public int NumberOfMutations { get; set; }
    public int NumberOfCopyNumberVariants { get; set; }
    public int NumberOfStructuralVariants { get; set; }
    public bool HasGeneExpressions { get; set; }
    public AnalysedSampleModel[] AnalysedSamples { get; set; }


    public DonorResource(DonorIndex index) : base(index)
    {
        NumberOfImages = index.NumberOfImages;
        NumberOfSpecimens = index.NumberOfSpecimens;
        NumberOfGenes = index.NumberOfGenes;
        NumberOfMutations = index.NumberOfMutations;
        NumberOfCopyNumberVariants = index.NumberOfCopyNumberVariants;
        NumberOfStructuralVariants = index.NumberOfStructuralVariants;
        HasGeneExpressions = index.HasGeneExpressions;
    }

    public DonorResource(DonorIndex index, AnalysedSampleModel[] samples) : this(index)
    {
        AnalysedSamples = samples;
    }
}
