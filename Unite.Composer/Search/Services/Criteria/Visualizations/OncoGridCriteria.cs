namespace Unite.Composer.Search.Services.Criteria.Visualizations
{
    public class OncoGridCriteria
    {
        /// <summary>
        /// Number of donors with highest number of mutations
        /// </summary>
        public int NumberOfDonors { get; set; }

        /// <summary>
        /// Number of genes with highest frequency of expression
        /// </summary>
        public int NumberOfGenes { get; set; }

        public OncoGridCriteria()
        {
            NumberOfDonors = 200;
            NumberOfGenes = 50;
        }
    }
}