using Microsoft.EntityFrameworkCore;
using Unite.Data.Context;
using Unite.Data.Entities.Specimens.Analysis;
using Unite.Data.Entities.Specimens.Analysis.Drugs;
using Unite.Data.Entities.Specimens.Analysis.Enums;

namespace Unite.Composer.Download.Repositories;

public class SpecimenAnalysisDataRepository : DataRepository
{
    public SpecimenAnalysisDataRepository(IDbContextFactory<DomainDbContext> dbContextFactory) : base(dbContextFactory)
    {
    }

    public virtual async Task<Sample[]> GetSamples(IEnumerable<int> ids)
    {
        using var dbContext = _dbContextFactory.CreateDbContext();

        return await CreateSamplesQuery(dbContext)
            .Where(entity => ids.Contains(entity.Id))
            .ToArrayAsync();
    }

    public virtual async Task<Sample[]> GetSamplesForDonors(IEnumerable<int> ids, IEnumerable<AnalysisType> typeIds = null)
    {
        var sampleIds = await _donorsRepository.GetRelatedSamples(ids, typeIds);

        return await GetSamples(sampleIds);
    }

    public virtual async Task<Sample[]> GetSamplesForSpecimens(IEnumerable<int> ids, IEnumerable<AnalysisType> typeIds = null)
    {
        var sampleIds = await _specimensRepository.GetRelatedSamples(ids, typeIds);

        return await GetSamples(sampleIds);
    }


    public async Task<DrugScreening[]> GetDrugsForSamples(IEnumerable<int> ids)
    {
        using var dbContext = _dbContextFactory.CreateDbContext();

        return await CreateDrugScreeningsQuery(dbContext)
            .Where(entity => ids.Contains(entity.SampleId))
            .ToArrayAsync();
    }

    public async Task<DrugScreening[]> GetDrugsForDonors(IEnumerable<int> ids)
    {
        var sampleIds = await _donorsRepository.GetRelatedSamples(ids, [AnalysisType.DSA]);

        return await GetDrugsForSamples(sampleIds);
    }

    public async Task<DrugScreening[]> GetDrugsForSpecimens(IEnumerable<int> ids)
    {
        var sampleIds = await _specimensRepository.GetRelatedSamples(ids, [AnalysisType.DSA]);

        return await GetDrugsForSamples(sampleIds);
    }


    protected static IQueryable<Sample> CreateSamplesQuery(DomainDbContext dbContext)
    {
        return dbContext.Set<Sample>()
            .AsNoTracking()
            .Include(entity => entity.Specimen.Donor)
            .Include(entity => entity.Analysis);
    }

    protected static IQueryable<DrugScreening> CreateDrugScreeningsQuery(DomainDbContext dbContext)
    {
        return dbContext.Set<DrugScreening>()
            .AsNoTracking()
            .Include(entity => entity.Sample.Specimen.Donor)
            .Include(entity => entity.Entity);
    }
}
