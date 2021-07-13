﻿using Unite.Indices.Entities.Basic.Mutations;

namespace Unite.Composer.Visualization.Oncogrid.Data
{
    public class OncoGridGene
    {
        public string Id { get; set; }
        public string Symbol { get; set; }


        public OncoGridGene(GeneIndex index)
        {
            Id = index.Id.ToString();
            Symbol = index.Symbol ?? index.EnsemblId;
        }
    }
}