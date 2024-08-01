using Unite.Indices.Entities.Basic.Donors;

namespace Unite.Composer.Visualization.Oncogrid.Data;

public class OncoGridDonor
{
    public string Id { get; }
    public string DisplayId { get; }

    public string Diagnosis { get; }
    public string PrimarySite { get; }
    public string Localization { get; }
    public string Gender { get; }
    public int? Age { get; }
    public bool? VitalStatus { get; }
    public int? VitalStatusChangeDay { get; }
    public bool? ProgressionStatus { get; }
    public int? ProgressionStatusChangeDay { get; }
    public int? KpsBaseline { get; }
    public bool? SteroidsBaseline { get; }


    public OncoGridDonor(DonorIndex index)
    {
        Id = index.Id.ToString();
        DisplayId = index.ReferenceId;

        Diagnosis = index.ClinicalData?.Diagnosis;
        PrimarySite = index.ClinicalData?.PrimarySite;
        Localization = index.ClinicalData?.Localization;
        Gender = index.ClinicalData?.Gender;
        Age = index.ClinicalData?.Age;
        VitalStatus = index.ClinicalData?.VitalStatus;
        VitalStatusChangeDay = index.ClinicalData?.VitalStatusChangeDay;
        ProgressionStatus = index.ClinicalData?.ProgressionStatus;
        ProgressionStatusChangeDay = index.ClinicalData?.ProgressionStatusChangeDay;
        KpsBaseline = index.ClinicalData?.KpsBaseline;
        SteroidsBaseline = index.ClinicalData?.SteroidsBaseline;
    }
}
