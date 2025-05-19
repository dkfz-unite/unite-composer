using Microsoft.EntityFrameworkCore;
using Unite.Composer.Download.Tsv.Mapping.Extensions;
using Unite.Composer.Download.Tsv.Mapping.Models;
using Unite.Data.Context;
using Unite.Data.Entities.Omics.Analysis.Dna;
using Unite.Data.Entities.Omics.Analysis.Dna.Enums;
using Unite.Essentials.Tsv;

using SM = Unite.Data.Entities.Omics.Analysis.Dna.Sm;
using CNV = Unite.Data.Entities.Omics.Analysis.Dna.Cnv;
using SV = Unite.Data.Entities.Omics.Analysis.Dna.Sv;

namespace Unite.Composer.Download.Tsv.Mapping;

public class VariantsTsvService : TsvServiceBase
{
    public VariantsTsvService(IDbContextFactory<DomainDbContext> dbContextFactory) : base(dbContextFactory)
    {
    }

    public async Task<string> GetData(IEnumerable<int> ids, VariantType typeId, bool transcripts = false)
    {
        if (typeId == VariantType.SM)
            return await GetData<SM.VariantEntry, SM.Variant>(ids, transcripts);
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


    public async Task<string> GetFullData(IEnumerable<int> ids, VariantType typeId)
    {
        if (typeId == VariantType.SM)
            return await GetFullData<SM.VariantEntry, SM.Variant>(ids);
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


    private async Task<string> GetData<TVE, TV>(IEnumerable<int> ids, bool transcripts)
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

    private async Task<string> GetFullData<TVE, TV>(IEnumerable<int> ids)
        where TVE : VariantEntry<TV>
        where TV : Variant
    {
        var type = typeof(TV);

        if (type == typeof(SM.Variant))
            return await GetFullSmsData(ids);
        else if (type == typeof(CNV.Variant))
            return await GetFullCnvsData(ids);
        else if (type == typeof(SV.Variant))
            return await GetFullSvsData(ids);
        else
            return null;
    }

    private async Task<string> GetFullSmsData(IEnumerable<int> ids)
    {
        using var dbContext = _dbContextFactory.CreateDbContext();

        var entities = await CreateQuery<SM.VariantEntry, SM.Variant>(dbContext, true)
            .Where(entity => ids.Contains(entity.EntityId))
            .ToArrayAsync();

        var entries = entities
            .SelectMany(entity => entity.Entity.AffectedTranscripts, (vo, vat) => new SmEntryWithAffectedTranscript(vo, vat))
            .ToArray();

        var map = new ClassMap<SmEntryWithAffectedTranscript>().MapVariantEntries();
        
        return Write(entries, map);
    }

    private async Task<string> GetFullCnvsData(IEnumerable<int> ids)
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

    private async Task<string> GetFullSvsData(IEnumerable<int> ids)
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


    private async Task<int[]> GetIdsForDonors(IEnumerable<int> ids, VariantType typeId)
    {
        if (typeId == VariantType.SM)
            return await _donorsRepository.GetRelatedVariants<SM.Variant>(ids);
        else if (typeId == VariantType.CNV)
            return await _donorsRepository.GetRelatedVariants<CNV.Variant>(ids);
        else if (typeId == VariantType.SV)
            return await _donorsRepository.GetRelatedVariants<SV.Variant>(ids);
        
        return null;
    }

    private async Task<int[]> GetIdsForImages(IEnumerable<int> ids, VariantType typeId)
    {
        if (typeId == VariantType.SM)
            return await _imagesRepository.GetRelatedVariants<SM.Variant>(ids);
        else if (typeId == VariantType.CNV)
            return await _imagesRepository.GetRelatedVariants<CNV.Variant>(ids);
        else if (typeId == VariantType.SV)
            return await _imagesRepository.GetRelatedVariants<SV.Variant>(ids);
        
        return null;
    }

    private async Task<int[]> GetIdsForSpecimens(IEnumerable<int> ids, VariantType typeId)
    {
        if (typeId == VariantType.SM)
            return await _specimensRepository.GetRelatedVariants<SM.Variant>(ids);
        else if (typeId == VariantType.CNV)
            return await _specimensRepository.GetRelatedVariants<CNV.Variant>(ids);
        else if (typeId == VariantType.SV)
            return await _specimensRepository.GetRelatedVariants<SV.Variant>(ids);
        
        return null;
    }

    private async Task<int[]> GetIdsForGenes(IEnumerable<int> ids, VariantType typeId)
    {
        if (typeId == VariantType.SM)
            return await _genesRepository.GetRelatedVariants<SM.Variant>(ids);
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
            .Include(entity => entity.Sample.Analysis)
            .Include(entity => entity.Sample.Specimen.Donor)
            .Include(entity => entity.Sample.Specimen.Material)
            .Include(entity => entity.Sample.Specimen.Line)
            .Include(entity => entity.Sample.Specimen.Organoid)
            .Include(entity => entity.Sample.Specimen.Xenograft)
            .Include(entity => entity.Sample.Specimen.Donor)
            .Include(entity => entity.Sample.Specimen.Material)
            .Include(entity => entity.Sample.MatchedSample.Specimen.Material)
            .Include(entity => entity.Sample.MatchedSample.Specimen.Line)
            .Include(entity => entity.Sample.MatchedSample.Specimen.Organoid)
            .Include(entity => entity.Sample.MatchedSample.Specimen.Xenograft);
    }

    private static IQueryable<TVE> IncludeAffectedFeatures<TVE, TV>(IQueryable<TVE> query, bool transcripts)
        where TVE : VariantEntry<TV>
        where TV : Variant
    {
        if (query is IQueryable<SM.VariantEntry> ssmQuery)
            return IncludeAffectedFeatures(ssmQuery, transcripts) as IQueryable<TVE>;
        else if (query is IQueryable<CNV.VariantEntry> cnvQuery)
            return IncludeAffectedFeatures(cnvQuery, transcripts) as IQueryable<TVE>;
        else if (query is IQueryable<SV.VariantEntry> svQuery)
            return IncludeAffectedFeatures(svQuery, transcripts) as IQueryable<TVE>;
        else
            return query;
    }

    private static IQueryable<SM.VariantEntry> IncludeAffectedFeatures(IQueryable<SM.VariantEntry> query, bool transcripts)
    {
        IQueryable<SM.VariantEntry> includeQuery = query;

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
            .OrderBy(entity => entity.Sample.Specimen.DonorId)
            .ThenBy(entity => entity.Entity.ChromosomeId)
            .ThenBy(entity => entity.Entity.Start);
    }
}
