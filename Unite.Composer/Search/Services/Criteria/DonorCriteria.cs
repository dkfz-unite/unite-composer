using Unite.Composer.Search.Services.Criteria.Models;

namespace Unite.Composer.Search.Services.Criteria
{
    public class DonorCriteria
    {
        public string[] ReferenceId { get; set; }

        public string[] Gender { get; set; }
        public Range<double?> Age { get; set; }
        public string[] Diagnosis { get; set; }
        public bool? VitalStatus { get; set; }
    }
}
