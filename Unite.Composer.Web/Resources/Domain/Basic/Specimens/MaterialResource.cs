using Unite.Indices.Entities.Basic.Specimens;

namespace Unite.Composer.Web.Resources.Domain.Basic.Specimens;

public class MaterialResource
{
    public int Id { get; set; }
    public string ReferenceId { get; set; }
    public int? CreationDay { get; set; }
    public string Type { get; set; }
    public string TumorType { get; set; }
    public string Source { get; set; }

    public MolecularDataResource MolecularData { get; set; }


    public MaterialResource(MaterialIndex index)
    {
        Id = index.Id;
        ReferenceId = index.ReferenceId;
        CreationDay = index.CreationDay;
        Type = index.Type;
        TumorType = index.TumorType;
        Source = index.Source;

        if (index.MolecularData != null)
            MolecularData = new MolecularDataResource(index.MolecularData);
    }
}
