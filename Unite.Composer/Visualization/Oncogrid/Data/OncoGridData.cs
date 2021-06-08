using System.Collections.Generic;

namespace Unite.Composer.Visualization.Oncogrid.Data
{
    /// <summary>
    /// This class wraps up all information required for the javascript oncogrid framework:
    /// https://github.com/oncojs/oncogrid
    /// </summary>
    public class OncoGridData
    {
        /// <summary>
        /// Each entry represents a column within the oncogrid.
        /// Each reference of <see cref="ObservationData.DonorId"/> requires the existence of an entry within
        /// <see cref="Donors"/> with an equal <see cref="OncoGridDonorData.Id"/>.
        /// Each reference of <see cref="ObservationData.GeneId"/> requires the existence of an entry within
        /// <see cref="Genes"/> with with an equal <see cref="OncoGridGeneData.Id"/>.
        /// </summary>
        public List<ObservationData> Observations { get; } = new List<ObservationData>();

        /// <summary>
        /// Gene metadata for the oncogrid 
        /// </summary>
        public List<OncoGridGeneData> Genes { get; } = new List<OncoGridGeneData>();

        /// <summary>
        /// Donor metadata for the oncogrid 
        /// </summary>
        public List<OncoGridDonorData> Donors { get; } = new List<OncoGridDonorData>();
    }
}