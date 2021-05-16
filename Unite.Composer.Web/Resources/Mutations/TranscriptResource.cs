﻿using Unite.Indices.Entities.Basic.Mutations;

namespace Unite.Composer.Resources.Mutations
{
    public class TranscriptResource
    {
        public int Id { get; set; }
        public string Symbol { get; set; }
        public string Biotype { get; set; }
        public bool? Strand { get; set; }

        public string EnsemblId { get; set; }

        public TranscriptResource(TranscriptIndex index)
        {
            Id = index.Id;
            Symbol = index.Symbol;
            Biotype = index.Biotype;
            Strand = index.Strand;

            EnsemblId = index.EnsemblId;
        }
    }
}