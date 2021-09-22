using System.Xml.Serialization;

namespace Unite.Composer.Visualization.Lolliplot.Annotations.Clients.Pfam.Resources
{
    [XmlRoot(ElementName = "pfam", Namespace = "https://pfam.xfam.org/")]
    public class PfamResource<T> where T : PfamResourceEntry
    {
        [XmlElement("entry")]
        public T Entry { get; set; }
    }
}
