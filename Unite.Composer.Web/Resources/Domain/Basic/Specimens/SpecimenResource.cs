using Unite.Composer.Data.Specimens.Models;
using Unite.Essentials.Extensions;
using Unite.Indices.Entities.Basic.Specimens;

namespace Unite.Composer.Web.Resources.Domain.Basic.Specimens;

public class SpecimenResource
{
    public int Id { get; set; }
    public string ReferenceId { get; set; }
    public string Type { get; set; }
    public int? CreationDay { get; set; }

    public MaterialResource Material { get; set; }
    public LineResource Line { get; set; }
    public OrganoidResource Organoid { get; set; }
    public XenograftResource Xenograft { get; set; }

    public MolecularDataResource MolecularData { get; set; }
    public InterventionResource[] Interventions { get; set; }
    public DrugScreeningResource[] DrugScreenings { get; set; }


    /// <summary>
    /// Initialises specimen resource with drugs screening data from the index.
    /// </summary>
    /// <param name="index">Specimen index</param>
    public SpecimenResource(SpecimenIndex index)
    {
        Id = index.Id;
        ReferenceId = index.ReferenceId;
        Type = index.Type;

        if (index.Material != null)
            Material = new MaterialResource(index.Material);
        else if (index.Line != null)
            Line = new LineResource(index.Line);
        else if (index.Organoid != null)
            Organoid = new OrganoidResource(index.Organoid);
        else if (index.Xenograft != null)
            Xenograft = new XenograftResource(index.Xenograft);

        if (index.GetMolecularData() != null)
            MolecularData = new MolecularDataResource(index.GetMolecularData());

        if (index.GetInterventions().IsNotEmpty())
            Interventions = index.GetInterventions().Select(interventionIndex => new InterventionResource(interventionIndex)).ToArray();

        if (index.GetDrugScreenings().IsNotEmpty())
            DrugScreenings = index.GetDrugScreenings().Select(drugScreeningIndex => new DrugScreeningResource(drugScreeningIndex)).ToArray();
    }

    /// <summary>
    /// Initialises specimen resource with drug screening data from database model.
    /// </summary>
    /// <param name="index">Specimen index</param>
    /// <param name="drugScreenings">Drugs sreening data models</param>
    public SpecimenResource(SpecimenIndex index, DrugScreeningModel[] drugScreenings) : this(index)
    {
        if (drugScreenings.IsNotEmpty())
            DrugScreenings = drugScreenings.Select(drugScreening => new DrugScreeningResource(drugScreening)).ToArray();
    }
}
