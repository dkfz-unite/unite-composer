using Unite.Indices.Entities.Basic.Genome;

namespace Unite.Composer.Web.Resources.Search.Basic.Genome;

public class ProteinResource
{
    public int Id { get; set; }
    public string StableId { get; set; }
    public bool? IsCanonical { get; set; }
    public int? Start { get; set; }
    public int? End { get; set; }
    public int? Length { get; set; }

    public ProteinResource(ProteinIndex index)
    {
        Id = index.Id;
        StableId = index.StableId;
        IsCanonical = index.IsCanonical;
        Start = index.Start;
        End = index.End;
        Length = index.Length;
    }
}
