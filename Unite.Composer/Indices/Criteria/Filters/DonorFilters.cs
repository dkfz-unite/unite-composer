using Unite.Composer.Indices.Criteria.Filters.Standard;

namespace Unite.Composer.Indices.Criteria.Filters
{
    public class DonorFilters
    {
        public string[] ReferenceId { get; set; }

        public string[] Gender { get; set; }
        public Range Age { get; set; }
        public string[] Diagnosis { get; set; }
        public bool? VitalStatus { get; set; }
    }
}
