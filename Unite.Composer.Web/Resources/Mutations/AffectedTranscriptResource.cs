using System.Linq;
using Unite.Composer.Resources.Mutations;
using Unite.Indices.Entities.Basic.Mutations;

namespace Unite.Composer.Web.Resources.Mutations
{
    public class AffectedTranscriptResource
    {
        public string AminoAcidChange { get; set; }
        public string CodonChange { get; set; }

        public GeneResource Gene { get; set; }
        public TranscriptResource Transcript { get; set; }
        public ConsequenceResource[] Consequences { get; set; }

        public AffectedTranscriptResource(AffectedTranscriptIndex index)
        {
            AminoAcidChange = index.AminoAcidChange;
            CodonChange = index.CodonChange;

            Gene = new GeneResource(index.Gene);
            Transcript = new TranscriptResource(index.Transcript);
            Consequences = index.Consequences.Select(index => new ConsequenceResource(index)).ToArray();
        }
    }
}
