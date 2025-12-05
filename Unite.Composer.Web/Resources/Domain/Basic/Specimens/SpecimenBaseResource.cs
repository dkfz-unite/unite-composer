using Unite.Indices.Entities.Basic.Specimens;

namespace Unite.Composer.Web.Resources.Domain.Basic.Specimens;

public abstract class SpecimenBaseResource
{
    public int Id { get; set; }
    public string ReferenceId { get; set; }
    public int? CreationDay { get; set; }
    public string Condition { get; set; }
    public string TumorType { get; set; }
    public double? TumorGrade { get; set; }


    public SpecimenBaseResource(SpecimenBaseIndex index)
    {
        Id = index.Id;
        ReferenceId = index.ReferenceId;
        CreationDay = index.CreationDay;
        Condition = index.Condition;
        TumorType = index.TumorType;
        TumorGrade = index.TumorGrade;
    }
}
