using Unite.Indices.Entities.Basic.Mutations;

namespace Unite.Composer.Visualization.Lolliplot.Data
{
    public class Transcript
    {
        /// <summary>
        /// Unite transcript id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Transcript symbol
        /// </summary>
        public string Symbol { get; set; }

        /// <summary>
        /// Protein Ensemb id
        /// </summary>
        public string EnsemblId { get; set; }


        /// <summary>
        /// Protein coded by the transcript
        /// </summary>
        public Protein Protein { get; set; }


        public Transcript(TranscriptIndex index)
        {
            Id = index.Id;
            Symbol = index.Symbol;
            EnsemblId = index.EnsemblId;

            Protein = new Protein(index.Protein);
        }
    }
}
