using System.Text.Json.Serialization;

namespace Unite.Composer.Visualization.Lolliplot.Annotations.Clients.Uniprot.Resources
{
    public class ProteinResource
    {
        [JsonPropertyName("metadata")]
        public ProteinMetadata Metadata { get; set; }

        [JsonPropertyName("entry_subset")]
        public ProteinDomain[] Domains { get; set; }
    }
}
