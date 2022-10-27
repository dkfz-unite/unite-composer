using Unite.Composer.Data.Specimens.Models;
using Unite.Indices.Entities.Basic.Specimens;

namespace Unite.Composer.Web.Resources.Search.Basic.Specimens;

public class SpecimenResource
{
    public int Id { get; set; }
    public int? ParentId { get; set; }
    public int? CreationDay { get; set; }

    public TissueResource Tissue { get; set; }
    public CellLineResource CellLine { get; set; }
    public OrganoidResource Organoid { get; set; }
    public XenograftResource Xenograft { get; set; }

    public MolecularDataResource MolecularData { get; set; }
    public DrugScreeningResource[] DrugScreenings { get; set; }


    /// <summary>
    /// Initialises specimen resource with drugs screening data from the index.
    /// </summary>
    /// <param name="index">Specimen index</param>
    public SpecimenResource(SpecimenIndex index)
    {
        Map(index);

        if (index.DrugScreenings?.Any() == true)
        {
            DrugScreenings = index.DrugScreenings
                .Select(screeningIndex => new DrugScreeningResource(screeningIndex))
                .ToArray();
        }
    }

    /// <summary>
    /// Initialises specimen resource with drug screening data from database model.
    /// </summary>
    /// <param name="index">Specimen index</param>
    /// <param name="drugScreenings">Drugs sreening data models</param>
    public SpecimenResource(SpecimenIndex index, DrugScreeningModel[] drugScreenings)
    {
        Map(index);

        if (drugScreenings?.Any() == true)
        {
            DrugScreenings = drugScreenings
                .Select(screeningModel => new DrugScreeningResource(screeningModel))
                .ToArray();
        }
    }


    private void Map(SpecimenIndex index)
    {
        Id = index.Id;
        ParentId = index.ParentId;
        CreationDay = index.CreationDay;

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

        if (index.MolecularData != null)
        {
            MolecularData = new MolecularDataResource(index.MolecularData);
        }
    }
}
