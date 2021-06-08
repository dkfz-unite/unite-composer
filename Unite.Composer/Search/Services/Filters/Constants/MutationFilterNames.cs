﻿namespace Unite.Composer.Search.Services.Filters.Constants
{
    public static class MutationFilterNames
    {
        private const string _prefix = "Mutation";

        public static readonly string Code = $"{_prefix}.Code";
        public static readonly string Type = $"{_prefix}.Type";
        public static readonly string Chromosome = $"{_prefix}.Chromosome";
        public static readonly string Position = $"{_prefix}.Position";

        public static readonly string Impact = $"{_prefix}.Impact";
        public static readonly string Consequence = $"{_prefix}.Consequence";
        public static readonly string Gene = $"{_prefix}.Gene";
    }
}
