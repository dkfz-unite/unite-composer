using System.Text.Json.Serialization;

namespace Unite.Composer.Visualization.Lolliplot.Annotations.Clients.Uniprot.Resources
{
    public class ProteinDomainResource
    {
        [JsonPropertyName("accession")]
        public string Id { get; set; }

        [JsonPropertyName("entry_protein_locations")]
        public ProteinDomainLocationResource[] Locations { get; set; }
    }
}
