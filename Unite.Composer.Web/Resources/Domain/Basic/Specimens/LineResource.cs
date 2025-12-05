using Unite.Indices.Entities.Basic.Specimens;

namespace Unite.Composer.Web.Resources.Domain.Basic.Specimens;

public class LineResource : SpecimenBaseResource
{
    public string CellsSpecies { get; set; }
    public string CellsType { get; set; }
    public string CellsCultureType { get; set; }

    public string Name { get; set; }
    public string DepositorName { get; set; }
    public string DepositorEstablishment { get; set; }
    public DateOnly? EstablishmentDate { get; set; }

    public string PubmedLink { get; set; }
    public string AtccLink { get; set; }
    public string ExpasyLink { get; set; }


    public LineResource(LineIndex index) : base(index)
    {
        CellsSpecies = index.CellsSpecies;
        CellsType = index.CellsType;
        CellsCultureType = index.CellsCultureType;

        Name = index.Name;
        DepositorName = index.DepositorName;
        DepositorEstablishment = index.DepositorEstablishment;
        EstablishmentDate = index.EstablishmentDate;

        PubmedLink = index.PubmedLink;
        AtccLink = index.AtccLink;
        ExpasyLink = index.ExpasyLink;
    }
}
