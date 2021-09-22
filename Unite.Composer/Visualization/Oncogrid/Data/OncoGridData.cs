using System.Collections.Generic;

namespace Unite.Composer.Visualization.Oncogrid.Data
{
    public class OncoGridData
    {
        /// <summary>
        /// Oncogrid donors data 
        /// </summary>
        public IEnumerable<OncoGridDonor> Donors { get; set; }

        /// <summary>
        /// Oncogrid genes data
        /// </summary>
        public IEnumerable<OncoGridGene> Genes { get; set; }

        /// <summary>
        /// Oncogrid observations data
        /// Each reference of <see cref="OncoGridMutation.DonorId"/> requires the existence of an entry within
        /// <see cref="Donors"/> with an equal <see cref="OncoGridDonor.Id"/>.
        /// Each reference of <see cref="OncoGridMutation.GeneId"/> requires the existence of an entry within
        /// <see cref="Genes"/> with with an equal <see cref="OncoGridGene.Id"/>.
        /// </summary>
        public IEnumerable<OncoGridMutation> Observations { get; set; }
    }
}
