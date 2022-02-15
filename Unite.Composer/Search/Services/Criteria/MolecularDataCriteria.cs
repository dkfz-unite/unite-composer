namespace Unite.Composer.Search.Services.Criteria
{
    public class MolecularDataCriteria
    {
        public string[] MgmtStatus { get; set; }
        public string[] IdhStatus { get; set; }
        public string[] IdhMutation { get; set; }
        public string[] GeneExpressionSubtype { get; set; }
        public string[] MethylationSubtype { get; set; }
        public bool? GcimpMethylation { get; set; }

        public virtual bool HasValues()
        {
            return (MgmtStatus != null && MgmtStatus.Length > 0)
                || (IdhStatus != null && IdhStatus.Length > 0)
                || (IdhMutation != null && IdhMutation.Length > 0)
                || (GeneExpressionSubtype != null && GeneExpressionSubtype.Length > 0)
                || (MethylationSubtype != null && MethylationSubtype.Length > 0)
                || GcimpMethylation.HasValue;
        }
    }
}
