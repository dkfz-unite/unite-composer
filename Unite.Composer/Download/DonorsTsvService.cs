using Microsoft.EntityFrameworkCore;
using Unite.Composer.Download.Converters;
using Unite.Data.Entities.Donors;
using Unite.Data.Entities.Donors.Clinical;
using Unite.Data.Entities.Images;
using Unite.Data.Entities.Specimens;
using Unite.Data.Services;
using Unite.Essentials.Tsv;

namespace Unite.Composer.Download;

public class DonorsTsvService
{
    private readonly DomainDbContext _dbContext;

    public DonorsTsvService(IDbContextFactory<DomainDbContext> dbContextFactory)
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

        var map = CreateDonorsMap();

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


    public async Task<string> GetClinicalData(IEnumerable<int> ids)
    {
        var entities = await CreateClinicalDataQuery()
            .Where(entity => ids.Contains(entity.DonorId))
            .ToArrayAsync();

        if (!entities.Any())
        {
            return null;
        }

        var map = CreateClinicalDataMap();

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


    public async Task<string> GetTreatmentsData(IEnumerable<int> ids)
    {
        var entities = await CreateTreatmentsQuery()
            .Where(entity => ids.Contains(entity.Id))
            .ToArrayAsync();

        if (!entities.Any())
        {
            return null;
        }

        var map = CreateTreatmentsMap();

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


    private static ClassMap<Donor> CreateDonorsMap()
    {
        var projectDonorConverter = new ProjectDonorConverter();
        var studyDonorConverter = new StudyDonorConverter();

        return new ClassMap<Donor>()
            .Map(entity => entity.ReferenceId, "donor_id")
            .Map(entity => entity.MtaProtected, "mta")
            .Map(entity => entity.DonorProjects, "projects", projectDonorConverter)
            .Map(entity => entity.DonorStudies, "studies", studyDonorConverter);
    }

    private static ClassMap<ClinicalData> CreateClinicalDataMap()
    {
        return new ClassMap<ClinicalData>()
            .Map(entity => entity.Donor.ReferenceId, "donor_id")
            .Map(entity => entity.GenderId, "gender")
            .Map(entity => entity.Age, "age")
            .Map(entity => entity.Diagnosis, "diagnosis")
            .Map(entity => entity.DiagnosisDate, "diagnosis_date")
            .Map(entity => entity.PrimarySite.Value, "primary_site")
            .Map(entity => entity.Localization.Value, "localization")
            .Map(entity => entity.VitalStatus, "vital_status")
            .Map(entity => entity.VitalStatusChangeDate, "vital_status_change_date")
            .Map(entity => entity.VitalStatusChangeDay, "vital_status_change_day")
            .Map(entity => entity.ProgressionStatus, "progression_status")
            .Map(entity => entity.ProgressionStatusChangeDate, "progression_status_change_date")
            .Map(entity => entity.ProgressionStatusChangeDay, "progression_status_change_day")
            .Map(entity => entity.KpsBaseline, "kps_baseline")
            .Map(entity => entity.SteroidsBaseline, "steroids_baseline");
    }

    private static ClassMap<Treatment> CreateTreatmentsMap()
    {
        return new ClassMap<Treatment>()
            .Map(entity => entity.Donor.ReferenceId, "donor_id")
            .Map(entity => entity.Therapy.Name, "therapy")
            .Map(entity => entity.Details, "details")
            .Map(entity => entity.StartDate, "start_date")
            .Map(entity => entity.StartDay, "start_day")
            .Map(entity => entity.EndDate, "end_date")
            .Map(entity => entity.DurationDays, "duration_days")
            .Map(entity => entity.Results, "results");
    }
}
