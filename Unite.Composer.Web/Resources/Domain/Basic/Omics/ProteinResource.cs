using Unite.Indices.Entities.Basic.Omics;

namespace Unite.Composer.Web.Resources.Domain.Basic.Omics;

public class ProteinResource
{
    public int Id { get; set; }
    public string StableId { get; set; }
    public string AccessionId { get; set; }
    public string Database { get; set; }
    public string Symbol { get; set; }
    public string Description { get; set; }
    public bool? IsCanonical { get; set; }
    public string Chromosome { get; set; }
    public int? Start { get; set; }
    public int? End { get; set; }
    public int? Length { get; set; }

    public ProteinResource(ProteinIndex index)
    {
        Id = index.Id;
        StableId = index.StableId;
        AccessionId = index.AccessionId;
        Database = index.Database;
        Symbol = index.Symbol;
        Description = index.Description;
        IsCanonical = index.IsCanonical;
        Chromosome = index.Chromosome;
        Start = index.Start;
        End = index.End;
        Length = index.Length;
    }
}
