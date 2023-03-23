using Microsoft.EntityFrameworkCore;
using Unite.Composer.Data.Genome.Models.Analysis;
using Unite.Data.Services;

namespace Unite.Composer.Data.Genome;

public class SampleDataService
{
    private readonly DomainDbContext _dbContext;


    public SampleDataService(DomainDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    public AnalysedSample[] GetDonorSamples(int donorId)
    {
        var specimens = LoadDonorSpecimens(donorId);

        return GetAnalysedSamples(specimens).ToArray();
    }

    public AnalysedSample[] GetImageSamples(int imageId)
    {
        var specimens = LoadImageSpecimens(imageId);

        return GetAnalysedSamples(specimens).ToArray();
    }

    public AnalysedSample[] GetSpecimenSamples(int specimenId)
    {
        var specimens = LoadSpecimens(specimenId);

        return GetAnalysedSamples(specimens).ToArray();
    }


    private IEnumerable<AnalysedSample> GetAnalysedSamples(Unite.Data.Entities.Specimens.Specimen[] specimens)
    {
        foreach (var specimen in specimens)
        {
            var analysedSamples = LoadAnalysedSamples(specimen.Id)
                .GroupBy(analysedSample => analysedSample.SampleId)
                .ToArray();

            foreach (var analysedSample in analysedSamples)
            {
                var sample = analysedSample.First().Sample;

                var ploidy = analysedSample.FirstOrDefault(analysedSample => analysedSample.Ploidy != null)?.Ploidy;

                var purity = analysedSample.FirstOrDefault(analysedSample => analysedSample.Purity != null)?.Purity;

                var analyses = analysedSample
                    .Where(analysedSample => analysedSample.Analysis.TypeId != null)
                    .Select(analysedSample => analysedSample.Analysis.TypeId.Value)
                    .Distinct()
                    .ToArray();

                yield return new AnalysedSample
                {
                    Id = sample.Id,
                    ReferenceId = sample.ReferenceId,
                    Ploidy = ploidy,
                    Purity = purity,
                    Specimen = new() { Id = specimen.Id, ReferenceId = GetSpecimenReferenceId(specimen), Type = GetSpecimenType(specimen) },
                    Analyses = analyses
                };
            }
        }
    }

    private Unite.Data.Entities.Specimens.Specimen[] LoadDonorSpecimens(int donorId)
    {
        var specimens = _dbContext.Set<Unite.Data.Entities.Specimens.Specimen>()
            .Include(specimen => specimen.Tissue)
            .Include(specimen => specimen.CellLine)
            .Include(specimen => specimen.Organoid)
            .Include(specimen => specimen.Xenograft)
            .Where(specimen => specimen.ParentId == null)
            .Where(specimen => specimen.DonorId == donorId)
            .ToArray();

        return specimens;
    }

    private Unite.Data.Entities.Specimens.Specimen[] LoadImageSpecimens(int imageId)
    {
        var donorId = _dbContext.Set<Unite.Data.Entities.Images.Image>()
            .First(image => image.Id == imageId).DonorId;

        var specimens = _dbContext.Set<Unite.Data.Entities.Specimens.Specimen>()
            .Include(specimen => specimen.Tissue)
            .Where(specimen => specimen.Tissue != null)
            .Where(specimen => specimen.ParentId == null)
            .Where(specimen => specimen.DonorId == donorId)
            .ToArray();

        return specimens;
    }

    private Unite.Data.Entities.Specimens.Specimen[] LoadSpecimens(int specimenId)
    {
        var specimens = _dbContext.Set<Unite.Data.Entities.Specimens.Specimen>()
            .Include(specimen => specimen.Tissue)
            .Where(specimen => specimen.Tissue != null)
            .Where(specimen => specimen.ParentId == null)
            .Where(specimen => specimen.Id == specimenId)
            .ToArray();

        return specimens;
    }

    private Unite.Data.Entities.Genome.Analysis.AnalysedSample[] LoadAnalysedSamples(int specimenId)
    {
        return _dbContext.Set<Unite.Data.Entities.Genome.Analysis.AnalysedSample>()
            .Include(analysedSample => analysedSample.Sample)
            .Include(analysedSample => analysedSample.Analysis)
            .Where(analysedSample => analysedSample.Sample.SpecimenId == specimenId)
            .Where(analysedSample => 
                analysedSample.MutationOccurrences.Count() > 0 ||
                analysedSample.CopyNumberVariantOccurrences.Count() > 0 ||
                analysedSample.StructuralVariantOccurrences.Count() > 0 ||
                analysedSample.GeneExpressions.Count() > 0)
            .ToArray();
    }

    private static string GetSpecimenReferenceId(Unite.Data.Entities.Specimens.Specimen entity)
    {
        return entity.Tissue != null ? entity.Tissue.ReferenceId
             : entity.CellLine != null ? entity.CellLine.ReferenceId
             : entity.Organoid != null ? entity.Organoid.ReferenceId
             : entity.Xenograft != null ? entity.Xenograft.ReferenceId
             : throw new NotSupportedException("Specimen type is not supported");
    }

    private static string GetSpecimenType(Unite.Data.Entities.Specimens.Specimen entity)
    {
        return entity.Tissue != null ? "Tissue"
             : entity.CellLine != null ? "CellLine"
             : entity.Organoid != null ? "Organoid"
             : entity.Xenograft != null ? "xenograft"
             : throw new NotSupportedException("Specimen type is not supported");
    }
}
