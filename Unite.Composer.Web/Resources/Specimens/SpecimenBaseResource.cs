using Unite.Composer.Data.Specimens.Models;
using Unite.Indices.Entities.Basic.Specimens;

namespace Unite.Composer.Web.Resources.Specimens;

public class SpecimenBaseResource
{
    public int Id { get; set; }
    public int? ParentId { get; set; }
    public int? CreationDay { get; set; }

    public TissueResource Tissue { get; set; }
    public CellLineResource CellLine { get; set; }
    public OrganoidResource Organoid { get; set; }
    public XenograftResource Xenograft { get; set; }


    /// <summary>
    /// Initialises specimen resource with drugs screening data from the index.
    /// </summary>
    /// <param name="index">Specimen index</param>
    public SpecimenBaseResource(SpecimenIndex index)
    {
        Map(index);

        if (index.Tissue != null)
        {
            Tissue = new TissueResource(index.Tissue);
        }
        else if (index.CellLine != null)
        {
            CellLine = new CellLineResource(index.CellLine);
        }
        else if (index.Organoid != null)
        {
            Organoid = new OrganoidResource(index.Organoid);
        }
        else if (index.Xenograft != null)
        {
            Xenograft = new XenograftResource(index.Xenograft);
        }
    }

    /// <summary>
    /// Initialises specimen resource with drug screening data from database model.
    /// </summary>
    /// <param name="index">Specimen index</param>
    /// <param name="drugScreenings">Drugs sreening data models</param>
    public SpecimenBaseResource(SpecimenIndex index, DrugScreeningModel[] drugScreenings)
    {
        Map(index);

        if (index.Tissue != null)
        {
            Tissue = new TissueResource(index.Tissue, drugScreenings);
        }
        else if (index.CellLine != null)
        {
            CellLine = new CellLineResource(index.CellLine, drugScreenings);
        }
        else if (index.Organoid != null)
        {
            Organoid = new OrganoidResource(index.Organoid, drugScreenings);
        }
        else if (index.Xenograft != null)
        {
            Xenograft = new XenograftResource(index.Xenograft, drugScreenings);
        }
    }


    private void Map(SpecimenIndex index)
    {
        Id = index.Id;
        ParentId = index.ParentId;
        CreationDay = index.CreationDay;
    }
}
