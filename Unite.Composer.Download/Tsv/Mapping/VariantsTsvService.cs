using Microsoft.EntityFrameworkCore;
using Unite.Composer.Download.Extensions.Queryable;
using Unite.Composer.Download.Tsv.Mapping.Extensions;
using Unite.Composer.Download.Tsv.Mapping.Models;
using Unite.Data.Entities.Genome.Variants;
using Unite.Data.Entities.Images;
using Unite.Data.Entities.Specimens;
using Unite.Data.Entities.Specimens.Tissues.Enums;
using Unite.Data.Services;
using Unite.Essentials.Tsv;

using SSM = Unite.Data.Entities.Genome.Variants.SSM;
using CNV = Unite.Data.Entities.Genome.Variants.CNV;
using SV = Unite.Data.Entities.Genome.Variants.SV;

namespace Unite.Composer.Download.Tsv.Mapping;

public class VariantsTsvService
{
    //TODO: Use DbContextFactory per request to allow parallel queries
    private readonly DomainDbContext _dbContext;

    public VariantsTsvService(IDbContextFactory<DomainDbContext> dbContextFactory)
    {
        _dbContext = dbContextFactory.CreateDbContext();
    }


    public async Task<string> GetSsmsData(IEnumerable<long> ids, bool transcripts = false)
    {
        var query = CreateSsmsQuery(transcripts).FilterByVariantIds(ids);

        var map = new ClassMap<SSM.VariantOccurrence>().MapVariantOccurrences(transcripts);

        var entities = await query.ToArrayAsync();

        return Write(entities, map);
    }

    public async Task<string> GetCnvsData(IEnumerable<long> ids, bool transcripts = false)
    {
        var query = CreateCnvsQuery(transcripts).FilterByVariantIds(ids);

        var map = new ClassMap<CNV.VariantOccurrence>().MapVariantOccurrences(transcripts);

        var entities = await query.ToArrayAsync();

        return Write(entities, map);
    }

    public async Task<string> GetSvsData(IEnumerable<long> ids, bool transcripts = false)
    {
        var query = CreateSvsQuery(transcripts).FilterByVariantIds(ids);

        var map = new ClassMap<SV.VariantOccurrence>().MapVariantOccurrences(transcripts);

        var entities = await query.ToArrayAsync();

        return Write(entities, map);
    }
    
    public async Task<string> GetSsmsDataForDonors(IEnumerable<int> ids, bool transcripts = false)
    {
        var specimenIds = await GetSpecimenIdsForDonors(ids);

        return await GetSsmsDataForSpecimens(specimenIds, transcripts);
    }

    public async Task<string> GetCnvsDataForDonors(IEnumerable<int> ids, bool transcripts = false)
    {
        var specimenIds = await GetSpecimenIdsForDonors(ids);

        return await GetCnvsDataForSpecimens(specimenIds, transcripts);
    }

    public async Task<string> GetSvsDataForDonors(IEnumerable<int> ids, bool transcripts = false)
    {
        var specimenIds = await GetSpecimenIdsForDonors(ids);

        return await GetSvsDataForSpecimens(specimenIds, transcripts);
    }

    public async Task<string> GetSsmsDataForImages(IEnumerable<int> ids, bool transcripts = false)
    {
        var specimenIds = await GetSpecimenIdsForImages(ids);

        return await GetSsmsDataForSpecimens(specimenIds, transcripts);
    }

    public async Task<string> GetCnvsDataForImages(IEnumerable<int> ids, bool transcripts = false)
    {
        var specimenIds = await GetSpecimenIdsForImages(ids);

        return await GetCnvsDataForSpecimens(specimenIds, transcripts);
    }

    public async Task<string> GetSvsDataForImages(IEnumerable<int> ids, bool transcripts = false)
    {
        var specimenIds = await GetSpecimenIdsForImages(ids);

        return await GetSvsDataForSpecimens(specimenIds, transcripts);
    }

