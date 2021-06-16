namespace Unite.Composer.Search.Services.Filters.Constants
{
    public static class CellLineFilterNames
    {
        private const string _prefix = "CellLine";

        public static readonly string Id = $"{_prefix}.Id";
        public static readonly string ReferenceId = $"{_prefix}.ReferenceId";
        public static readonly string Species = $"{_prefix}.Species";
        public static readonly string Type = $"{_prefix}.Type";
        public static readonly string CultureType = $"{_prefix}.CultureType";

        public static readonly string Name = $"{_prefix}.Name";

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
