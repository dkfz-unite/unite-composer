using System.Text.Json.Serialization;

namespace Unite.Composer.Visualization.Lolliplot.Annotations.Clients.Uniprot.Resources
{
    public class ProteinMetadata
    {
        [JsonPropertyName("accession")]
        public string Id { get; set; }

        [JsonPropertyName("id")]
        public string Symbol { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("length")]
        public int Length { get; set; }
    }
}
