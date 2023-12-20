using Unite.Composer.Data.Specimens.Models;
using Unite.Essentials.Extensions;
using Unite.Indices.Entities.Basic.Specimens;

namespace Unite.Composer.Web.Resources.Domain.Basic.Specimens;

public class OrganoidResource
{
    public int Id { get; set; }
    public string ReferenceId { get; set; }
    public int? CreationDay { get; set; }
    public string Medium { get; set; }
    public int? ImplantedCellsNumber { get; set; }
    public bool? Tumorigenicity { get; set; }

    public MolecularDataResource MolecularData { get; set; }
    public DrugScreeningResource[] DrugScreenings { get; set; }
    public OrganoidInterventionResource[] Interventions { get; set; }


    public OrganoidResource(OrganoidIndex index)
    {
        Id = index.Id;
        ReferenceId = index.ReferenceId;
        CreationDay = index.CreationDay;
        Medium = index.Medium;
        ImplantedCellsNumber = index.ImplantedCellsNumber;
        Tumorigenicity = index.Tumorigenicity;

        if (index.MolecularData != null)
            MolecularData = new MolecularDataResource(index.MolecularData);

        if (index.DrugScreenings.IsNotEmpty())
        {
            DrugScreenings = index.DrugScreenings
                .Select(screeningIndex => new DrugScreeningResource(screeningIndex))
                .ToArray();
        }

        if (index.Interventions.IsNotEmpty())
        {
            Interventions = index.Interventions
                .Select(interventionIndex => new OrganoidInterventionResource(interventionIndex))
                .ToArray();
        }
    }

    public OrganoidResource(OrganoidIndex index, DrugScreeningModel[] drugScreenings) : this(index)
    {
        if (drugScreenings.IsNotEmpty())
        {
            DrugScreenings = drugScreenings
                .Select(screeningModel => new DrugScreeningResource(screeningModel))
                .ToArray();
        }
    }
}
