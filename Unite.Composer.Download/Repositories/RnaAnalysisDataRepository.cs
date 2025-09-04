using Microsoft.EntityFrameworkCore;
using Unite.Data.Context;
using Unite.Data.Entities.Omics.Analysis.Enums;
using Unite.Data.Entities.Omics.Analysis.Rna;

namespace Unite.Composer.Download.Repositories;

public class RnaAnalysisDataRepository : OmicsAnalysisDataRepository
{
    private static readonly AnalysisType[] AnalysisTypes = [AnalysisType.RNASeq];

    public RnaAnalysisDataRepository(IDbContextFactory<DomainDbContext> dbContextFactory) : base(dbContextFactory)
    {
    }


    public async Task<GeneExpression[]> GetExpressionsForSamples(IEnumerable<int> ids)
    {
        var dbContext = _dbContextFactory.CreateDbContext();

        return await CreateExpressionsQuery(dbContext)
            .Where(entity => ids.Contains(entity.SampleId))
            .ToArrayAsync();
    }

    public async Task<GeneExpression[]> GetExpressionsForDonors(IEnumerable<int> ids)
    {
        var sampleIds = await _donorsRepository.GetRelatedSamples(ids, AnalysisTypes);

        return await GetExpressionsForSamples(sampleIds);
    }

    public async Task<GeneExpression[]> GetExpressionsForImages(IEnumerable<int> ids)
    {
        var sampleIds = await _imagesRepository.GetRelatedSamples(ids, AnalysisTypes);

        return await GetExpressionsForSamples(sampleIds);
    }

    public async Task<GeneExpression[]> GetExpressionsForSpecimens(IEnumerable<int> ids)
    {
        var sampleIds = await _specimensRepository.GetRelatedSamples(ids, AnalysisTypes);

        return await GetExpressionsForSamples(sampleIds);
    }


    private static IQueryable<GeneExpression> CreateExpressionsQuery(DomainDbContext dbContext)
    {
        return dbContext.Set<GeneExpression>()
            .AsNoTracking()
            .Include(entity => entity.Entity);
    }
}
