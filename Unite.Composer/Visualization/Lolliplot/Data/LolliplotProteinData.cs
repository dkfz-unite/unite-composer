
namespace Unite.Composer.Visualization.Lolliplot.Data
{
    /// <summary>
    /// A Protein represents a range of the gene within the lolliplot.
    /// 
    /// Attention: Do not rename the attributes because the name is predefined within the used javascript framework for lolliplot:
    /// https://github.com/oncojs/lolliplot
    /// </summary>
    public class LolliplotProteinData
    {
        /// <summary>
        /// Unique Id of the protein 
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Start-Codon of the Protein within the Gene
        /// </summary>
        public int Start { get; set; }

        /// <summary>
        /// End-Codon of the Protein within the Gene
        /// </summary>
        public int End { get; set; }

        /// <summary>
        /// Description of the protein
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The color of the entry/dot which is displayed in the lolliplot
        /// </summary>
        public string GetProteinColor { get; set; }
    }
}