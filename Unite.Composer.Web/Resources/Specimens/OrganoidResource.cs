using Unite.Indices.Entities.Basic.Specimens;

namespace Unite.Composer.Web.Resources.Specimens;

public class OrganoidResource
{
    public string ReferenceId { get; set; }
    public string Medium { get; set; }
    public int? ImplantedCellsNumber { get; set; }
    public bool? Tumorigenicity { get; set; }

    public MolecularDataResource MolecularData { get; set; }

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

        if (index.Interventions != null && index.Interventions.Any())
        {
            Interventions = index.Interventions
                .Select(interventionIndex => new OrganoidInterventionResource(interventionIndex))
                .ToArray();
        }
    }
}
