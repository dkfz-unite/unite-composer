using Unite.Indices.Entities.Mutations;

namespace Unite.Composer.Visualization.Lolliplot.Data
{
    /// <summary>
    /// A Mutation represents a "dot", the actual entry, within the lolliplot.
    /// 
    /// Attention: Do not rename the attributes because the name is predefined within the used javascript framework for lolliplot:
    /// https://github.com/oncojs/lolliplot
    /// </summary>
    public class LolliplotMutationData
    {
        /// <summary>
        /// Unique Id for this mutation 
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Position inside of the protein
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// Number of donors affected by this mutation
        /// </summary>
        public int Y { get; set; }

        /// <summary>
        /// Consequence of the mutation <see cref="ConsequenceResource.Type"/>
        /// </summary>
        public string Consequence { get; set; }

        /// <summary>
        /// Severity of the mutation <see cref="ConsequenceIndex.Severity"/>
        /// </summary>
        public string Impact { get; set; }
        
        /// <summary>
        /// How many donors have this mutation <see cref="MutationResource.Donors"/>
        /// </summary>
        public int Donors { get; set; }
    }
}