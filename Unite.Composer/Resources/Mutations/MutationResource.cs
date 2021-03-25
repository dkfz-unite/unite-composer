using System.Linq;
using Unite.Indices.Entities.Mutations;

namespace Unite.Composer.Resources.Mutations
{
    public class MutationResource
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Type { get; set; }
        public string Chromosome { get; set; }
        public string SequenceType { get; set; }
        public string Position { get; set; }
        public string Ref { get; set; }
        public string Alt { get; set; }

        public AffectedTranscriptResource[] AffectedTranscripts { get; set; }
        public dynamic[] TranscriptConsequences { get; set; }

        public int Donors { get; set; }


        public MutationResource(MutationIndex index)
        {
            Id = index.Id;
            Code = index.Code;
            Type = index.Type;
            Chromosome = index.Chromosome;
            SequenceType = index.SequenceType;
            Position = GetPosition(index.Start, index.End);
            Ref = index.Ref;
            Alt = index.Alt;

            AffectedTranscripts = index.AffectedTranscripts?
                .Select(index => new AffectedTranscriptResource(index))
                .OrderBy(resource => resource.Gene.Symbol)
                .ThenBy(resource => resource.Consequences.OrderBy(consequence => consequence.Severity).First().Severity)
                .ToArray();

            // This code transfroms affected transcripts data to format suitable for mutations list view.
            // This code is written not in javascript, but in C# because of weak grouping functionality in javascript.
            TranscriptConsequences = index.AffectedTranscripts?
                .Select(affectedTranscript => new
                {
                    AminoAcidChange = affectedTranscript.AminoAcidChange,
                    CodonChange = affectedTranscript.CodonChange,
                    Gene = affectedTranscript.Gene,
                    Transcript = affectedTranscript.Transcript,
                    Consequence = affectedTranscript.Consequences.OrderBy(consequence => consequence.Severity).First()
                })
                .OrderBy(affectedTranscript => affectedTranscript.Consequence.Severity)
                .GroupBy(
                    affectedTranscript => affectedTranscript.Consequence.Type,
                    affectedTranscript => affectedTranscript,
                    (key, group) => new { Value = group.First().Consequence, Elements = group })
                .Select(consequenceGroup => new
                {
                    Term = consequenceGroup.Value.Type,
                    Name = consequenceGroup.Value.Type,
                    Impact = consequenceGroup.Value.Impact,
                    Genes = consequenceGroup.Elements
                        .GroupBy(
                            affectedTranscript => affectedTranscript.Gene.Symbol,
                            affectedTranscript => affectedTranscript,
                            (key, group) => new { Value = group.First().Gene, Elements = group })
                        .Select(geneGroup => new
                        {
                            Symbol = geneGroup.Value.Symbol,
                            EnsemblId = geneGroup.Value.EnsemblId,
                            Transcripts = geneGroup.Elements
                                .Where(affectedTranscript => affectedTranscript.AminoAcidChange != null)
                                .Select(affectedTranscript => affectedTranscript.AminoAcidChange)
                        })
                })
                .ToArray();

            Donors = index.NumberOfDonors;
        }

        private string GetPosition(int start, int end)
        {
            return start == end ? $"{start}" : $"{start}-{end}";
        }
    }
}
