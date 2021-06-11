namespace Unite.Composer.Search.Services.Filters.Constants
{
    public static class TissueFilterNames
    {
        private const string _prefix = "Tissue";

        public static readonly string Id = $"{_prefix}.Id";
        public static readonly string ReferenceId = $"{_prefix}.ReferenceId";
        public static readonly string Type = $"{_prefix}.Type";
        public static readonly string TumourType = $"{_prefix}.TumourType";
        public static readonly string Source = $"{_prefix}.Source";

        public static readonly string GeneExpressionSubtype = $"{_prefix}.GeneExpressionSubtype";
        public static readonly string IdhStatus = $"{_prefix}.IhdStatus";
        public static readonly string IdhMutation = $"{_prefix}.IdhMutation";
        public static readonly string MethylationStatus = $"{_prefix}.MethylationStatus";
        public static readonly string MethylationType = $"{_prefix}.MethylationType";
        public static readonly string GcimpMethylation = $"{_prefix}.GcimpMethylation";


        public static SpecimenFilterNames SpecimenFilterNames()
        {
            return new SpecimenFilterNames
            {
                Id = Id,
                GeneExpressionSubtype = GeneExpressionSubtype,
                IdhStatus = IdhStatus,
                IdhMutation = IdhMutation,
                MethylationStatus = MethylationStatus,
                MethylationType = MethylationType,
                GcimpMethylation = GcimpMethylation
            };
        }
    }
}
