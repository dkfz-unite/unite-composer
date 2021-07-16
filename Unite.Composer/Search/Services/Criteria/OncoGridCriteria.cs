namespace Unite.Composer.Search.Services.Criteria
{
    public class OncoGridCriteria
    {
        public int MostAffectedDonorCount { get; set; }
        public int MostAffectedGeneCount { get; set; }

        public OncoGridCriteria()
        {
            MostAffectedDonorCount = 200;
            MostAffectedGeneCount = 50;
        }
    }
}