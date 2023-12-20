using Microsoft.EntityFrameworkCore;
using Unite.Composer.Download.Tsv.Mapping.Extensions;
using Unite.Data.Context;
using Unite.Data.Entities.Donors;
using Unite.Data.Entities.Donors.Clinical;
using Unite.Data.Entities.Genome.Variants;
using Unite.Essentials.Tsv;

namespace Unite.Composer.Download.Tsv.Mapping;

public class DonorsTsvService : TsvServiceBase
{
    public DonorsTsvService(IDbContextFactory<DomainDbContext> dbContextFactory) : base(dbContextFactory)
    {
    }


    public async Task<string> GetData(IEnumerable<int> ids)
    {
        using var dbContext = _dbContextFactory.CreateDbContext();

        var entities = await CreateDonorsQuery(dbContext)
            .Where(entity => ids.Contains(entity.Id))
            .ToArrayAsync();

        var map = new ClassMap<Donor>().MapDonors();

        return Write(entities, map);
    }

    public async Task<string> GetDataForImages(IEnumerable<int> ids)
    {
        var donorIds = await _imagesRepository.GetRelatedDonors(ids);

        return await GetData(donorIds);
    }

    public async Task<string> GetDataForSpecimens(IEnumerable<int> ids)
    {
        var donorIds = await _specimensRepository.GetRelatedDonors(ids);

        return await GetData(donorIds);
    }

    public async Task<string> GetDataForGenes(IEnumerable<int> ids)
    {
        var donorIds = await _genesRepository.GetRelatedDonors(ids);

        return await GetData(donorIds);
    }

    public async Task<string> GetDataForVariants<TV>(IEnumerable<long> ids)
        where TV : Variant
    {
        var donorIds = await _variantsRepository.GetRelatedDonors<TV>(ids);

        return await GetData(donorIds);
    }


    public async Task<string> GetClinicalData(IEnumerable<int> ids)
    {
        using var dbContext = _dbContextFactory.CreateDbContext();

        var entities = await CreateClinicalDataQuery(dbContext)
            .Where(entity => ids.Contains(entity.DonorId))
            .ToArrayAsync();

        var map = new ClassMap<ClinicalData>().MapClinicalData();

        return Write(entities, map);
    }

    public async Task<string> GetClinicalDataForImages(IEnumerable<int> ids)
    {
        var donorIds = await _imagesRepository.GetRelatedDonors(ids);

        return await GetClinicalData(donorIds);
    }

    public async Task<string> GetClinicalDataForSpecimens(IEnumerable<int> ids)
    {
        var donorIds = await _specimensRepository.GetRelatedDonors(ids);

        return await GetClinicalData(donorIds);
    }

    public async Task<string> GetClinicalDataForGenes(IEnumerable<int> ids)
    {
        var donorIds = await _genesRepository.GetRelatedDonors(ids);

        return await GetClinicalData(donorIds);
    }

    public async Task<string> GetClinicalDataForVariants<TV>(IEnumerable<long> ids)
        where TV : Variant
    {
        var donorIds = await _variantsRepository.GetRelatedDonors<TV>(ids);

        return await GetClinicalData(donorIds);
    }


    public async Task<string> GetTreatmentsData(IEnumerable<int> ids)
    {
        using var dbContext = _dbContextFactory.CreateDbContext();

        var entities = await CreateTreatmentsQuery(dbContext)
            .Where(entity => ids.Contains(entity.Id))
            .ToArrayAsync();

        var map = new ClassMap<Treatment>().MapTreatments();

        return Write(entities, map);
    }

    public async Task<string> GetTreatmentsDataForImages(IEnumerable<int> ids)
    {
        var donorIds = await _imagesRepository.GetRelatedDonors(ids);

        return await GetTreatmentsData(donorIds);
    }

    public async Task<string> GetTreatmentsDataForSpecimens(IEnumerable<int> ids)
    {
        var donorIds = await _specimensRepository.GetRelatedDonors(ids);

        return await GetTreatmentsData(donorIds);
    }

    public async Task<string> GetTreatmentsDataForGenes(IEnumerable<int> ids)
    {
        var donorIds = await _genesRepository.GetRelatedDonors(ids);

        return await GetTreatmentsData(donorIds);
    }

    public async Task<string> GetTreatmentsDataForVariants<TV>(IEnumerable<long> ids)
        where TV : Variant
    {
        var donorIds = await _variantsRepository.GetRelatedDonors<TV>(ids);

        return await GetTreatmentsData(donorIds);
    }


    private static IQueryable<Donor> CreateDonorsQuery(DomainDbContext dbContext)
    {
        return dbContext.Set<Donor>().AsNoTracking()
            .Include(entity => entity.DonorProjects).ThenInclude(entity => entity.Project)
            .Include(entity => entity.DonorStudies).ThenInclude(entity => entity.Study);
    }

    private static IQueryable<ClinicalData> CreateClinicalDataQuery(DomainDbContext dbContext)
    {
        return dbContext.Set<ClinicalData>().AsNoTracking()
            .Include(entity => entity.Donor)
            .Include(entity => entity.PrimarySite)
            .Include(entity => entity.Localization);
    }

    private static IQueryable<Treatment> CreateTreatmentsQuery(DomainDbContext dbContext)
    {
        return dbContext.Set<Treatment>().AsNoTracking()
            .Include(entity => entity.Donor)
            .Include(entity => entity.Therapy);
    }
}
