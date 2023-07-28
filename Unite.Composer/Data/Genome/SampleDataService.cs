using Microsoft.EntityFrameworkCore;
using Unite.Composer.Data.Genome.Models.Analysis;
using Unite.Data.Services;
using Unite.Data.Extensions;

namespace Unite.Composer.Data.Genome;

public class SampleDataService
{
    private readonly DomainDbContext _dbContext;


    public SampleDataService(IDbContextFactory<DomainDbContext> dbContextFactory)
    {
        _dbContext = dbContextFactory.CreateDbContext();
    }


    public async Task<AnalysedSample[]> GetDonorSamples(int donorId)
    {
        var specimens = await LoadDonorSpecimens(donorId);

        return await GetAnalysedSamples(specimens);
    }

    public async Task<AnalysedSample[]> GetImageSamples(int imageId)
    {
        var specimens = await LoadImageSpecimens(imageId);

        return await GetAnalysedSamples(specimens);
    }

    public async Task<AnalysedSample[]> GetSpecimenSamples(int specimenId)
    {
        var specimens = await LoadSpecimens(specimenId);

        return await GetAnalysedSamples(specimens);
    }


    private async Task<AnalysedSample[]> GetAnalysedSamples(Unite.Data.Entities.Specimens.Specimen[] specimens)
    {
        var models = new List<AnalysedSample>();

        foreach (var specimen in specimens)
        {
            var analysedSamples = (await LoadAnalysedSamples(specimen.Id))
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

                var model = new AnalysedSample
                {
                    Id = sample.Id,
                    ReferenceId = sample.ReferenceId,
                    Ploidy = ploidy,
                    Purity = purity,
                    Specimen = new() { Id = specimen.Id, ReferenceId = specimen.ReferenceId, Type = specimen.Type?.ToDefinitionString() },
                    Analyses = analyses
                };

                models.Add(model);
            }
        }

        return models.ToArray();
    }

    private async Task<Unite.Data.Entities.Specimens.Specimen[]> LoadDonorSpecimens(int donorId)
    {
        var specimens = await _dbContext.Set<Unite.Data.Entities.Specimens.Specimen>()
            .AsNoTracking()
            .Include(specimen => specimen.Tissue)
            .Include(specimen => specimen.CellLine)
            .Include(specimen => specimen.Organoid)
            .Include(specimen => specimen.Xenograft)
            .Where(specimen => specimen.ParentId == null)
            .Where(specimen => specimen.DonorId == donorId)
            .ToArrayAsync();

        return specimens;
    }

    private async Task<Unite.Data.Entities.Specimens.Specimen[]> LoadImageSpecimens(int imageId)
    {
        var donorId = await _dbContext.Set<Unite.Data.Entities.Images.Image>()
            .AsNoTracking()
            .Where(image => image.Id == imageId)
            .Select(image => image.DonorId)
            .FirstAsync();

        var specimens = await _dbContext.Set<Unite.Data.Entities.Specimens.Specimen>()
            .AsNoTracking()
            .Include(specimen => specimen.Tissue)
            .Where(specimen => specimen.Tissue != null)
            .Where(specimen => specimen.ParentId == null)
            .Where(specimen => specimen.DonorId == donorId)
            .ToArrayAsync();

        return specimens;
    }

    private async Task<Unite.Data.Entities.Specimens.Specimen[]> LoadSpecimens(int specimenId)
    {
        var specimens = await _dbContext.Set<Unite.Data.Entities.Specimens.Specimen>()
            .AsNoTracking()
            .Include(specimen => specimen.Tissue)
            .Where(specimen => specimen.Tissue != null)
            .Where(specimen => specimen.ParentId == null)
            .Where(specimen => specimen.Id == specimenId)
            .ToArrayAsync();

        return specimens;
    }

    private async Task<Unite.Data.Entities.Genome.Analysis.AnalysedSample[]> LoadAnalysedSamples(int specimenId)
    {
        var samples = await _dbContext.Set<Unite.Data.Entities.Genome.Analysis.AnalysedSample>()
            .AsNoTracking()
            .Include(analysedSample => analysedSample.Sample)
            .Include(analysedSample => analysedSample.Analysis)
            .Where(analysedSample => analysedSample.Sample.SpecimenId == specimenId)
            .Where(analysedSample => 
                analysedSample.MutationOccurrences.Any() ||
                analysedSample.CopyNumberVariantOccurrences.Any() ||
                analysedSample.StructuralVariantOccurrences.Any() ||
                analysedSample.GeneExpressions.Any())
            .ToArrayAsync();

        return samples;
    }
}
