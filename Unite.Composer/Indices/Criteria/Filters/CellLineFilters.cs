namespace Unite.Composer.Indices.Criteria.Filters
{
    public class CellLineFilters
    {
		public string[] Name { get; set; }
		public string[] Type { get; set; }
		public string[] Species { get; set; }
		public string[] GeneExpressionSubtype { get; set; }
		public string[] IdhStatus { get; set; }
		public string[] IdhMutation { get; set; }
		public string[] MethylationStatus { get; set; }
		public string[] MethylationSubtype { get; set; }
		public bool? GcimpMethylation { get; set; }
	}
}
