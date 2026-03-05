using Microsoft.EntityFrameworkCore;
using Unite.Data.Context;
using Unite.Data.Entities.Omics.Analysis.Enums;
using Unite.Data.Entities.Omics.Analysis.Prot;

namespace Unite.Composer.Download.Repositories;

public class ProtAnalysisDataRepository : OmicsAnalysisDataRepository
{
    private static readonly AnalysisType[] AnalysisTypes = [AnalysisType.MS];

    public ProtAnalysisDataRepository(IDbContextFactory<DomainDbContext> dbContextFactory) : base(dbContextFactory)
    {
    }


    public async Task<ProteinExpression[]> GetExpressionsForSamples(IEnumerable<int> ids)
    {
        var dbContext = _dbContextFactory.CreateDbContext();

        return await CreateExpressionsQuery(dbContext)
            .Where(entity => ids.Contains(entity.SampleId))
            .ToArrayAsync();
    }

    public async Task<ProteinExpression[]> GetExpressionsForDonors(IEnumerable<int> ids)
    {
        var sampleIds = await _donorsRepository.GetRelatedSamples(ids, AnalysisTypes);

        return await GetExpressionsForSamples(sampleIds);
    }

    public async Task<ProteinExpression[]> GetExpressionsForImages(IEnumerable<int> ids)
    {
        var sampleIds = await _imagesRepository.GetRelatedSamples(ids, AnalysisTypes);

        return await GetExpressionsForSamples(sampleIds);
    }

    public async Task<ProteinExpression[]> GetExpressionsForSpecimens(IEnumerable<int> ids)
    {
        var sampleIds = await _specimensRepository.GetRelatedSamples(ids, AnalysisTypes);

        return await GetExpressionsForSamples(sampleIds);
    }


    private static IQueryable<ProteinExpression> CreateExpressionsQuery(DomainDbContext dbContext)
    {
        return dbContext.Set<ProteinExpression>()
            .AsNoTracking()
            .Include(entity => entity.Entity);
    }
}