    public async Task<string> GetSsmsDataForSpecimens(IEnumerable<int> ids, bool transcripts = false)
    {
        var query = CreateSsmsQuery(transcripts).FilterBySpecimenIds(ids);

        var map = new ClassMap<SSM.VariantOccurrence>().MapVariantOccurrences(transcripts);

        var entities = await query.ToArrayAsync();

        return Write(entities, map);
    }

    public async Task<string> GetCnvsDataForSpecimens(IEnumerable<int> ids, bool transcripts = false)
    {
        var query = CreateCnvsQuery(transcripts).FilterByAffectedGeneIds(ids);

        var map = new ClassMap<CNV.VariantOccurrence>().MapVariantOccurrences(transcripts);

        var entities = await query.ToArrayAsync();

        return Write(entities, map);
    }

    public async Task<string> GetSvsDataForSpecimens(IEnumerable<int> ids, bool transcripts = false)
    {
        var query = CreateSvsQuery(transcripts).FilterBySpecimenIds(ids);

        var map = new ClassMap<SV.VariantOccurrence>().MapVariantOccurrences(transcripts);

        var entities = await query.ToArrayAsync();

        return Write(entities, map);
    }

    public async Task<string> GetSsmsDataForGenes(IEnumerable<int> ids, bool transcripts = false)
    {
        var query = CreateSsmsQuery(transcripts).FilterByAffectedGeneIds(ids);

        var map = new ClassMap<SSM.VariantOccurrence>().MapVariantOccurrences(transcripts);

        var entities = await query.ToArrayAsync();

        return Write(entities, map);
    }

    public async Task<string> GetCnvsDataForGenes(IEnumerable<int> ids, bool transcripts = false)
    {
        var query = CreateCnvsQuery(transcripts).FilterByAffectedGeneIds(ids);

        var map = new ClassMap<CNV.VariantOccurrence>().MapVariantOccurrences(transcripts);

        var entities = await query.ToArrayAsync();

        return Write(entities, map);
    }

    public async Task<string> GetSvsDataForGenes(IEnumerable<int> ids, bool transcripts = false)
    {
        var query = CreateSvsQuery(transcripts).FilterByAffectedGeneIds(ids);

        var map = new ClassMap<SV.VariantOccurrence>().MapVariantOccurrences(transcripts);

        var entities = await query.ToArrayAsync();

        return Write(entities, map);
    }


    public async Task<string> GetFullSsmsData(IEnumerable<long> ids)
    {
        var query = CreateSsmsQuery(true).FilterByVariantIds(ids);

        var map = new ClassMap<SsmOccurrenceWithAffectedTranscript>().MapVariantOccurrences();

        var entities = await query.ToArrayAsync();
        
        var entries = entities?.SelectMany(vo => vo.Variant.AffectedTranscripts, (vo, vat) => new SsmOccurrenceWithAffectedTranscript(vo, vat)).ToArray();

        return TsvWriter.Write(entries, map);
    }

    public async Task<string> GetFullCnvsData(IEnumerable<long> ids)
    {
        var query = CreateCnvsQuery(true).FilterByVariantIds(ids);

        var map = new ClassMap<CnvOccurrenceWithAffectedTranscript>().MapVariantOccurrences();

        var entities = await query.ToArrayAsync();
        
        var entries = entities?.SelectMany(vo => vo.Variant.AffectedTranscripts, (vo, vat) => new CnvOccurrenceWithAffectedTranscript(vo, vat)).ToArray();

        return TsvWriter.Write(entries, map);
    }

    public async Task<string> GetFullSvsData(IEnumerable<long> ids)
    {
        var query = CreateSvsQuery(true).FilterByVariantIds(ids);

        var map = new ClassMap<SvOccurrenceWithAffectedTranscript>().MapVariantOccurrences();

        var entities = await query.ToArrayAsync();

        var entries = entities?.SelectMany(vo => vo.Variant.AffectedTranscripts, (vo, vat) => new SvOccurrenceWithAffectedTranscript(vo, vat)).ToArray();

        return TsvWriter.Write(entries, map);
    }

