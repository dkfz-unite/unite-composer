using System.Collections.Generic;

namespace Unite.Composer.Web.Resources.OncoGrid
{
    /// <summary>
    /// This class wraps up all information required for the javascript oncogrid framework:
    /// https://github.com/oncojs/oncogrid
    /// </summary>
    public class OncoGridResource
    {
        /// <summary>
        /// Each entry represents a column within the oncogrid.
        /// Each reference of <see cref="ObservationResource.DonorId"/> requires the existence of an entry within
        /// <see cref="Donors"/> with an equal <see cref="OncoGridDonorResource.Id"/>.
        /// Each reference of <see cref="ObservationResource.GeneId"/> requires the existence of an entry within
        /// <see cref="Genes"/> with with an equal <see cref="OncoGridGeneResource.Id"/>.
        /// </summary>
        public List<ObservationResource> Observations { get; } = new();
        
        /// <summary>
        /// Gene metadata for the oncogrid 
        /// </summary>
        public List<OncoGridGeneResource> Genes { get; } = new();
        
        /// <summary>
        /// Donor metadata for the oncogrid 
        /// </summary>
        public List<OncoGridDonorResource> Donors { get; } = new();
    }
}