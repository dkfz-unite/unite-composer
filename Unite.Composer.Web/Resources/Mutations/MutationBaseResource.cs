using System.Linq;
using Unite.Indices.Entities.Basic.Genome.Mutations;

namespace Unite.Composer.Web.Resources.Mutations
{
    public class MutationBaseResource
    {
        public long Id { get; }
        public string Code { get; }
        public string Type { get; }
        public string Chromosome { get; }
        public string SequenceType { get; }
        public int Start { get; }
        public int End { get; }
        public string Ref { get; }
        public string Alt { get; }

        public AffectedTranscriptResource[] AffectedTranscripts { get; }

        /// <summary>
        /// Projection of mutation affected transcripts to their consequences.
        /// Has the following format:
        /// - Term - consequence term (e.g. "missense_variant")
        /// - Name - consequence name (e.g. "Missense")
        /// - Impact - consequence impact (e,g. "Moderate")
        /// - Genes - list of genes affected by the consequence
        /// -- Symbol - gene symbol
        /// -- EnsemblId - gene ensemble id
        /// -- Transcripts - list of unique amino acid changes caused by the consiquence in corresponding gene
        /// </summary>
        public dynamic[] TranscriptConsequences { get; }


        public MutationBaseResource(MutationIndex index)
        {
            Id = index.Id;
            Code = index.Code;
            Type = index.Type;
            Chromosome = index.Chromosome;
            Start = index.Start;
            End = index.End;
            Ref = index.Ref;
            Alt = index.Alt;

            AffectedTranscripts = index.AffectedTranscripts?
                .Select(index => new AffectedTranscriptResource(index))
                .OrderBy(resource => resource.Transcript.Gene.Symbol)
                .ThenBy(resource => resource.Consequences.OrderBy(consequence => consequence.Severity).First().Severity)
                .ThenBy(resource => resource.Transcript.Symbol)
                .ToArray();

            TranscriptConsequences = index.AffectedTranscripts?
                .Select(affectedTranscript => new
                {
                    AminoAcidChange = affectedTranscript.AminoAcidChange,
                    CodonChange = affectedTranscript.CodonChange,
                    Gene = affectedTranscript.Transcript.Gene,
                    Transcript = affectedTranscript.Transcript,
                    Consequence = affectedTranscript.Consequences.OrderBy(consequence => consequence.Severity).First()
                })
                .OrderBy(affectedTranscript => affectedTranscript.Consequence.Severity)
                .GroupBy(
                    affectedTranscript => affectedTranscript.Consequence.Type,
                    (key, group) => new { Value = group.First().Consequence, Elements = group })
                .Select(consequenceGroup => new
                {
                    Term = consequenceGroup.Value.Type,
                    Name = consequenceGroup.Value.Type,
                    Impact = consequenceGroup.Value.Impact,
                    Genes = consequenceGroup.Elements
                        .GroupBy(
                            affectedTranscript => affectedTranscript.Gene.Symbol,
                            (key, group) => new { Value = group.First().Gene, Elements = group })
                        .OrderBy(geneGroup => geneGroup.Value.Symbol)
                        .Select(geneGroup => new
                        {
                            Id = geneGroup.Value.Id,
                            Symbol = geneGroup.Value.Symbol,
                            EnsemblId = geneGroup.Value.EnsemblId,
                            Transcripts = geneGroup.Elements
                                .Where(affectedTranscript => affectedTranscript.AminoAcidChange != null)
                                .Select(affectedTranscript => affectedTranscript.AminoAcidChange)
                                .OrderBy(aminoAcidChange => aminoAcidChange)
                                .Distinct()
                        })
                })
                .ToArray();
        }
    }
}
