using Unite.Composer.Data.Specimens.Models;
using Unite.Indices.Entities.Basic.Specimens;

namespace Unite.Composer.Web.Resources.Specimens;

public class TissueResource
{
    public string ReferenceId { get; set; }
    public string Type { get; set; }
    public string TumorType { get; set; }
    public string Source { get; set; }

    public MolecularDataResource MolecularData { get; set; }

    public DrugScreeningResource[] DrugScreenings { get; set; }


    /// <summary>
    /// Initialises tissue resource with drugs screening data from the index.
    /// </summary>
    /// <param name="index">Tissue index</param>
    public TissueResource(TissueIndex index)
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
    /// Initialises tissue resource with drug screening data from database model.
    /// </summary>
    /// <param name="index">Tissue index</param>
    /// <param name="drugScreenings">Drugs sreening data models</param>
    public TissueResource(TissueIndex index, DrugScreeningModel[] drugScreenings)
    {
        Map(index);

        if (drugScreenings != null && drugScreenings.Any())
        {
            DrugScreenings = drugScreenings
                .Select(screeningModel => new DrugScreeningResource(screeningModel))
                .ToArray();
        }
    }


    private void Map(TissueIndex index)
    {
        ReferenceId = index.ReferenceId;
        Type = index.Type;
        TumorType = index.TumorType;
        Source = index.Source;

        if (index.MolecularData != null)
        {
            MolecularData = new MolecularDataResource(index.MolecularData);
        }
    }
}
