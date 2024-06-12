using Unite.Indices.Entities.Basic.Genome.Dna;

namespace Unite.Composer.Web.Resources.Domain.Basic.Genome.Variants;

public class AffectedTranscriptResource
{
    public string ProteinChange { get; set; }
    public string CodonChange { get; set; }

    public int? Distance { get; set; }
    public int? OverlapBpNumber { get; set; }
    public double? OverlapPercentage { get; set; }

    public TranscriptResource Feature { get; set; }


    public AffectedTranscriptResource(AffectedTranscriptIndex index)
    {
        ProteinChange = index.ProteinChange;
        CodonChange = index.CodonChange;

        Distance = index.Distance;
        OverlapBpNumber = index.OverlapBpNumber;
        OverlapPercentage = index.OverlapPercentage;

        if (index.Feature != null)
            Feature = new TranscriptResource(index.Feature);
    }
}
