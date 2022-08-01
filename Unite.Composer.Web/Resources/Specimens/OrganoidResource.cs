using Unite.Composer.Data.Specimens.Models;
using Unite.Indices.Entities.Basic.Specimens;

namespace Unite.Composer.Web.Resources.Specimens;

public class OrganoidResource
{
    public string ReferenceId { get; set; }
    public string Medium { get; set; }
    public int? ImplantedCellsNumber { get; set; }
    public bool? Tumorigenicity { get; set; }

    public MolecularDataResource MolecularData { get; set; }

    public DrugScreeningResource[] DrugScreenings { get; set; }
    public OrganoidInterventionResource[] Interventions { get; set; }


    /// <summary>
    /// Initialises organoid resource with drugs screening data from the index.
    /// </summary>
    /// <param name="index">Organoid index</param>
    public OrganoidResource(OrganoidIndex index)
    {
        Map(index);

        if (index.DrugScreenings != null && index.DrugScreenings.Any())
        {
            DrugScreenings = index.DrugScreenings
                .Select(screeningIndex => new DrugScreeningResource(screeningIndex))
                .ToArray();
        }
    }

    /// <summary>
    /// Initialises organoid resource with drug screening data from database model.
    /// </summary>
    /// <param name="index">Organoid index</param>
    /// <param name="drugScreenings">Drugs sreening data models</param>
    public OrganoidResource(OrganoidIndex index, DrugScreeningModel[] drugScreenings)
    {
        Map(index);

        if (drugScreenings != null && drugScreenings.Any())
        {
            DrugScreenings = drugScreenings
                .Select(screeningModel => new DrugScreeningResource(screeningModel))
                .ToArray();
        }
    }


    private void Map(OrganoidIndex index)
    {
        ReferenceId = index.ReferenceId;
        Medium = index.Medium;
        ImplantedCellsNumber = index.ImplantedCellsNumber;
        Tumorigenicity = index.Tumorigenicity;

        if (index.MolecularData != null)
        {
            MolecularData = new MolecularDataResource(index.MolecularData);
        }

        if (index.Interventions != null && index.Interventions.Any())
        {
            Interventions = index.Interventions
                .Select(interventionIndex => new OrganoidInterventionResource(interventionIndex))
                .ToArray();
        }
    }
}
