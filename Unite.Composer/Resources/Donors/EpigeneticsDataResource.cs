using Unite.Indices.Entities.Basic.Epigenetics;

namespace Unite.Composer.Resources.Donors
{
    public class EpigeneticsDataResource
    {
        public string GeneExpressionSubtype { get; set; }
        public string IdhStatus { get; set; }
        public string IdhMutation { get; set; }
        public string MethylationStatus { get; set; }
        public string MethylationSubtype { get; set; }
        public bool? GcimpMethylation { get; set; }

        public EpigeneticsDataResource(EpigeneticsDataIndex index)
        {
            GeneExpressionSubtype = index.GeneExpressionSubtype;
            IdhStatus = index.IdhStatus;
            IdhMutation = index.IdhMutation;
            MethylationStatus = index.MethylationStatus;
            MethylationSubtype = index.MethylationSubtype;
            GcimpMethylation = index.GcimpMethylation;
        }
    }
}
