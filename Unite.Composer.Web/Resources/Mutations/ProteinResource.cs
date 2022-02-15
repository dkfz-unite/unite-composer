using Unite.Indices.Entities.Basic.Genome;

namespace Unite.Composer.Web.Resources.Mutations
{
    public class ProteinResource
    {
        public int Id { get; set; }
        public string Symbol { get; set; }
        public int? Start { get; set; }
        public int? End { get; set; }
        public int? Length { get; set; }

        public string EnsemblId { get; set; }

        public ProteinResource(ProteinIndex index)
        {
            Id = index.Id;
            Symbol = index.Symbol;
            Start = index.Start;
            End = index.End;
            Length = index.Length;

            EnsemblId = index.EnsemblId;
        }
    }
}
