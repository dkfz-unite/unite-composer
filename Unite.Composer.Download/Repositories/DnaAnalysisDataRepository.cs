using Microsoft.EntityFrameworkCore;
using Unite.Data.Entities.Omics.Analysis.Enums;
using Unite.Data.Entities.Omics.Analysis.Dna;
using Unite.Data.Context;
using Unite.Data.Context.Repositories.Extensions.Queryable;

using SM = Unite.Data.Entities.Omics.Analysis.Dna.Sm;
using CNV = Unite.Data.Entities.Omics.Analysis.Dna.Cnv;
using SV = Unite.Data.Entities.Omics.Analysis.Dna.Sv;

namespace Unite.Composer.Download.Repositories;

public class DnaAnalysisDataRepository : OmicsAnalysisDataRepository
{
    private static readonly AnalysisType[] AnalysisTypes = [AnalysisType.WES, AnalysisType.WGS];


    public DnaAnalysisDataRepository(IDbContextFactory<DomainDbContext> dbContextFactory) : base(dbContextFactory)
    {
    }

    public async Task<TVE[]> GetVariantsForSamples<TVE, TV>(IEnumerable<int> ids, bool transcripts)
        where TVE : VariantEntry<TV>
        where TV : Variant
    {
        using var dbContext = _dbContextFactory.CreateDbContext();

        return await CreateVariantsQuery<TVE, TV>(dbContext, transcripts)
            .Where(entity => ids.Contains(entity.SampleId))
            .ToArrayAsync();
    }

    public async Task<TVE[]> GetVariantsForDonors<TVE, TV>(IEnumerable<int> ids, bool transcripts)
        where TVE : VariantEntry<TV>
        where TV : Variant
    {
        var sampleIds = await _donorsRepository.GetRelatedSamples(ids, AnalysisTypes);

        return await GetVariantsForSamples<TVE, TV>(sampleIds, transcripts);
    }

    public async Task<TVE[]> GetVariantsForImages<TVE, TV>(IEnumerable<int> ids, bool transcripts)
        where TVE : VariantEntry<TV>
        where TV : Variant
    {
        var sampleIds = await _imagesRepository.GetRelatedSamples(ids, AnalysisTypes);

        return await GetVariantsForSamples<TVE, TV>(sampleIds, transcripts);
    }

    public async Task<TVE[]> GetVariantsForSpecimens<TVE, TV>(IEnumerable<int> ids, bool transcripts)
        where TVE : VariantEntry<TV>
        where TV : Variant
    {
        var sampleIds = await _specimensRepository.GetRelatedSamples(ids, AnalysisTypes);

        return await GetVariantsForSamples<TVE, TV>(sampleIds, transcripts);
    }


    private static IQueryable<TVE> CreateVariantsQuery<TVE, TV>(DomainDbContext dbContext, bool transcripts)
        where TVE : VariantEntry<TV>
        where TV : Variant
    {
        if (typeof(TVE) == typeof(SM.VariantEntry))
            return (IQueryable<TVE>)CreateSmsQuery(dbContext, transcripts);
        else if (typeof(TVE) == typeof(CNV.VariantEntry))
            return (IQueryable<TVE>)CreateCnvsQuery(dbContext, transcripts);
        else if (typeof(TVE) == typeof(SV.VariantEntry))
            return (IQueryable<TVE>)CreateSvsQuery(dbContext, transcripts);
        else
            throw new NotSupportedException($"Type '{typeof(TVE)}' is not supported.");
    }

    private static IQueryable<SM.VariantEntry> CreateSmsQuery(DomainDbContext dbContext, bool transcripts)
    {
        var query = dbContext.Set<SM.VariantEntry>().AsNoTracking();

        if (transcripts)
            query = query.IncludeAffectedTranscripts();

        return query;
    }

    private static IQueryable<CNV.VariantEntry> CreateCnvsQuery(DomainDbContext dbContext, bool transcripts)
    {
        var query = dbContext.Set<CNV.VariantEntry>().AsNoTracking();

        if (transcripts)
            query = query.IncludeAffectedTranscripts();
            
        return query;
    }

    private static IQueryable<SV.VariantEntry> CreateSvsQuery(DomainDbContext dbContext, bool transcripts)
    {
        var query = dbContext.Set<SV.VariantEntry>().AsNoTracking();

        if (transcripts)
            query = query.IncludeAffectedTranscripts();

        return query;
    }
}
