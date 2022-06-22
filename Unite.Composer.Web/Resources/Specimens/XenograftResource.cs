using Unite.Indices.Entities.Basic.Specimens;

namespace Unite.Composer.Web.Resources.Specimens;

public class XenograftResource
{
    public string ReferenceId { get; set; }
    public string MouseStrain { get; set; }
    public int? GroupSize { get; set; }
    public string ImplantType { get; set; }
    public string TissueLocation { get; set; }
    public int? ImplantedCellsNumber { get; set; }
    public bool? Tumorigenicity { get; set; }
    public string TumorGrowthForm { get; set; }
    public string SurvivalDays { get; set; }

    public MolecularDataResource MolecularData { get; set; }

    public XenograftInterventionResource[] Interventions { get; set; }


    public XenograftResource(XenograftIndex index)
    {
        ReferenceId = index.ReferenceId;
        MouseStrain = index.MouseStrain;
        GroupSize = index.GroupSize;
        ImplantType = index.ImplantType;
        TissueLocation = index.TissueLocation;
        ImplantedCellsNumber = index.ImplantedCellsNumber;
        Tumorigenicity = index.Tumorigenicity;
        TumorGrowthForm = index.TumorGrowthForm;
        SurvivalDays = GetSurvivalDays(index.SurvivalDaysFrom, index.SurvivalDaysTo);

        if (index.MolecularData != null)
        {
            MolecularData = new MolecularDataResource(index.MolecularData);
        }

        if (index.Interventions != null && index.Interventions.Any())
        {
            Interventions = index.Interventions
                .Select(interventionIndex => new XenograftInterventionResource(interventionIndex))
                .ToArray();
        }
    }


    private string GetSurvivalDays(int? from, int? to)
    {
        return from == to ? $"{from}" : $"{from}-{to}";
    }
}
