using Unite.Composer.Data.Specimens.Models;
using Unite.Indices.Entities.Basic.Specimens;

namespace Unite.Composer.Web.Resources.Specimens;

public class CellLineResource
{
    public string ReferenceId { get; set; }
    public string Species { get; set; }
    public string Type { get; set; }
    public string CultureType { get; set; }

    public string Name { get; set; }
    public string DepositorName { get; set; }
    public string DepositorEstablishment { get; set; }
    public DateTime? EstablishmentDate { get; set; }

    public string PubMedLink { get; set; }
    public string AtccLink { get; set; }
    public string ExPasyLink { get; set; }

    public MolecularDataResource MolecularData { get; set; }

    public DrugScreeningResource[] DrugScreenings { get; set; }


    /// <summary>
    /// Initialises cell line resource with drugs screening data from the index.
    /// </summary>
    /// <param name="index">Cell line index</param>
    public CellLineResource(CellLineIndex index)
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
    /// Initialises cell line resource with drug screening data from database model.
    /// </summary>
    /// <param name="index">Cell line index</param>
    /// <param name="drugScreenings">Drugs sreening data models</param>
    public CellLineResource(CellLineIndex index, DrugScreeningModel[] drugScreenings)
    {
        Map(index);

        if (drugScreenings != null && drugScreenings.Any())
        {
            DrugScreenings = drugScreenings
                .Select(screeningModel => new DrugScreeningResource(screeningModel))
                .ToArray();
        }
    }


    private void Map(CellLineIndex index)
    {
        ReferenceId = index.ReferenceId;
        Species = index.Species;
        Type = index.Type;
        CultureType = index.CultureType;

        Name = index.Name;
        DepositorName = index.DepositorName;
        DepositorEstablishment = index.DepositorEstablishment;
        EstablishmentDate = index.EstablishmentDate;

        PubMedLink = index.PubMedLink;
        AtccLink = index.AtccLink;
        ExPasyLink = index.ExPasyLink;

        if (index.MolecularData != null)
        {
            MolecularData = new MolecularDataResource(index.MolecularData);
        }
    }
}
