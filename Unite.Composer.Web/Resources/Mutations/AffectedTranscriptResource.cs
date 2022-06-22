using Unite.Indices.Entities.Basic.Genome.Mutations;

namespace Unite.Composer.Web.Resources.Mutations;

public class AffectedTranscriptResource
{
    public string AminoAcidChange { get; set; }
    public string CodonChange { get; set; }

    public TranscriptResource Transcript { get; set; }
    public ConsequenceResource[] Consequences { get; set; }

    public AffectedTranscriptResource(AffectedTranscriptIndex index)
    {
        AminoAcidChange = index.AminoAcidChange;
        CodonChange = index.CodonChange;

        Transcript = new TranscriptResource(index.Transcript);
        Consequences = index.Consequences.Select(index => new ConsequenceResource(index)).ToArray();
    }
}
