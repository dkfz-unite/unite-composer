using System.Collections.Generic;

namespace Unite.Composer.Visualization.Oncogrid.Data
{
    public class OncoGridData
    {
        /// <summary>
        /// Donors metadata for the oncogrid 
        /// </summary>
        public IEnumerable<OncoGridDonor> Donors { get; set; }

        /// <summary>
        /// Genes metadata for the oncogrid 
        /// </summary>
        public IEnumerable<OncoGridGene> Genes { get; set; }

        /// <summary>
        /// Each entry represents a column within the oncogrid.
        /// Each reference of <see cref="OncoGridMutation.DonorId"/> requires the existence of an entry within
        /// <see cref="Donors"/> with an equal <see cref="OncoGridDonor.Id"/>.
        /// Each reference of <see cref="OncoGridMutation.GeneId"/> requires the existence of an entry within
        /// <see cref="Genes"/> with with an equal <see cref="OncoGridGene.Id"/>.
        /// </summary>
        public IEnumerable<OncoGridMutation> Observations { get; set; }


        //public OncoGridData()
        //{
        //    Donors = new List<OncoGridDonorData>();
        //    Genes = new List<OncoGridGeneData>();
        //    Observations = new List<OncoGridMutationData>();
        //}
    }
}