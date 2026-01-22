using Unite.Indices.Entities.Basic.Specimens;

namespace Unite.Composer.Web.Resources.Domain.Basic.Specimens;

public class TumorClassificationResource
{
    public string Superfamily { get; set; }
    public string Family { get; set; }
    public string Class { get; set; }
    public string Subclass { get; set; }


    public TumorClassificationResource(TumorClassificationIndex index)
    {
        Superfamily = index.Superfamily;
        Family = index.Family;
        Class = index.Class;
        Subclass = index.Subclass;
    }
}
