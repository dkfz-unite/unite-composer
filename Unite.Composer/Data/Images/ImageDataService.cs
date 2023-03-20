using Microsoft.EntityFrameworkCore;
using Unite.Composer.Data.Images.Models;
using Unite.Data.Entities.Genome.Analysis;
using Unite.Data.Entities.Images;
using Unite.Data.Entities.Specimens;
using Unite.Data.Services;

namespace Unite.Composer.Data.Images;

public class ImageDataService
{
    private readonly DomainDbContext _dbContext;


    public ImageDataService(DomainDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    public IEnumerable<AnalysedSampleModel> GetAnalysedSamples(int imageId)
    {
        var specimens = LoadSpecimens(imageId);

        foreach (var specimen in specimens)
        {
            var analysedSamples = LoadAnalysedSamples(specimen.Id);

            if (analysedSamples.Any())
            {
                var analyses = analysedSamples
                    .Where(analysedSample => analysedSample.Analysis.TypeId != null)
                    .Select(analysedSample => analysedSample.Analysis.TypeId.Value)
                    .Distinct()
                    .ToArray();

                yield return new AnalysedSampleModel
                {
                    Id = specimen.Id,
                    ReferenceId = specimen.Tissue.ReferenceId,
                    Type = "Tissue",
                    Analyses = analyses
                };
            }
        }
    }


    private Specimen[] LoadSpecimens(int imageId)
    {
        var donorId = _dbContext.Set<Image>().First(image => image.Id == imageId).DonorId;

        var specimens = _dbContext.Set<Specimen>()
            .Include(specimen => specimen.Tissue)
            .Where(specimen => specimen.Tissue != null)
            .Where(specimen => specimen.ParentId == null)
            .Where(specimen => specimen.DonorId == donorId)
            .ToArray();

        return specimens;
    }

    private AnalysedSample[] LoadAnalysedSamples(int specimenId)
    {
        return _dbContext.Set<AnalysedSample>()
            .Include(analysedSample => analysedSample.Analysis)
            .Where(analysedSample => analysedSample.Sample.SpecimenId == specimenId)
            .Where(analysedSample => 
                analysedSample.MutationOccurrences.Count() > 0 ||
                analysedSample.CopyNumberVariantOccurrences.Count() > 0 ||
                analysedSample.StructuralVariantOccurrences.Count() > 0 ||
                analysedSample.GeneExpressions.Count() > 0)
            .ToArray();
    }
}
