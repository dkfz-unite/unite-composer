using Unite.Composer.Indices.Criteria.Filters.Standard;

namespace Unite.Composer.Indices.Criteria.Filters
{
    public class DonorFilters : EpigeneticsDataFilters
    {
        public string[] Id { get; set; }
        public string[] Diagnosis { get; set; }

        public string[] Gender { get; set; }
        public Range Age { get;  set; }
        public string[] AgeCategory { get; set; }
        public string[] VitalStatus { get; set; }
    }
}
