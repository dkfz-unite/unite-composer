using Unite.Composer.Search.Services.Criteria.Models;

namespace Unite.Composer.Search.Services.Criteria
{
    public class DonorCriteria
    {
        public int[] Id { get; set; }
        public string[] ReferenceId { get; set; }

        public string[] Gender { get; set; }
        public Range<double?> Age { get; set; }
        public string[] Diagnosis { get; set; }
        public bool? VitalStatus { get; set; }

        public string[] Therapy { get; set; }

        public bool? MtaProtected { get; set; }
        public string[] WorkPackage { get; set; }
    }
}
