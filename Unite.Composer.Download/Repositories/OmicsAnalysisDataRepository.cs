using Microsoft.EntityFrameworkCore;
using Unite.Data.Context;
using Unite.Data.Entities.Omics.Analysis;
using Unite.Data.Entities.Omics.Analysis.Enums;

namespace Unite.Composer.Download.Repositories;

public abstract class OmicsAnalysisDataRepository : DataRepository
{
    public OmicsAnalysisDataRepository(IDbContextFactory<DomainDbContext> dbContextFactory) : base(dbContextFactory)
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

    public virtual async Task<Sample[]> GetSamplesForImages(IEnumerable<int> ids, IEnumerable<AnalysisType> typeIds = null)
    {
        var sampleIds = await _imagesRepository.GetRelatedSamples(ids, typeIds);

        return await GetSamples(sampleIds);
    }

    public virtual async Task<Sample[]> GetSamplesForSpecimens(IEnumerable<int> ids, IEnumerable<AnalysisType> typeIds = null)
    {
        var sampleIds = await _specimensRepository.GetRelatedSamples(ids, typeIds);

        return await GetSamples(sampleIds);
    }


    protected static IQueryable<Sample> CreateSamplesQuery(DomainDbContext dbContext)
    {
        return dbContext.Set<Sample>()
            .AsNoTracking()
            .Include(entity => entity.Specimen.Donor)
            .Include(entity => entity.MatchedSample.Specimen.Donor)
            .Include(entity => entity.Analysis);
    }
}
