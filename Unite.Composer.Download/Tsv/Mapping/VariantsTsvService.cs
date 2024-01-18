using Microsoft.EntityFrameworkCore;
using Unite.Composer.Download.Tsv.Mapping.Extensions;
using Unite.Composer.Download.Tsv.Mapping.Models;
using Unite.Data.Context;
using Unite.Data.Entities.Genome.Variants.Enums;
using Unite.Data.Entities.Genome.Variants;
using Unite.Essentials.Tsv;

using SSM = Unite.Data.Entities.Genome.Variants.SSM;
using CNV = Unite.Data.Entities.Genome.Variants.CNV;
using SV = Unite.Data.Entities.Genome.Variants.SV;

namespace Unite.Composer.Download.Tsv.Mapping;

public class VariantsTsvService : TsvServiceBase
{
    public VariantsTsvService(IDbContextFactory<DomainDbContext> dbContextFactory) : base(dbContextFactory)
    {
    }

    public async Task<string> GetData(IEnumerable<long> ids, VariantType typeId, bool transcripts = false)
    {
        if (typeId == VariantType.SSM)
            return await GetData<SSM.VariantEntry, SSM.Variant>(ids, transcripts);
        else if (typeId == VariantType.CNV)
            return await GetData<CNV.VariantEntry, CNV.Variant>(ids, transcripts);
        else if (typeId == VariantType.SV)
            return await GetData<SV.VariantEntry, SV.Variant>(ids, transcripts);
        
        return null;
    }

    public async Task<string> GetDataForDonors(IEnumerable<int> ids, VariantType typeId, bool transcripts = false)
    {
        var variantIds = await GetIdsForDonors(ids, typeId);

        return await GetData(variantIds, typeId, transcripts);
    }

    public async Task<string> GetDataForImages(IEnumerable<int> ids, VariantType typeId, bool transcripts = false)
    {
        var variantIds = await GetIdsForImages(ids, typeId);

        return await GetData(variantIds, typeId, transcripts);
    }

    public async Task<string> GetDataForSpecimens(IEnumerable<int> ids, VariantType typeId, bool transcripts = false)
    {
        var variantIds = await GetIdsForSpecimens(ids, typeId);

        return await GetData(variantIds, typeId, transcripts);
    }

    public async Task<string> GetDataForGenes(IEnumerable<int> ids, VariantType typeId, bool transcripts = false)
    {
        var variantIds = await GetIdsForGenes(ids, typeId);

        return await GetData(variantIds, typeId, transcripts);
    }


    public async Task<string> GetFullData(IEnumerable<long> ids, VariantType typeId)
    {
        if (typeId == VariantType.SSM)
            return await GetFullData<SSM.VariantEntry, SSM.Variant>(ids);
        else if (typeId == VariantType.CNV)
            return await GetFullData<CNV.VariantEntry, CNV.Variant>(ids);
        else if (typeId == VariantType.SV)
            return await GetFullData<SV.VariantEntry, SV.Variant>(ids);

        return null;
    }

    public async Task<string> GetFullDataForDonors(IEnumerable<int> ids, VariantType typeId)
    {
        var variantIds = await GetIdsForDonors(ids, typeId);

        return await GetFullData(variantIds, typeId);
    }

    public async Task<string> GetFullDataForImages(IEnumerable<int> ids, VariantType typeId)
    {
        var variantIds = await GetIdsForImages(ids, typeId);

        return await GetFullData(variantIds, typeId);
    }

    public async Task<string> GetFullDataForSpecimens(IEnumerable<int> ids, VariantType typeId)
    {
        var variantIds = await GetIdsForSpecimens(ids, typeId);

        return await GetFullData(variantIds, typeId);
    }

    public async Task<string> GetFullDataForGenes(IEnumerable<int> ids, VariantType typeId)
    {
        var variantIds = await GetIdsForGenes(ids, typeId);

        return await GetFullData(variantIds, typeId);
    }


    private async Task<string> GetData<TVE, TV>(IEnumerable<long> ids, bool transcripts)
        where TVE : VariantEntry<TV>
        where TV : Variant
    {
        using var dbContext = _dbContextFactory.CreateDbContext();

        var entities = await CreateQuery<TVE, TV>(dbContext, transcripts)
            .Where(entity => ids.Contains(entity.EntityId))
            .ToArrayAsync();

        var map = new ClassMap<TVE>().MapVariantEntries<TVE, TV>(transcripts);

        return Write(entities, map);
    }

