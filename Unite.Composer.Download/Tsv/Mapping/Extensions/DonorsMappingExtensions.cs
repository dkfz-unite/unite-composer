using System.Linq.Expressions;
using Unite.Composer.Download.Extensions;
using Unite.Composer.Download.Tsv.Mapping.Converters;
using Unite.Data.Entities.Donors;
using Unite.Data.Entities.Donors.Clinical;
using Unite.Essentials.Tsv;

namespace Unite.Composer.Download.Tsv.Mapping.Extensions;

internal static class DonorsMappingExtensions
{
    internal static ClassMap<Donor> MapDonors(this ClassMap<Donor> map)
    {
        var projectDonorConverter = new ProjectDonorConverter();
        var studyDonorConverter = new StudyDonorConverter();

        return map
            .Map(entity => entity.ReferenceId, "donor_id")
            .Map(entity => entity.MtaProtected, "mta")
            .Map(entity => entity.DonorProjects, "projects", projectDonorConverter)
            .Map(entity => entity.DonorStudies, "studies", studyDonorConverter);
    }

    internal static ClassMap<ClinicalData> MapClinicalData(this ClassMap<ClinicalData> map)
    {
        return map
            .MapDonor(entity => entity.Donor)
            .Map(entity => entity.GenderId, "sex")
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

    internal static ClassMap<Treatment> MapTreatments(this ClassMap<Treatment> map)
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
