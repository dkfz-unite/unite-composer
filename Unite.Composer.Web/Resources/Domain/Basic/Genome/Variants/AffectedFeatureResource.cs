using Unite.Indices.Entities.Basic.Genome.Variants;

namespace Unite.Composer.Web.Resources.Domain.Basic.Genome.Variants;

public class AffectedFeatureResource
{
    public GeneResource Gene { get; set; }

    public AffectedTranscriptResource Transcript { get; set; }
    //public AffectedRegulatorResource Regulator { get; set; }
    //public AffectedMotifResource Motif { get; set; }

    public ConsequenceResource[] Consequences { get; set; }


    public AffectedFeatureResource(AffectedFeatureIndex index)
    {
        if (index.Gene != null)
        {
            Gene = new GeneResource(index.Gene);
        }

        if (index.Transcript != null)
        {
            Transcript = new AffectedTranscriptResource(index.Transcript);
        }

        if (index.Consequences?.Any() == true)
        {
            Consequences = index.Consequences
                .Select(consequenceIndex => new ConsequenceResource(consequenceIndex))
                .ToArray();
        }
    }
}
