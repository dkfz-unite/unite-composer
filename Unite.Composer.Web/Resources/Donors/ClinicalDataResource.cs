using System;
using Unite.Indices.Entities.Basic.Clinical;

namespace Unite.Composer.Web.Resources.Donors
{
    public class ClinicalDataResource
    {
		public string Gender { get; set; }
		public int? Age { get; set; }
		public string Diagnosis { get; set; }
		public DateTime? DiagnosisDate { get; set; }
		public string PrimarySite { get; set; }
		public string Localization { get; set; }
		public bool? VitalStatus { get; set; }
		public DateTime? VitalStatusChangeDate { get; set; }
		public int? KpsBaseline { get; set; }
		public bool? SteroidsBaseline { get; set; }

		public ClinicalDataResource(ClinicalDataIndex index)
        {
			Gender = index.Gender;
			Age = index.Age;
			Diagnosis = index.Diagnosis;
			DiagnosisDate = index.DiagnosisDate;
			PrimarySite = index.PrimarySite;
			Localization = index.Localization;
			VitalStatus = index.VitalStatus;
			VitalStatusChangeDate = index.VitalStatusChangeDate;
			KpsBaseline = index.KpsBaseline;
			SteroidsBaseline = index.SteroidsBaseline;
        }
	}
}
