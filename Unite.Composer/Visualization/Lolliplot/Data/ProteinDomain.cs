namespace Unite.Composer.Visualization.Lolliplot.Data
{
    /// <summary>
    /// Represents protein domain data.
    /// </summary>
    public class ProteinDomain
    {
        /// <summary>
        /// Protein domain id 
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Protein domain symbol
        /// </summary>
        public string Symbol { get; set; }

        /// <summary>
        /// Protein domain description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Protein domain start position
        /// </summary>
        public int Start { get; set; }

        /// <summary>
        /// Protein domain end position
        /// </summary>
        public int End { get; set; }
    }
}
