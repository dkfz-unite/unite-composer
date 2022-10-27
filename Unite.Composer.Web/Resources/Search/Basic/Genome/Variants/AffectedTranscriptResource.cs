using Unite.Indices.Entities.Basic.Genome.Variants;

namespace Unite.Composer.Web.Resources.Search.Basic.Genome.Variants;

public class AffectedTranscriptResource
{
    public TranscriptResource Feature { get; set; }

    public string AminoAcidChange { get; set; }
    public string CodonChange { get; set; }

    public int? Distance { get; set; }
    public int? OverlapBpNumber { get; set; }
    public int? OverlapPercentage { get; set; }

    public AffectedTranscriptResource(AffectedTranscriptIndex index)
    {
        Feature = new TranscriptResource(index.Feature);

        AminoAcidChange = index.AminoAcidChange;
        CodonChange = index.CodonChange;

        Distance = index.Distance;
        OverlapBpNumber = index.OverlapBpNumber;
        OverlapPercentage = index.OverlapPercentage;
    }
}
