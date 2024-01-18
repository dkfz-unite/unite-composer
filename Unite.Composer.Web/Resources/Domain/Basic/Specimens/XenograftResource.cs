using Unite.Composer.Data.Specimens.Models;
using Unite.Essentials.Extensions;
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

    public MolecularDataResource MolecularData { get; set; }
    public InterventionResource[] Interventions { get; set; }
    public DrugScreeningResource[] DrugScreenings { get; set; }


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

        if (index.MolecularData != null)
            MolecularData = new MolecularDataResource(index.MolecularData);

        if (index.Interventions.IsNotEmpty())
        {
            Interventions = index.Interventions
                .Select(interventionIndex => new InterventionResource(interventionIndex))
                .ToArray();
        }

        if (index.DrugScreenings.IsNotEmpty())
        {
            DrugScreenings = index.DrugScreenings
                .Select(screeningIndex => new DrugScreeningResource(screeningIndex))
                .ToArray();
        }
    }

    public XenograftResource(XenograftIndex index, DrugScreeningModel[] drugScreenings) : this(index)
    {
        if (drugScreenings.IsEmpty())
        {
            DrugScreenings = drugScreenings
                .Select(screeningModel => new DrugScreeningResource(screeningModel))
                .ToArray();
        }
    }


    private string GetSurvivalDays(int? from, int? to)
    {
        return from == to ? $"{from}" : $"{from}-{to}";
    }
}
