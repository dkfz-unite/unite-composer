using Unite.Indices.Entities.Basic.Donors;

namespace Unite.Composer.Web.Resources.Search.Basic.Donors;

public class ClinicalDataResource
{
    public string Gender { get; set; }
    public int? Age { get; set; }
    public string Diagnosis { get; set; }
    public string PrimarySite { get; set; }
    public string Localization { get; set; }
    public bool? VitalStatus { get; set; }
    public int? VitalStatusChangeDay { get; set; }
    public bool? ProgressionStatus { get; set; }
    public int? ProgressionStatusChangeDay { get; set; }
    public int? KpsBaseline { get; set; }
    public bool? SteroidsBaseline { get; set; }


    public ClinicalDataResource(ClinicalDataIndex index)
    {
        Gender = index.Gender;
        Age = index.Age;
        Diagnosis = index.Diagnosis;
        PrimarySite = index.PrimarySite;
        Localization = index.Localization;
        VitalStatus = index.VitalStatus;
        VitalStatusChangeDay = index.VitalStatusChangeDay;
        ProgressionStatus = index.ProgressionStatus;
        ProgressionStatusChangeDay = index.ProgressionStatusChangeDay;
        KpsBaseline = index.KpsBaseline;
        SteroidsBaseline = index.SteroidsBaseline;
    }
}