    private async Task<string> GetFullData<TVE, TV>(IEnumerable<long> ids)
        where TVE : VariantEntry<TV>
        where TV : Variant
    {
        var type = typeof(TV);

        if (type == typeof(SSM.Variant))
            return await GetFullSsmsData(ids);
        else if (type == typeof(CNV.Variant))
            return await GetFullCnvsData(ids);
        else if (type == typeof(SV.Variant))
            return await GetFullSvsData(ids);
        else
            return null;
    }

    private async Task<string> GetFullSsmsData(IEnumerable<long> ids)
    {
        using var dbContext = _dbContextFactory.CreateDbContext();

        var entities = await CreateQuery<SSM.VariantEntry, SSM.Variant>(dbContext, true)
            .Where(entity => ids.Contains(entity.EntityId))
            .ToArrayAsync();

        var entries = entities
            .SelectMany(entity => entity.Entity.AffectedTranscripts, (vo, vat) => new SsmEntryWithAffectedTranscript(vo, vat))
            .ToArray();

        var map = new ClassMap<SsmEntryWithAffectedTranscript>().MapVariantEntries();
        
        return Write(entries, map);
    }

    private async Task<string> GetFullCnvsData(IEnumerable<long> ids)
    {
        using var dbContext = _dbContextFactory.CreateDbContext();

        var entities = await CreateQuery<CNV.VariantEntry, CNV.Variant>(dbContext, true)
            .Where(entity => ids.Contains(entity.EntityId))
            .ToArrayAsync();

        var entries = entities
            .SelectMany(entity => entity.Entity.AffectedTranscripts, (vo, vat) => new CnvEntryWithAffectedTranscript(vo, vat))
            .ToArray();

        var map = new ClassMap<CnvEntryWithAffectedTranscript>().MapVariantEntries();
        
        return Write(entries, map);
    }

    private async Task<string> GetFullSvsData(IEnumerable<long> ids)
    {
        using var dbContext = _dbContextFactory.CreateDbContext();

        var entities = await CreateQuery<SV.VariantEntry, SV.Variant>(dbContext, true)
            .Where(entity => ids.Contains(entity.EntityId))
            .ToArrayAsync();

        var entries = entities
            .SelectMany(entity => entity.Entity.AffectedTranscripts, (vo, vat) => new SvEntryWithAffectedTranscript(vo, vat))
            .ToArray();

        var map = new ClassMap<SvEntryWithAffectedTranscript>().MapVariantEntries();
        
        return Write(entries, map);
    }


    private async Task<long[]> GetIdsForDonors(IEnumerable<int> ids, VariantType typeId)
    {
        if (typeId == VariantType.SSM)
            return await _donorsRepository.GetRelatedVariants<SSM.Variant>(ids);
        else if (typeId == VariantType.CNV)
            return await _donorsRepository.GetRelatedVariants<CNV.Variant>(ids);
        else if (typeId == VariantType.SV)
            return await _donorsRepository.GetRelatedVariants<SV.Variant>(ids);
        
        return null;
    }

    private async Task<long[]> GetIdsForImages(IEnumerable<int> ids, VariantType typeId)
    {
        if (typeId == VariantType.SSM)
            return await _imagesRepository.GetRelatedVariants<SSM.Variant>(ids);
        else if (typeId == VariantType.CNV)
            return await _imagesRepository.GetRelatedVariants<CNV.Variant>(ids);
        else if (typeId == VariantType.SV)
            return await _imagesRepository.GetRelatedVariants<SV.Variant>(ids);
        
        return null;
    }

    private async Task<long[]> GetIdsForSpecimens(IEnumerable<int> ids, VariantType typeId)
    {
        if (typeId == VariantType.SSM)
            return await _specimensRepository.GetRelatedVariants<SSM.Variant>(ids);
        else if (typeId == VariantType.CNV)
            return await _specimensRepository.GetRelatedVariants<CNV.Variant>(ids);
        else if (typeId == VariantType.SV)
            return await _specimensRepository.GetRelatedVariants<SV.Variant>(ids);
        
        return null;
    }

    private async Task<long[]> GetIdsForGenes(IEnumerable<int> ids, VariantType typeId)
    {
        if (typeId == VariantType.SSM)
            return await _genesRepository.GetRelatedVariants<SSM.Variant>(ids);
        else if (typeId == VariantType.CNV)
            return await _genesRepository.GetRelatedVariants<CNV.Variant>(ids);
        else if (typeId == VariantType.SV)
            return await _genesRepository.GetRelatedVariants<SV.Variant>(ids);
        
        return null;
    }


