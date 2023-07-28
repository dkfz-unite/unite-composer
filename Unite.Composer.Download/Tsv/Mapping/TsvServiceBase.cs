using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Unite.Data.Entities.Genome.Transcriptomics;
using Unite.Data.Entities.Genome.Variants;
using Unite.Data.Services;

using SSM = Unite.Data.Entities.Genome.Variants.SSM;
using CNV = Unite.Data.Entities.Genome.Variants.CNV;
using SV = Unite.Data.Entities.Genome.Variants.SV;

namespace Unite.Composer.Download.Tsv.Mapping;

public abstract class TsvServiceBase
{
    protected readonly IDbContextFactory<DomainDbContext> _dbContextFactory;    

    public TsvServiceBase(IDbContextFactory<DomainDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }


    protected virtual async Task<int[]> GetDonorIdsForGenes(IEnumerable<int> ids)
    {
        var withSsmsTask = GetEntitiesForSsmAffectedGenes(ids, entity => entity.AnalysedSample.Sample.Specimen.DonorId);
        var withCnvsTask = GetEntitiesForCnvAffectedGenes(ids, entity => entity.AnalysedSample.Sample.Specimen.DonorId);
        var withSvsTask = GetEntitiesForSvAffectedGenes(ids, entity => entity.AnalysedSample.Sample.Specimen.DonorId);
        var withExpressionsTask = GetEntitiesForExpressedGenes(ids, entity => entity.AnalysedSample.Sample.Specimen.DonorId);

        await Task.WhenAll(withSsmsTask, withCnvsTask, withSvsTask, withExpressionsTask);

        var withSsms = withSsmsTask.Result;
        var withCnvs = withCnvsTask.Result;
        var withSvs = withSvsTask.Result;
        var withExpressions = withExpressionsTask.Result;

        return Array.Empty<int>().Union(withSsms).Union(withCnvs).Union(withSvs).Union(withExpressions).ToArray();
    }

    protected virtual async Task<int[]> GetSpecimenIdsForGenes(IEnumerable<int> ids)
    {
        var withSsmsTask = GetEntitiesForSsmAffectedGenes(ids, entity => entity.AnalysedSample.Sample.SpecimenId);
        var withCnvsTask = GetEntitiesForCnvAffectedGenes(ids, entity => entity.AnalysedSample.Sample.SpecimenId);
        var withSvsTask = GetEntitiesForSvAffectedGenes(ids, entity => entity.AnalysedSample.Sample.SpecimenId);
        var withExpressionsTask = GetEntitiesForExpressedGenes(ids, entity => entity.AnalysedSample.Sample.SpecimenId);

        await Task.WhenAll(withSsmsTask, withCnvsTask, withSvsTask, withExpressionsTask);

        var withSsms = withSsmsTask.Result;
        var withCnvs = withCnvsTask.Result;
        var withSvs = withSvsTask.Result;
        var withExpressions = withExpressionsTask.Result;

        return Array.Empty<int>().Union(withSsms).Union(withCnvs).Union(withSvs).Union(withExpressions).ToArray();
    }

    protected virtual async Task<int[]> GetDonorIdsForVariants<TVO, TV>(IEnumerable<long> ids)
        where TVO : VariantOccurrence<TV>
        where TV : Variant
    {
        return await GetEntitiesForVariants<TVO, TV, int>(ids, entity => entity.AnalysedSample.Sample.Specimen.DonorId);
    }

    protected virtual async Task<int[]> GetSpecimenIdsForVariants<TVO, TV>(IEnumerable<long> ids)
        where TVO : VariantOccurrence<TV>
        where TV : Variant
    {
        return await GetEntitiesForVariants<TVO, TV, int>(ids, entity => entity.AnalysedSample.Sample.SpecimenId);
    }


    private async Task<T[]> GetEntitiesForSsmAffectedGenes<T>(IEnumerable<int> ids, Expression<Func<SSM.VariantOccurrence, T>> selector)
    {
        using var dbContext = _dbContextFactory.CreateDbContext();

        return await dbContext.Set<SSM.VariantOccurrence>().AsNoTracking()
            .Where(entity => entity.Variant.AffectedTranscripts
                .Any(affected => ids.Contains(affected.Feature.GeneId.Value)))
            .Select(selector)
            .Distinct()
            .ToArrayAsync();
    }

    private async Task<T[]> GetEntitiesForCnvAffectedGenes<T>(IEnumerable<int> ids, Expression<Func<CNV.VariantOccurrence, T>> selector)
    {
        using var dbContext = _dbContextFactory.CreateDbContext();

        return await dbContext.Set<CNV.VariantOccurrence>().AsNoTracking()
            .Where(entity => entity.Variant.AffectedTranscripts
                .Any(affected => ids.Contains(affected.Feature.GeneId.Value)))
            .Select(selector)
            .Distinct()
            .ToArrayAsync();
    }

    private async Task<T[]> GetEntitiesForSvAffectedGenes<T>(IEnumerable<int> ids, Expression<Func<SV.VariantOccurrence, T>> selector)
    {
        using var dbContext = _dbContextFactory.CreateDbContext();

        return await dbContext.Set<SV.VariantOccurrence>().AsNoTracking()
            .Where(entity => entity.Variant.AffectedTranscripts
                .Any(affected => ids.Contains(affected.Feature.GeneId.Value)))
            .Select(selector)
            .Distinct()
            .ToArrayAsync();
    }

    private async Task<T[]> GetEntitiesForExpressedGenes<T>(IEnumerable<int> ids, Expression<Func<GeneExpression, T>> selector)
    {
        using var dbContext = _dbContextFactory.CreateDbContext();

        return await dbContext.Set<GeneExpression>().AsNoTracking()
            .Where(entity => ids.Contains(entity.GeneId))
            .Select(selector)
            .Distinct()
            .ToArrayAsync();
    }

    private async Task <T[]> GetEntitiesForVariants<TVO, TV, T>(IEnumerable<long> ids, Expression<Func<TVO, T>> selector)
        where TVO : VariantOccurrence<TV>
        where TV : Variant
    {
        using var dbContext = _dbContextFactory.CreateDbContext();

        return await dbContext.Set<TVO>().AsNoTracking()
            .Where(entity => ids.Contains(entity.VariantId))
            .Select(selector)
            .Distinct()
            .ToArrayAsync();
    }
}
