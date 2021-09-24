using System.Text.Json.Serialization;

namespace Unite.Composer.Visualization.Lolliplot.Annotations.Clients.Uniprot.Resources
{
    public class ProteinDomainLocation
    {
        [JsonPropertyName("fragments")]
        public ProteinDomainFragment[] Fragments { get; set; }
    }
}
