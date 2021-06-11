﻿namespace Unite.Composer.Search.Services.Criteria
{
    public abstract class SpecimenCriteria
    {
        public int[] Id { get; set; }
        public string[] ReferenceId { get; set; }

        public string[] GeneExpressionSubtype { get; set; }
        public string[] IdhStatus { get; set; }
        public string[] IdhMutation { get; set; }
        public string[] MethylationStatus { get; set; }
        public string[] MethylationType { get; set; }
        public bool? GcimpMethylation { get; set; }
    }
}
