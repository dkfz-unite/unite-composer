using Unite.Indices.Entities.Basic.Donors;

namespace Unite.Composer.Visualization.Oncogrid.Data;

public class OncoGridDonor
{
    public string Id { get; }
    public string DisplayId { get; }

    public string Diagnosis { get; }
    public string PrimarySite { get; }
    public string Localization { get; }
    public string Sex { get; }
    public int? Age { get; }
    public bool? VitalStatus { get; }
    public int? VitalStatusChangeDay { get; }
    public bool? ProgressionStatus { get; }
    public int? ProgressionStatusChangeDay { get; }
    public bool? SteroidsReactive { get; }
    public int? Kps { get; }


    public OncoGridDonor(DonorIndex index)
    {
        Id = index.Id.ToString();
        DisplayId = index.ReferenceId;

        Diagnosis = index.ClinicalData?.Diagnosis;
        PrimarySite = index.ClinicalData?.PrimarySite;
        Localization = index.ClinicalData?.Localization;
        Sex = index.ClinicalData?.Sex;
        Age = index.ClinicalData?.Age;
        VitalStatus = index.ClinicalData?.VitalStatus;
        VitalStatusChangeDay = index.ClinicalData?.VitalStatusChangeDay;
        ProgressionStatus = index.ClinicalData?.ProgressionStatus;
        ProgressionStatusChangeDay = index.ClinicalData?.ProgressionStatusChangeDay;
        SteroidsReactive = index.ClinicalData?.SteroidsReactive;
        Kps = index.ClinicalData?.Kps;
    }
}
