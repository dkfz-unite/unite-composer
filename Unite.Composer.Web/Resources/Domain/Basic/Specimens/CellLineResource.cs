using Unite.Composer.Data.Specimens.Models;
using Unite.Essentials.Extensions;
using Unite.Indices.Entities.Basic.Specimens;

namespace Unite.Composer.Web.Resources.Domain.Basic.Specimens;

public class CellLineResource
{
    public int Id { get; set; }
    public string ReferenceId { get; set; }
    public int? CreationDay { get; set; }
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


    public CellLineResource(CellLineIndex index)
    {
        Id = index.Id;
        ReferenceId = index.ReferenceId;
        CreationDay = index.CreationDay;
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
            MolecularData = new MolecularDataResource(index.MolecularData);

        if (index.DrugScreenings.IsNotEmpty())
        {
            DrugScreenings = index.DrugScreenings
                .Select(screeningIndex => new DrugScreeningResource(screeningIndex))
                .ToArray();
        }
    }

    public CellLineResource(CellLineIndex index, DrugScreeningModel[] drugScreenings) : this(index)
    {
        if (drugScreenings.IsNotEmpty())
        {
            DrugScreenings = drugScreenings
                .Select(screeningModel => new DrugScreeningResource(screeningModel))
                .ToArray();
        }
    }
}