    public async Task<string> GetFullSsmsDataForDonors(IEnumerable<int> ids)
    {
        var specimenIds = await GetSpecimenIdsForDonors(ids);

        return await GetFullSsmsDataForSpecimens(specimenIds);
    }

    public async Task<string> GetFullCnvsDataForDonors(IEnumerable<int> ids)
    {
        var specimenIds = await GetSpecimenIdsForDonors(ids);

        return await GetFullCnvsDataForSpecimens(specimenIds);
    }

    public async Task<string> GetFullSvsDataForDonors(IEnumerable<int> ids)
    {
        var specimenIds = await GetSpecimenIdsForDonors(ids);

        return await GetFullSvsDataForSpecimens(specimenIds);
    }

    public async Task<string> GetFullSsmsDataForImages(IEnumerable<int> ids)
    {
        var specimenIds = await GetSpecimenIdsForImages(ids);

        return await GetFullSsmsDataForSpecimens(specimenIds);
    }

    public async Task<string> GetFullCnvsDataForImages(IEnumerable<int> ids)
    {
        var specimenIds = await GetSpecimenIdsForImages(ids);

        return await GetFullCnvsDataForSpecimens(specimenIds);
    }

    public async Task<string> GetFullSvsDataForImages(IEnumerable<int> ids)
    {
        var specimenIds = await GetSpecimenIdsForImages(ids);

        return await GetFullSvsDataForSpecimens(specimenIds);
    }

    public async Task<string> GetFullSsmsDataForSpecimens(IEnumerable<int> ids)
    {
        var query = CreateSsmsQuery(true).FilterBySpecimenIds(ids);

        var map = new ClassMap<SsmOccurrenceWithAffectedTranscript>().MapVariantOccurrences();

        var entities = await query.ToArrayAsync();

        var entries = entities?.SelectMany(vo => vo.Variant.AffectedTranscripts, (vo, vat) => new SsmOccurrenceWithAffectedTranscript(vo, vat)).ToArray();

        return TsvWriter.Write(entries, map);
    }

    public async Task<string> GetFullCnvsDataForSpecimens(IEnumerable<int> ids)
    {
        var query = CreateCnvsQuery(true).FilterBySpecimenIds(ids);

        var map = new ClassMap<CnvOccurrenceWithAffectedTranscript>().MapVariantOccurrences();

        var entities = await query.ToArrayAsync();

        var entries = entities?.SelectMany(vo => vo.Variant.AffectedTranscripts, (vo, vat) => new CnvOccurrenceWithAffectedTranscript(vo, vat)).ToArray();

        return TsvWriter.Write(entries, map);
    }

    public async Task<string> GetFullSvsDataForSpecimens(IEnumerable<int> ids)
    {
        var query = CreateSvsQuery(true).FilterBySpecimenIds(ids);

        var map = new ClassMap<SvOccurrenceWithAffectedTranscript>().MapVariantOccurrences();

        var entities = await query.ToArrayAsync();

        var entries = entities?.SelectMany(vo => vo.Variant.AffectedTranscripts, (vo, vat) => new SvOccurrenceWithAffectedTranscript(vo, vat)).ToArray();

        return TsvWriter.Write(entries, map);
    }

    public async Task<string> GetFullSsmsDataForGenes(IEnumerable<int> ids)
    {
        var query = CreateSsmsQuery(true).FilterByAffectedGeneIds(ids);

        var map = new ClassMap<SsmOccurrenceWithAffectedTranscript>().MapVariantOccurrences();

        var entities = await query.ToArrayAsync();

        var entries = entities?.SelectMany(vo => vo.Variant.AffectedTranscripts, (vo, vat) => new SsmOccurrenceWithAffectedTranscript(vo, vat)).ToArray();

        return TsvWriter.Write(entries, map);
    }

