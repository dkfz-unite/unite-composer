using Unite.Composer.Data.Specimens.Models;
using Unite.Indices.Entities.Basic.Specimens;

namespace Unite.Composer.Web.Resources.Domain.Basic.Specimens;

public class SpecimenResource
{
    public int Id { get; set; }
    public string ReferenceId { get; set; }
    public string Type { get; set; }
    public int? CreationDay { get; set; }

    public TissueResource Tissue { get; set; }
    public CellLineResource Cell { get; set; }
    public OrganoidResource Organoid { get; set; }
    public XenograftResource Xenograft { get; set; }


    /// <summary>
    /// Initialises specimen resource with drugs screening data from the index.
    /// </summary>
    /// <param name="index">Specimen index</param>
    public SpecimenResource(SpecimenIndex index)
    {
        Id = index.Id;
        ReferenceId = index.ReferenceId;
        Type = index.Type;

        if (index.Tissue != null)
            Tissue = new TissueResource(index.Tissue);
        else if (index.Cell != null)
            Cell = new CellLineResource(index.Cell);
        else if (index.Organoid != null)
            Organoid = new OrganoidResource(index.Organoid);
        else if (index.Xenograft != null)
            Xenograft = new XenograftResource(index.Xenograft);
    }

    /// <summary>
    /// Initialises specimen resource with drug screening data from database model.
    /// </summary>
    /// <param name="index">Specimen index</param>
    /// <param name="drugScreenings">Drugs sreening data models</param>
    public SpecimenResource(SpecimenIndex index, DrugScreeningModel[] drugScreenings)
    {
        Id = index.Id;
        ReferenceId = index.ReferenceId;
        Type = index.Type;

        if (index.Tissue != null)
            Tissue = new TissueResource(index.Tissue);
        else if (index.Cell != null)
            Cell = new CellLineResource(index.Cell, drugScreenings);
        else if (index.Organoid != null)
            Organoid = new OrganoidResource(index.Organoid, drugScreenings);
        else if (index.Xenograft != null)
            Xenograft = new XenograftResource(index.Xenograft, drugScreenings);
    }
}
