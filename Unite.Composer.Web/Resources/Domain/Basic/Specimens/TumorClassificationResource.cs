using Unite.Indices.Entities.Basic.Specimens;

namespace Unite.Composer.Web.Resources.Domain.Basic.Specimens;

public class TumorClassificationResource
{
    public string Superfamily { get; set; }
    public double? SuperfamilyScore { get; set; }
    public string Family { get; set; }
    public double? FamilyScore { get; set; }
    public string Class { get; set; }
    public double? ClassScore { get; set; }
    public string Subclass { get; set; }
    public double? SubclassScore { get; set; }


    public TumorClassificationResource(TumorClassificationIndex index)
    {
        Superfamily = index.Superfamily;
        SuperfamilyScore = index.SuperfamilyScore;
        Family = index.Family;
        FamilyScore = index.FamilyScore;
        Class = index.Class;
        ClassScore = index.ClassScore;
        Subclass = index.Subclass;
        SubclassScore = index.SubclassScore;
    }
}
