using Unite.Indices.Entities.Basic.Specimens;

namespace Unite.Composer.Web.Resources.Domain.Basic.Specimens;

public class XenograftResource
{
    public int Id { get; set; }
    public string ReferenceId { get; set; }
    public int? CreationDay { get; set; }
    public string MouseStrain { get; set; }
    public int? GroupSize { get; set; }
    public string ImplantType { get; set; }
    public string ImplantLocation { get; set; }
    public int? ImplantedCellsNumber { get; set; }
    public bool? Tumorigenicity { get; set; }
    public string TumorGrowthForm { get; set; }
    public string SurvivalDays { get; set; }


    public XenograftResource(XenograftIndex index)
    {
        Id = index.Id;
        ReferenceId = index.ReferenceId;
        CreationDay = index.CreationDay;
        MouseStrain = index.MouseStrain;
        GroupSize = index.GroupSize;
        ImplantType = index.ImplantType;
        ImplantLocation = index.ImplantType;
        ImplantedCellsNumber = index.ImplantedCellsNumber;
        Tumorigenicity = index.Tumorigenicity;
        TumorGrowthForm = index.TumorGrowthForm;
        SurvivalDays = GetSurvivalDays(index.SurvivalDaysFrom, index.SurvivalDaysTo);
    }


    private static string GetSurvivalDays(int? from, int? to)
    {
        return from == to ? $"{from}" : $"{from}-{to}";
    }
}
