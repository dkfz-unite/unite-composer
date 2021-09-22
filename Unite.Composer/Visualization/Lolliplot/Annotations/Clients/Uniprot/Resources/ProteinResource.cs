using System.Text.Json.Serialization;

namespace Unite.Composer.Visualization.Lolliplot.Annotations.Clients.Uniprot.Resources
{
    public class ProteinResource
    {
        [JsonPropertyName("metadata")]
        public ProteinMetadataResource Metadata { get; set; }

        [JsonPropertyName("entry_subset")]
        public ProteinDomainResource[] Domains { get; set; }
    }
}
