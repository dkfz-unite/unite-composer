using Unite.Indices.Entities.Basic.Donors;

namespace Unite.Composer.Web.Resources.Domain.Basic.Donors;

public class ClinicalDataResource
{
    public string Sex { get; set; }
    public int? Age { get; set; }
    public string Diagnosis { get; set; }
    public string PrimarySite { get; set; }
    public string Localization { get; set; }
    public bool? VitalStatus { get; set; }
    public int? VitalStatusChangeDay { get; set; }
    public bool? ProgressionStatus { get; set; }
    public int? ProgressionStatusChangeDay { get; set; }
    public bool? SteroidsReactive { get; set; }
    public int? Kps { get; set; }


    public ClinicalDataResource(ClinicalDataIndex index)
    {
        Sex = index.Sex;
        Age = index.Age;
        Diagnosis = index.Diagnosis;
        PrimarySite = index.PrimarySite;
        Localization = index.Localization;
        VitalStatus = index.VitalStatus;
        VitalStatusChangeDay = index.VitalStatusChangeDay;
        ProgressionStatus = index.ProgressionStatus;
        ProgressionStatusChangeDay = index.ProgressionStatusChangeDay;
        SteroidsReactive = index.SteroidsReactive;
        Kps = index.Kps;
    }
}
