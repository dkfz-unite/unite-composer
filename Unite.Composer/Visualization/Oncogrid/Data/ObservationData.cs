
namespace Unite.Composer.Visualization.Oncogrid.Data
{
    /// <summary>
    /// An observation represents a column within the oncogrid.
    /// 
    /// Attention: Do not rename the attributes because the name is predefined within the used javascript framework for oncogrid:
    /// https://github.com/oncojs/oncogrid
    /// </summary>
    public class ObservationData
    {
        /// <summary>
        /// Unique Id for this observation entry 
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Should be equal to <see cref="DonorResource.Id"/> within <see cref="OncoGridData.Donors"/>
        /// </summary>
        public string DonorId { get; set; }

        /// <summary>
        /// Should be equal to <see cref="GeneResource.Id"/> within <see cref="OncoGridData.Genes"/>
        /// </summary>
        public string GeneId { get; set; }

        /// <summary>
        /// Type of the mutation <see cref="MutationResource.Type"/>
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Consequence of the mutation <see cref="ConsequenceResource.Type"/>
        /// </summary>
        public string Consequence { get; set; }
    }
}