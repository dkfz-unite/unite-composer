using System.Xml.Serialization;

namespace Unite.Composer.Visualization.Lolliplot.Annotations.Clients.Pfam.Resources
{
    public class ProteinSequenceResource
    {
        [XmlAttribute("length")]
        public string LengthString { get; set; }

        [XmlIgnore]
        public int Length => int.Parse(LengthString);
    }
}
