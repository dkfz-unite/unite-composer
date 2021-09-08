using Unite.Indices.Entities.Basic.Mutations;

namespace Unite.Composer.Web.Resources.Genes
{
    public class GeneBaseResource
    {
        public long Id { get; }
        public string Symbol { get; }
        public string Biotype { get; }
        public string Chromosome { get; }
        public int? Start { get; }
        public int? End { get; }
        public bool? Strand { get; }

        public string EnsemblId { get; }


        public GeneBaseResource(GeneIndex index)
        {
            Id = index.Id;
            Symbol = index.Symbol;
            Biotype = index.Biotype;
            Chromosome = index.Chromosome;
            Start = index.Start;
            End = index.End;
            Strand = index.Strand;

            EnsemblId = index.EnsemblId;
        }
    }
}