    public async Task<string> GetFullCnvsDataForGenes(IEnumerable<int> ids)
    {
        var query = CreateCnvsQuery(true).FilterByAffectedGeneIds(ids);

        var map = new ClassMap<CnvOccurrenceWithAffectedTranscript>().MapVariantOccurrences();

        var entities = await query.ToArrayAsync();

        var entries = entities?.SelectMany(vo => vo.Variant.AffectedTranscripts, (vo, vat) => new CnvOccurrenceWithAffectedTranscript(vo, vat)).ToArray();

        return Write(entries, map);
    }

    public async Task<string> GetFullSvsDataForGenes(IEnumerable<int> ids)
    {
        var query = CreateSvsQuery(true).FilterByAffectedGeneIds(ids);

        var map = new ClassMap<SvOccurrenceWithAffectedTranscript>().MapVariantOccurrences();

        var entities = await query.ToArrayAsync();

        var entries = entities?.SelectMany(vo => vo.Variant.AffectedTranscripts, (vo, vat) => new SvOccurrenceWithAffectedTranscript(vo, vat)).ToArray();

        return TsvWriter.Write(entries, map);
    }


    private async Task<int[]> GetSpecimenIdsForDonors(IEnumerable<int> ids)
    {
        return await _dbContext.Set<Specimen>().AsNoTracking()
            .Where(entity => ids.Contains(entity.DonorId))
            .Select(entity => entity.Id)
            .Distinct()
            .ToArrayAsync();
    }

    private async Task<int[]> GetSpecimenIdsForImages(IEnumerable<int> ids)
    { 
        var donorIds = await _dbContext.Set<Image>().AsNoTracking()
            .Where(entity => ids.Contains(entity.Id))
            .Select(entity => entity.DonorId)
            .Distinct()
            .ToArrayAsync();

        return await _dbContext.Set<Specimen>().AsNoTracking()
            .Include(entity => entity.Tissue)
            .Where(entity => entity.Tissue != null && entity.Tissue.TypeId != TissueType.Control)
            .Where(entity => ids.Contains(entity.DonorId))
            .Select(entity => entity.Id)
            .Distinct()
            .ToArrayAsync();
    }


    private IQueryable<SSM.VariantOccurrence> CreateSsmsQuery(bool transcripts = false)
    {
        var query = _dbContext.Set<SSM.VariantOccurrence>().AsNoTracking();

        query = IncludeAffectedFeatures(query, transcripts);
        query = IncludeVariant<SSM.VariantOccurrence, SSM.Variant>(query);
        query = IncludeSamples<SSM.VariantOccurrence, SSM.Variant>(query);
        query = OrderVariant<SSM.VariantOccurrence, SSM.Variant>(query);

        return query;
    }

    private IQueryable<CNV.VariantOccurrence> CreateCnvsQuery(bool transcripts = false)
    {
        var query = _dbContext.Set<CNV.VariantOccurrence>().AsNoTracking();

        query = IncludeAffectedFeatures(query, transcripts);
        query = IncludeVariant<CNV.VariantOccurrence, CNV.Variant>(query);
        query = IncludeSamples<CNV.VariantOccurrence, CNV.Variant>(query);
        query = OrderVariant<CNV.VariantOccurrence, CNV.Variant>(query);

        return query;
    }

    private IQueryable<SV.VariantOccurrence> CreateSvsQuery(bool transcripts = false)
    {
        var query = _dbContext.Set<SV.VariantOccurrence>().AsNoTracking();

        query = IncludeAffectedFeatures(query, transcripts);
        query = IncludeVariant<SV.VariantOccurrence, SV.Variant>(query);
        query = IncludeSamples<SV.VariantOccurrence, SV.Variant>(query);
        query = OrderVariant<SV.VariantOccurrence, SV.Variant>(query);

        return query;
    }


