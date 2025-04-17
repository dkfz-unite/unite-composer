using System.Linq.Expressions;
using Unite.Composer.Download.Tsv.Mapping.Converters;
using Unite.Data.Entities.Donors;
using Unite.Data.Entities.Donors.Clinical;
using Unite.Essentials.Extensions;
using Unite.Essentials.Tsv;

namespace Unite.Composer.Download.Tsv.Mapping.Extensions;

internal static class DonorsMappingExtensions
{
    public static ClassMap<Donor> MapDonors(this ClassMap<Donor> map)
    {
        var projectDonorConverter = new ProjectDonorConverter();
        var studyDonorConverter = new StudyDonorConverter();

        return map
            .Map(entity => entity.ReferenceId, "donor_id")
            .Map(entity => entity.MtaProtected, "mta")
            .Map(entity => entity.DonorProjects, "projects", projectDonorConverter)
            .Map(entity => entity.DonorStudies, "studies", studyDonorConverter);
    }

    public static ClassMap<ClinicalData> MapClinicalData(this ClassMap<ClinicalData> map)
    {
        return map
            .MapDonor(entity => entity.Donor)
            .Map(entity => entity.SexId, "sex")
            .Map(entity => entity.EnrollmentDate, "enrollment_date")
            .Map(entity => entity.EnrollmentAge, "enrollment_age")
            .Map(entity => entity.Diagnosis, "diagnosis")
            .Map(entity => entity.PrimarySite.Value, "primary_site")
            .Map(entity => entity.Localization.Value, "localization")
            .Map(entity => entity.VitalStatus, "vital_status")
            .Map(entity => entity.VitalStatusChangeDate, "vital_status_change_date")
            .Map(entity => entity.VitalStatusChangeDay, "vital_status_change_day")
            .Map(entity => entity.ProgressionStatus, "progression_status")
            .Map(entity => entity.ProgressionStatusChangeDate, "progression_status_change_date")
            .Map(entity => entity.ProgressionStatusChangeDay, "progression_status_change_day")
            .Map(entity => entity.SteroidsReactive, "steroids_reactive")
            .Map(entity => entity.Kps, "kps");
    }

    public static ClassMap<Treatment> MapTreatments(this ClassMap<Treatment> map)
    {
        return map
            .MapDonor(entity => entity.Donor)
            .Map(entity => entity.Therapy.Name, "therapy")
            .Map(entity => entity.Details, "details")
            .Map(entity => entity.StartDate, "start_date")
            .Map(entity => entity.StartDay, "start_day")
            .Map(entity => entity.EndDate, "end_date")
            .Map(entity => entity.DurationDays, "duration_days")
            .Map(entity => entity.Results, "results");
    }


    private static ClassMap<T> MapDonor<T>(this ClassMap<T> map, Expression<Func<T, Donor>> path) where T : class
    {
        return map
            .Map(path.Join(entity => entity.ReferenceId), "donor_id");
    }
}
