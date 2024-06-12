using Unite.Indices.Entities.Basic.Genome.Dna;

namespace Unite.Composer.Web.Resources.Domain.Basic.Genome.Variants;

public class EffectResource
{
    public string Type { get; set; }
    public string Impact { get; set; }
    public int Severity { get; set; }

    public EffectResource(EffectIndex index)
    {
        Type = index.Type;
        Impact = index.Impact;
        Severity = index.Severity;
    }
}
