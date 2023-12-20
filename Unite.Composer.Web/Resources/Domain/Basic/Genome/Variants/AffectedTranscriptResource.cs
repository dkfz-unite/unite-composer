using Unite.Indices.Entities.Basic.Genome.Variants;

namespace Unite.Composer.Web.Resources.Domain.Basic.Genome.Variants;

public class AffectedTranscriptResource
{
    public string AminoAcidChange { get; set; }
    public string CodonChange { get; set; }

    public int? Distance { get; set; }
    public int? OverlapBpNumber { get; set; }
    public double? OverlapPercentage { get; set; }

    public TranscriptResource Feature { get; set; }


    public AffectedTranscriptResource(AffectedTranscriptIndex index)
    {
        AminoAcidChange = index.AminoAcidChange;
        CodonChange = index.CodonChange;

        Distance = index.Distance;
        OverlapBpNumber = index.OverlapBpNumber;
        OverlapPercentage = index.OverlapPercentage;

        if (index.Feature != null)
            Feature = new TranscriptResource(index.Feature);
    }
}
