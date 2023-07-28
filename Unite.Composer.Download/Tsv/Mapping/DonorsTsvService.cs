using Microsoft.EntityFrameworkCore;
using Unite.Composer.Download.Tsv.Mapping.Extensions;
using Unite.Data.Entities.Donors;
using Unite.Data.Entities.Donors.Clinical;
using Unite.Data.Entities.Genome.Variants;
using Unite.Data.Entities.Images;
using Unite.Data.Entities.Specimens;
using Unite.Data.Services;
using Unite.Essentials.Tsv;

namespace Unite.Composer.Download.Tsv.Mapping;

public class DonorsTsvService : TsvServiceBase
{
    //TODO: Use DbContextFactory per request to allow parallel queries
    private readonly DomainDbContext _dbContext;


    public DonorsTsvService(IDbContextFactory<DomainDbContext> dbContextFactory) : base(dbContextFactory)
    {
        _dbContext = dbContextFactory.CreateDbContext();
    }


    public async Task<string> GetDonorsData(IEnumerable<int> ids)
    {
        var entities = await CreateDonorsQuery()
            .Where(entity => ids.Contains(entity.Id))
            .ToArrayAsync();

        if (!entities.Any())
        {
            return null;
        }

        var map = new ClassMap<Donor>().MapDonors();

        return TsvWriter.Write(entities, map);
    }

    public async Task<string> GetDonorsDataForImages(IEnumerable<int> ids)
    {
        var donorIds = await GetDonorIdsForImages(ids);

        return await GetDonorsData(donorIds);
    }

    public async Task<string> GetDonorsDataForSpecimens(IEnumerable<int> ids)
    {
        var donorIds = await GetDonorIdsForSpecimens(ids);

        return await GetDonorsData(donorIds);
    }

    public async Task<string> GetDonorsDataForGenes(IEnumerable<int> ids)
    {
        var donorIds = await GetDonorIdsForGenes(ids);

        return await GetDonorsData(donorIds);
    }

    public async Task<string> GetDonorsForVariants<TVO, TV>(IEnumerable<long> ids)
        where TVO : VariantOccurrence<TV>
        where TV : Variant
    {
        var donorIds = await GetDonorIdsForVariants<TVO, TV>(ids);

        return await GetDonorsData(donorIds);
    }


    public async Task<string> GetClinicalData(IEnumerable<int> ids)
    {
        var entities = await CreateClinicalDataQuery()
            .Where(entity => ids.Contains(entity.DonorId))
            .ToArrayAsync();

        if (!entities.Any())
        {
            return null;
        }

        var map = new ClassMap<ClinicalData>().MapClinicalData();

        return TsvWriter.Write(entities, map);
    }

    public async Task<string> GetClinicalDataForImages(IEnumerable<int> ids)
    {
        var donorIds = await GetDonorIdsForImages(ids);

        return await GetClinicalData(donorIds);
    }

    public async Task<string> GetClinicalDataForSpecimens(IEnumerable<int> ids)
    {
        var donorIds = await GetDonorIdsForSpecimens(ids);

        return await GetClinicalData(donorIds);
    }

    public async Task<string> GetClinicalDataForGenes(IEnumerable<int> ids)
    {
        var donorIds = await GetDonorIdsForGenes(ids);

        return await GetClinicalData(donorIds);
    }

    public async Task<string> GetClinicalDataForVariants<TVO, TV>(IEnumerable<long> ids)
        where TVO : VariantOccurrence<TV>
        where TV : Variant
    {
        var donorIds = await GetDonorIdsForVariants<TVO, TV>(ids);

        return await GetClinicalData(donorIds);
    }


    public async Task<string> GetTreatmentsData(IEnumerable<int> ids)
    {
        var entities = await CreateTreatmentsQuery()
            .Where(entity => ids.Contains(entity.Id))
            .ToArrayAsync();

        if (!entities.Any())
        {
            return null;
        }

        var map = new ClassMap<Treatment>().MapTreatments();

        return TsvWriter.Write(entities, map);
    }

    public async Task<string> GetTreatmentsDataForImages(IEnumerable<int> ids)
    {
        var donorIds = await GetDonorIdsForImages(ids);

        return await GetTreatmentsData(donorIds);
    }

    public async Task<string> GetTreatmentsDataForSpecimens(IEnumerable<int> ids)
    {
        var donorIds = await GetDonorIdsForSpecimens(ids);

        return await GetTreatmentsData(donorIds);
    }

    public async Task<string> GetTreatmentsDataForGenes(IEnumerable<int> ids)
    {
        var donorIds = await GetDonorIdsForGenes(ids);

        return await GetTreatmentsData(donorIds);
    }

    public async Task<string> GetTreatmentsDataForVariants<TVO, TV>(IEnumerable<long> ids)
        where TVO : VariantOccurrence<TV>
        where TV : Variant
    {
        var donorIds = await GetDonorIdsForVariants<TVO, TV>(ids);

        return await GetTreatmentsData(donorIds);
    }


    private async Task<int[]> GetDonorIdsForImages(IEnumerable<int> ids)
    {
        return await _dbContext.Set<Image>().AsNoTracking()
            .Where(entity => ids.Contains(entity.Id))
            .Select(entity => entity.DonorId)
            .Distinct()
            .ToArrayAsync();
    }

    private async Task<int[]> GetDonorIdsForSpecimens(IEnumerable<int> ids)
    {
        return await _dbContext.Set<Specimen>().AsNoTracking()
            .Where(entity => ids.Contains(entity.Id))
            .Select(entity => entity.DonorId)
            .Distinct()
            .ToArrayAsync();
    }


    private IQueryable<Donor> CreateDonorsQuery()
    {
        return _dbContext.Set<Donor>().AsNoTracking()
            .Include(entity => entity.DonorProjects).ThenInclude(entity => entity.Project)
            .Include(entity => entity.DonorStudies).ThenInclude(entity => entity.Study);
    }

    private IQueryable<ClinicalData> CreateClinicalDataQuery()
    {
        return _dbContext.Set<ClinicalData>().AsNoTracking()
            .Include(entity => entity.Donor)
            .Include(entity => entity.PrimarySite)
            .Include(entity => entity.Localization);
    }

    private IQueryable<Treatment> CreateTreatmentsQuery()
    {
        return _dbContext.Set<Treatment>().AsNoTracking()
            .Include(entity => entity.Donor)
            .Include(entity => entity.Therapy);
    }
}