    private static IQueryable<TVE> CreateQuery<TVE, TV>(DomainDbContext dbContext, bool transcripts)
        where TVE : VariantEntry<TV>
        where TV : Variant
    {
        var query = dbContext.Set<TVE>().AsNoTracking();

        query = IncludeAffectedFeatures<TVE, TV>(query, transcripts);
        query = IncludeVariant<TVE, TV>(query);
        query = IncludeSamples<TVE, TV>(query);
        query = OrderVariant<TVE, TV>(query);

        return query;
    }

    private static IQueryable<TVE> IncludeVariant<TVE, TV>(IQueryable<TVE> query)
        where TVE : VariantEntry<TV>
        where TV : Variant
    {
        return query
            .Include(entity => entity.Entity);
    }

    private static IQueryable<TVE> IncludeSamples<TVE, TV>(IQueryable<TVE> query)
        where TVE : VariantEntry<TV>
        where TV : Variant
    {
        return query
            .Include(entity => entity.AnalysedSample.Analysis)
            .Include(entity => entity.AnalysedSample.TargetSample.Donor)
            .Include(entity => entity.AnalysedSample.TargetSample.Material)
            .Include(entity => entity.AnalysedSample.TargetSample.Line)
            .Include(entity => entity.AnalysedSample.TargetSample.Organoid)
            .Include(entity => entity.AnalysedSample.TargetSample.Xenograft)
            .Include(entity => entity.AnalysedSample.MatchedSample.Donor)
            .Include(entity => entity.AnalysedSample.MatchedSample.Material)
            .Include(entity => entity.AnalysedSample.MatchedSample.Line)
            .Include(entity => entity.AnalysedSample.MatchedSample.Organoid)
            .Include(entity => entity.AnalysedSample.MatchedSample.Xenograft);
    }

    private static IQueryable<TVE> IncludeAffectedFeatures<TVE, TV>(IQueryable<TVE> query, bool transcripts)
        where TVE : VariantEntry<TV>
        where TV : Variant
    {
        if (query is IQueryable<SSM.VariantEntry> ssmQuery)
            return IncludeAffectedFeatures(ssmQuery, transcripts) as IQueryable<TVE>;
        else if (query is IQueryable<CNV.VariantEntry> cnvQuery)
            return IncludeAffectedFeatures(cnvQuery, transcripts) as IQueryable<TVE>;
        else if (query is IQueryable<SV.VariantEntry> svQuery)
            return IncludeAffectedFeatures(svQuery, transcripts) as IQueryable<TVE>;
        else
            return query;
    }

    private static IQueryable<SSM.VariantEntry> IncludeAffectedFeatures(IQueryable<SSM.VariantEntry> query, bool transcripts)
    {
        IQueryable<SSM.VariantEntry> includeQuery = query;

        if (transcripts)
        {
            includeQuery = includeQuery
                .Include(entity => entity.Entity.AffectedTranscripts)
                    .ThenInclude(entity => entity.Feature.Gene)
                .Include(entity => entity.Entity.AffectedTranscripts)
                    .ThenInclude(entity => entity.Feature.Protein);
        }
        
        return includeQuery;
    }

    private static IQueryable<CNV.VariantEntry> IncludeAffectedFeatures(IQueryable<CNV.VariantEntry> query, bool transcripts)
    {
        IQueryable<CNV.VariantEntry> includeQuery = query;

        if (transcripts)
        {
            includeQuery = includeQuery
                .Include(entity => entity.Entity.AffectedTranscripts)
                    .ThenInclude(entity => entity.Feature.Gene)
                .Include(entity => entity.Entity.AffectedTranscripts)
                    .ThenInclude(entity => entity.Feature.Protein);
        }
        
        return includeQuery;
    }

    private static IQueryable<SV.VariantEntry> IncludeAffectedFeatures(IQueryable<SV.VariantEntry> query, bool transcripts)
    {
        IQueryable<SV.VariantEntry> includeQuery = query;

        if (transcripts)
        {
            includeQuery = includeQuery
                .Include(entity => entity.Entity.AffectedTranscripts)
                    .ThenInclude(entity => entity.Feature.Gene)
                .Include(entity => entity.Entity.AffectedTranscripts)
                    .ThenInclude(entity => entity.Feature.Protein);
        }
        
        return includeQuery;
    }

    private static IQueryable<TVE> OrderVariant<TVE, TV>(IQueryable<TVE> query)
        where TVE : VariantEntry<TV>
        where TV : Variant
    {
        return query
            .OrderBy(entity => entity.AnalysedSample.TargetSample.DonorId)
            .ThenBy(entity => entity.Entity.ChromosomeId)
            .ThenBy(entity => entity.Entity.Start);
    }
}
