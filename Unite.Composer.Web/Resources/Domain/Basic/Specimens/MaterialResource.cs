using Unite.Indices.Entities.Basic.Specimens;

namespace Unite.Composer.Web.Resources.Domain.Basic.Specimens;

public class MaterialResource
{
    public int Id { get; set; }
    public string ReferenceId { get; set; }
    public int? CreationDay { get; set; }
    public string Type { get; set; }
    public string FixationType { get; set; }
    public string TumorType { get; set; }
    public double? TumorGrade { get; set; }
    public string Source { get; set; }

    public MaterialResource(MaterialIndex index)
    {
        Id = index.Id;
        ReferenceId = index.ReferenceId;
        CreationDay = index.CreationDay;
        Type = index.Type;
        FixationType = index.FixationType;
        TumorType = index.TumorType;
        TumorGrade = index.TumorGrade;
        Source = index.Source;
    }
}
