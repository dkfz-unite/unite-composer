using Unite.Composer.Data.Specimens.Models;
using Unite.Indices.Entities.Basic.Specimens;

namespace Unite.Composer.Web.Resources.Domain.Basic.Specimens;

public class OrganoidResource
{
    public string ReferenceId { get; set; }
    public string Medium { get; set; }
    public int? ImplantedCellsNumber { get; set; }
    public bool? Tumorigenicity { get; set; }

    public MolecularDataResource MolecularData { get; set; }
    public DrugScreeningResource[] DrugScreenings { get; set; }
    public OrganoidInterventionResource[] Interventions { get; set; }


    public OrganoidResource(OrganoidIndex index)
    {
        ReferenceId = index.ReferenceId;
        Medium = index.Medium;
        ImplantedCellsNumber = index.ImplantedCellsNumber;
        Tumorigenicity = index.Tumorigenicity;

        if (index.MolecularData != null)
        {
            MolecularData = new MolecularDataResource(index.MolecularData);
        }

        if (index.DrugScreenings?.Any() == true)
        {
            DrugScreenings = index.DrugScreenings
                .Select(screeningIndex => new DrugScreeningResource(screeningIndex))
                .ToArray();
        }

        if (index.Interventions?.Any() == true)
        {
            Interventions = index.Interventions
                .Select(interventionIndex => new OrganoidInterventionResource(interventionIndex))
                .ToArray();
        }
    }

    public OrganoidResource(OrganoidIndex index, DrugScreeningModel[] drugScreenings) : this(index)
    {
        if (drugScreenings?.Any() == true)
        {
            DrugScreenings = drugScreenings
                .Select(screeningModel => new DrugScreeningResource(screeningModel))
                .ToArray();
        }
    }
}
