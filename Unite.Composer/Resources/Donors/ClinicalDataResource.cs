using System;
using Unite.Indices.Entities;

namespace Unite.Composer.Resources.Donors
{
    public class ClinicalDataResource
    {
		public string Gender { get; set; }
		public int? Age { get; set; }
		public string AgeCategory { get; set; }
		public string Localization { get; set; }
		public string VitalStatus { get; set; }
		public DateTime? VitalStatusChangeDate { get; set; }
		public int? SurvivalDays { get; set; }
		public DateTime? ProgressionDate { get; set; }
		public int? ProgressionFreeDays { get; set; }
		public DateTime? RelapseDate { get; set; }
		public int? RelapseFreeDays { get; set; }
		public int? KpsBaseline { get; set; }
		public bool? SteroidsBaseline { get; set; }

		public ClinicalDataResource(ClinicalDataIndex index)
        {
			Gender = index.Gender;
			Age = index.Age;
			AgeCategory = index.AgeCategory;
			Localization = index.Localization;
			VitalStatus = index.VitalStatus;
			VitalStatusChangeDate = index.VitalStatusChangeDate;
			SurvivalDays = index.SurvivalDays;
			ProgressionDate = index.ProgressionDate;
			ProgressionFreeDays = index.ProgressionFreeDays;
			RelapseDate = index.RelapseDate;
			RelapseFreeDays = index.RelapseFreeDays;
			KpsBaseline = index.KpsBaseline;
			SteroidsBaseline = index.SteroidsBaseline;
        }

	}
}
