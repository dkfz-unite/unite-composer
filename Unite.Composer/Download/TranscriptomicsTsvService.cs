using Microsoft.EntityFrameworkCore;
using Unite.Data.Entities.Genome.Transcriptomics;
using Unite.Data.Entities.Images;
using Unite.Data.Entities.Specimens;
using Unite.Data.Entities.Specimens.Tissues.Enums;
using Unite.Data.Services;
using Unite.Essentials.Tsv;

namespace Unite.Composer.Download;

public class TranscriptomicsTsvService
{
    private readonly DomainDbContext _dbContext;


    public TranscriptomicsTsvService(IDbContextFactory<DomainDbContext> dbContextFactory)
    {
        _dbContext = dbContextFactory.CreateDbContext();
    }


    public async Task<string> GetTranscriptomicsData(IEnumerable<int> ids)
    {
        var entities = await CreateQuery()
            .Where(entity => ids.Contains(entity.AnalysedSample.Sample.SpecimenId))
            .ToArrayAsync();

        if (!entities.Any())
        {
            return null;
        }

        var map = CreateMap();

        return TsvWriter.Write(entities, map);
    }

    public async Task<string> GetTranscriptomicsDataForDonors(IEnumerable<int> ids)
    {
        var specimenIds = await GetSpecimenIdsForDonors(ids);

        return await GetTranscriptomicsData(specimenIds);
    }

    public async Task<string> GetTranscriptomicsDataForImages(IEnumerable<int> ids)
    {
        var specimenIds = await GetSpecimenIdsForImages(ids);

        return await GetTranscriptomicsData(specimenIds);
    }


    private async Task<int[]> GetSpecimenIdsForDonors(IEnumerable<int> donors)
    {
        return await _dbContext.Set<Specimen>().AsNoTracking()
            .Where(entity => donors.Contains(entity.DonorId))
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


    private static ClassMap<GeneExpression> CreateMap()
    {
        return new ClassMap<GeneExpression>()
            .Map(entity => entity.AnalysedSample.Sample.Specimen.Donor.ReferenceId, "donor_id")
            .Map(entity => entity.AnalysedSample.Sample.Specimen.ReferenceId, "specimen_id")
            .Map(entity => entity.AnalysedSample.Sample.Specimen.Type, "specimen_type")
            .Map(entity => entity.AnalysedSample.Sample.ReferenceId, "sample_id")
            .Map(entity => entity.Gene.StableId, "gene_id")
            .Map(entity => entity.Gene.Symbol, "gene_symbol")
            .Map(entity => entity.Reads, "reads")
            .Map(entity => entity.TPM, "tpm")
            .Map(entity => entity.FPKM, "fpkm");
    }
}
