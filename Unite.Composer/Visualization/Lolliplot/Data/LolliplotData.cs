using System.Collections.Generic;

namespace Unite.Composer.Visualization.Lolliplot.Data
{
    /// <summary>
    /// This class wraps up all information required for the javascript Lolliplot framework:
    /// https://github.com/oncojs/lolliplot
    /// </summary>
    public class LolliplotData
    {
        /// <summary>
        /// This is the protein definition for the lolliplot. More than one protein can be found within a gene.
        /// This List should define all available Proteins within the affected Gene of the mutation.
        /// </summary>
        public List<LolliplotProteinData> Proteins { get; } = new List<LolliplotProteinData>();

        /// <summary>
        /// Mutation metadata for the lolliplot 
        /// </summary>
        public List<LolliplotMutationData> Mutations { get; } = new List<LolliplotMutationData>();
    }
}
