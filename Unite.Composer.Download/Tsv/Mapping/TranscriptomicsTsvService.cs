using Microsoft.EntityFrameworkCore;
using Unite.Composer.Download.Tsv.Mapping.Extensions;
using Unite.Data.Entities.Genome.Transcriptomics;
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

public class TranscriptomicsTsvService
{
    //TODO: Use DbContextFactory per request to allow parallel queries
    private readonly DomainDbContext _dbContext;


    public TranscriptomicsTsvService(IDbContextFactory<DomainDbContext> dbContextFactory)
    {
        _dbContext = dbContextFactory.CreateDbContext();
    }


    public async Task<string> GetTranscriptomicsData(IEnumerable<int> ids, IEnumerable<int> specimenIds = null)
    {
        var map = new ClassMap<GeneExpression>().MapGeneExpressions();

        var entities = await CreateQuery()
            .Where(entity => ids.Contains(entity.GeneId))
            .Where(entity => specimenIds == null || specimenIds.Contains(entity.AnalysedSample.Sample.SpecimenId))
            .ToArrayAsync();

        return Write(entities, map);
    }

    public async Task<string> GetTranscriptomicsDataForSpecimens(IEnumerable<int> ids)
    {
        var map = new ClassMap<GeneExpression>().MapGeneExpressions();

        var entities = await CreateQuery()
            .Where(entity => ids.Contains(entity.AnalysedSample.Sample.SpecimenId))
            .ToArrayAsync();

        return Write(entities, map);
    }

    public async Task<string> GetTranscriptomicsDataForDonors(IEnumerable<int> ids)
    {
        var specimenIds = await GetSpecimenIdsForDonors(ids);

        return await GetTranscriptomicsDataForSpecimens(specimenIds);
    }

    public async Task<string> GetTranscriptomicsDataForImages(IEnumerable<int> ids)
    {
        var specimenIds = await GetSpecimenIdsForImages(ids);

        return await GetTranscriptomicsDataForSpecimens(specimenIds);
    }

    public async Task<string> GetTranscriptomicsDataForVariants<TVO, TV>(IEnumerable<long> ids)
        where TVO : VariantOccurrence<TV>
        where TV : Variant
    {
        if (typeof(TVO) == typeof(SSM.VariantOccurrence))
            return await GetTranscriptomicsDataForSsms(ids.Cast<long>());
        else if (typeof(TVO) == typeof(CNV.VariantOccurrence))
            return await GetTranscriptomicsDataForCnvs(ids.Cast<long>());
        else if (typeof(TVO) == typeof(SV.VariantOccurrence))
            return await GetTranscriptomicsDataForSvs(ids.Cast<long>());
        else
            throw new NotSupportedException("Variant type is not supported.");
    }

    private async Task<string> GetTranscriptomicsDataForSsms(IEnumerable<long> ids)
    {
        var geneIds = await GetGeneIdsForSsms(ids);
        var specimenIds = await GetSpecimenIdsForVariants<SSM.VariantOccurrence, SSM.Variant>(ids);

        return await GetTranscriptomicsData(geneIds, specimenIds);
    }

    private async Task<string> GetTranscriptomicsDataForCnvs(IEnumerable<long> ids)
    {
        var geneIds = await GetGeneIdsForCnvs(ids);
        var specimenIds = await GetSpecimenIdsForVariants<CNV.VariantOccurrence, CNV.Variant>(ids);

        return await GetTranscriptomicsData(geneIds, specimenIds);
    }

    private async Task<string> GetTranscriptomicsDataForSvs(IEnumerable<long> ids)
    {
        var geneIds = await GetGeneIdsForSvs(ids);
        var specimenIds = await GetSpecimenIdsForVariants<SV.VariantOccurrence, SV.Variant>(ids);

        return await GetTranscriptomicsData(geneIds, specimenIds);
    }


    private async Task<int[]> GetSpecimenIdsForDonors(IEnumerable<int> ids)
    {
        return await _dbContext.Set<Specimen>().AsNoTracking()
            .Where(entity => ids.Contains(entity.DonorId))
            .Select(entity => entity.Id)
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

    private async Task<int[]> GetSpecimenIdsForVariants<TVO, TV>(IEnumerable<long> ids)
        where TVO : VariantOccurrence<TV>
        where TV : Variant
    {
        return await _dbContext.Set<TVO>().AsNoTracking()
            .Where(entity => ids.Contains(entity.VariantId))
            .Select(entity => entity.AnalysedSample.Sample.SpecimenId)
            .Distinct()
            .ToArrayAsync();  
    }

    private async Task<int[]> GetGeneIdsForSsms(IEnumerable<long> ids)
    {
        return await _dbContext.Set<SSM.VariantOccurrence>().AsNoTracking()
            .Where(entity => ids.Contains(entity.VariantId))
            .SelectMany(entity => entity.Variant.AffectedTranscripts)
            .Select(entity => entity.Feature.GeneId.Value)
            .Distinct()
            .ToArrayAsync();
    }

    private async Task<int[]> GetGeneIdsForCnvs(IEnumerable<long> ids)
    {
        return await _dbContext.Set<CNV.VariantOccurrence>().AsNoTracking()
            .Where(entity => ids.Contains(entity.VariantId))
            .SelectMany(entity => entity.Variant.AffectedTranscripts)
            .Select(entity => entity.Feature.GeneId.Value)
            .Distinct()
            .ToArrayAsync();
    }

    private async Task<int[]> GetGeneIdsForSvs(IEnumerable<long> ids)
    {
        return await _dbContext.Set<SV.VariantOccurrence>().AsNoTracking()
            .Where(entity => ids.Contains(entity.VariantId))
            .SelectMany(entity => entity.Variant.AffectedTranscripts)
            .Select(entity => entity.Feature.GeneId.Value)
            .Distinct()
            .ToArrayAsync();
    }


    private IQueryable<GeneExpression> CreateQuery()
    {
        return _dbContext.Set<GeneExpression>()
            .Include(entity => entity.Gene)
            .Include(entity => entity.AnalysedSample.Sample.Specimen.Donor)
            .Include(entity => entity.AnalysedSample.Sample.Specimen.Tissue)
            .Include(entity => entity.AnalysedSample.Sample.Specimen.CellLine)
            .Include(entity => entity.AnalysedSample.Sample.Specimen.Organoid)
            .Include(entity => entity.AnalysedSample.Sample.Specimen.Xenograft);
    }

    private static string Write<T>(IEnumerable<T> entities, ClassMap<T> map)
        where T : class
    {
        return entities?.Any() == true ? TsvWriter.Write(entities, map) : null;
    }
}
