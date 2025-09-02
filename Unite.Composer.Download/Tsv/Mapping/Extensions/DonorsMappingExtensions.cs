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
            .Map(entity => entity.ReferenceId, "id")
            .Map(entity => entity.MtaProtected, "mta")
            .Map(entity => entity.DonorProjects, "projects", projectDonorConverter)
            .Map(entity => entity.DonorStudies, "studies", studyDonorConverter)
            .MapClinicalData();
    }

    public static ClassMap<Donor> MapClinicalData(this ClassMap<Donor> map)
    {
        return map
            .Map(entity => entity.ClinicalData.SexId, "sex")
            .Map(entity => entity.ClinicalData.EnrollmentDate, "enrollment_date")
            .Map(entity => entity.ClinicalData.EnrollmentAge, "enrollment_age")
            .Map(entity => entity.ClinicalData.Diagnosis, "diagnosis")
            .Map(entity => entity.ClinicalData.PrimarySite.Value, "primary_site")
            .Map(entity => entity.ClinicalData.Localization.Value, "localization")
            .Map(entity => entity.ClinicalData.VitalStatus, "vital_status")
            .Map(entity => entity.ClinicalData.VitalStatusChangeDate, "vital_status_change_date")
            .Map(entity => entity.ClinicalData.VitalStatusChangeDay, "vital_status_change_day")
            .Map(entity => entity.ClinicalData.ProgressionStatus, "progression_status")
            .Map(entity => entity.ClinicalData.ProgressionStatusChangeDate, "progression_status_change_date")
            .Map(entity => entity.ClinicalData.ProgressionStatusChangeDay, "progression_status_change_day")
            .Map(entity => entity.ClinicalData.SteroidsReactive, "steroids_reactive")
            .Map(entity => entity.ClinicalData.Kps, "kps");
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
