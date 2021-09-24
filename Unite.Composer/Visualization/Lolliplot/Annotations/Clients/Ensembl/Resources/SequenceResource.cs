using System.Text.Json.Serialization;

namespace Unite.Composer.Visualization.Lolliplot.Annotations.Clients.Ensembl.Resources
{
    public class SequenceResource
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("query")]
        public string Query { get; set; }

        [JsonPropertyName("molecule")]
        public string Molecule { get; set; }

        [JsonPropertyName("seq")]
        public string Sequence { get; set; }
    }
}
