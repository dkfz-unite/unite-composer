using Microsoft.EntityFrameworkCore;
using Unite.Composer.Download.Tsv.Mapping.Extensions;
using Unite.Data.Context;
using Unite.Data.Entities.Genome.Transcriptomics;
using Unite.Data.Entities.Genome.Variants;
using Unite.Essentials.Tsv;

namespace Unite.Composer.Download.Tsv.Mapping;

public class TranscriptomicsTsvService : TsvServiceBase
{
    public TranscriptomicsTsvService(IDbContextFactory<DomainDbContext> dbContextFactory) : base(dbContextFactory)
    {
    }


    public async Task<string> GetData(IEnumerable <int> ids, IEnumerable<int> specimenIds = null)
    {
        using var dbContext = _dbContextFactory.CreateDbContext();

        var map = new ClassMap<BulkExpression>().MapBulkExpressions();

        var entities = await CreateQuery(dbContext)
            .Where(entity => ids.Contains(entity.EntityId))
            .Where(entity => specimenIds == null || specimenIds.Contains(entity.AnalysedSample.TargetSampleId))
            .ToArrayAsync();

        return Write(entities, map);
    }

    public async Task<string> GetDataForSpecimens(IEnumerable<int> ids)
    {
        using var dbContext = _dbContextFactory.CreateDbContext();

        var map = new ClassMap<BulkExpression>().MapBulkExpressions();

        var entities = await CreateQuery(dbContext)
            .Where(entity => ids.Contains(entity.AnalysedSample.TargetSampleId))
            .ToArrayAsync();

        return Write(entities, map);
    }

    public async Task<string> GetDataForDonors(IEnumerable<int> ids)
    {
        var specimenIds = await _donorsRepository.GetRelatedSpecimens(ids.ToArray());

        return await GetDataForSpecimens(specimenIds);
    }

    public async Task<string> GetDataForImages(IEnumerable<int> ids)
    {
        var specimenIds = await _imagesRepository.GetRelatedSpecimens(ids.ToArray());

        return await GetDataForSpecimens(specimenIds);
    }

    public async Task<string> GetDataForVariants<TV>(IEnumerable<long> ids)
        where TV : Variant
    {
        var geneIds = await _variantsRepository.GetRelatedGenes<TV>(ids.ToArray());
        var specimenIds = await _variantsRepository.GetRelatedSpecimens<TV>(ids.ToArray());

        return await GetData(geneIds, specimenIds);
    }


    private static IQueryable<BulkExpression> CreateQuery(DomainDbContext dbContext)
    {
        return dbContext.Set<BulkExpression>()
            .AsNoTracking()
            .Include(entity => entity.Entity)
            .Include(entity => entity.AnalysedSample.TargetSample.Donor)
            .Include(entity => entity.AnalysedSample.TargetSample.Tissue)
            .Include(entity => entity.AnalysedSample.TargetSample.CellLine)
            .Include(entity => entity.AnalysedSample.TargetSample.Organoid)
            .Include(entity => entity.AnalysedSample.TargetSample.Xenograft);
    }
}