    private static IQueryable<TVO> IncludeVariant<TVO, TV>(IQueryable<TVO> query)
        where TVO : VariantOccurrence<TV>
        where TV : Variant
    {
        return query
            .Include(entity => entity.Variant);
    }

    private static IQueryable<TVO> IncludeSamples<TVO, TV>(IQueryable<TVO> query)
        where TVO : VariantOccurrence<TV>
        where TV : Variant
    {
        return query
            .Include(entity => entity.AnalysedSample.Analysis)
            .Include(entity => entity.AnalysedSample.Sample.Specimen.Donor)
            .Include(entity => entity.AnalysedSample.Sample.Specimen.Tissue)
            .Include(entity => entity.AnalysedSample.Sample.Specimen.CellLine)
            .Include(entity => entity.AnalysedSample.Sample.Specimen.Organoid)
            .Include(entity => entity.AnalysedSample.Sample.Specimen.Xenograft)
            .Include(entity => entity.AnalysedSample.MatchedSample.Specimen.Donor)
            .Include(entity => entity.AnalysedSample.MatchedSample.Specimen.Tissue)
            .Include(entity => entity.AnalysedSample.MatchedSample.Specimen.CellLine)
            .Include(entity => entity.AnalysedSample.MatchedSample.Specimen.Organoid)
            .Include(entity => entity.AnalysedSample.MatchedSample.Specimen.Xenograft);
    }

    private static IQueryable<SSM.VariantOccurrence> IncludeAffectedFeatures(IQueryable<SSM.VariantOccurrence> query, bool transcripts = false)
    {
        IQueryable<SSM.VariantOccurrence> includeQuery = query;

        if (transcripts)
        {
            includeQuery = includeQuery
                .Include(entity => entity.Variant.AffectedTranscripts)
                    .ThenInclude(entity => entity.Feature.Gene)
                .Include(entity => entity.Variant.AffectedTranscripts)
                    .ThenInclude(entity => entity.Feature.Protein);
        }
        
        return includeQuery;
    }

    private static IQueryable<CNV.VariantOccurrence> IncludeAffectedFeatures(IQueryable<CNV.VariantOccurrence> query, bool transcripts = false)
    {
        IQueryable<CNV.VariantOccurrence> includeQuery = query;

        if (transcripts)
        {
            includeQuery = includeQuery
                .Include(entity => entity.Variant.AffectedTranscripts)
                    .ThenInclude(entity => entity.Feature.Gene)
                .Include(entity => entity.Variant.AffectedTranscripts)
                    .ThenInclude(entity => entity.Feature.Protein);
        }
        
        return includeQuery;
    }

    private static IQueryable<SV.VariantOccurrence> IncludeAffectedFeatures(IQueryable<SV.VariantOccurrence> query, bool transcripts = false)
    {
        IQueryable<SV.VariantOccurrence> includeQuery = query;

        if (transcripts)
        {
            includeQuery = includeQuery
                .Include(entity => entity.Variant.AffectedTranscripts)
                    .ThenInclude(entity => entity.Feature.Gene)
                .Include(entity => entity.Variant.AffectedTranscripts)
                    .ThenInclude(entity => entity.Feature.Protein);
        }
        
        return includeQuery;
    }


    private static IQueryable<TVO> OrderVariant<TVO, TV>(IQueryable<TVO> query)
        where TVO : VariantOccurrence<TV>
        where TV : Variant
    {
        return query
            .OrderBy(entity => entity.AnalysedSample.Sample.Specimen.DonorId)
            .ThenBy(entity => entity.Variant.ChromosomeId)
            .ThenBy(entity => entity.Variant.Start);
    }


    private static string Write<T>(IEnumerable<T> entities, ClassMap<T> map)
        where T : class
    {
        return entities?.Any() == true ? TsvWriter.Write(entities, map) : null;
    }
}
