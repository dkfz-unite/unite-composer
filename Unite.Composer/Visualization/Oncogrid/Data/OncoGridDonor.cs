using Unite.Indices.Entities.Basic.Donors;

namespace Unite.Composer.Visualization.Oncogrid.Data
{
    public class OncoGridDonor
    {
        public string Id { get; }
        public string DisplayId { get; }

        public string Gender { get; }
        public int? Age { get; }
        public bool? VitalStatus { get; }


        public OncoGridDonor(DonorIndex index)
        {
            Id = index.Id.ToString();
            DisplayId = index.ReferenceId;

            Gender = index.ClinicalData?.Gender;
            Age = index.ClinicalData?.Age;
            VitalStatus = index.ClinicalData?.VitalStatus;
        }
    }
}
