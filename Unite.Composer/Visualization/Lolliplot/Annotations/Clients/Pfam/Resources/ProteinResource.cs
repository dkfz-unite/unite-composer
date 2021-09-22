using System.Xml.Serialization;

namespace Unite.Composer.Visualization.Lolliplot.Annotations.Clients.Pfam.Resources
{
    public class ProteinResource : PfamResourceEntry
    {
        [XmlAttribute("accession")]
        public string Id { get; set; }

        [XmlAttribute("id")]
        public string Symbol { get; set; }

        [XmlElement("description")]
        public string Description { get; set; }

        [XmlElement("sequence")]
        public ProteinSequenceResource Sequence { get; set; }

        [XmlArray("matches")]
        [XmlArrayItem("match")]
        public ProteinDomainResource[] Domains { get; set; }
    }
}
