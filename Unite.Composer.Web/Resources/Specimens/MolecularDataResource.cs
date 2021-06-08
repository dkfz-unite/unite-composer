using Unite.Indices.Entities.Basic.Molecular;

namespace Unite.Composer.Web.Resources.Specimens
{
    public class MolecularDataResource
    {
        public string GeneExpressionSubtype { get; set; }
        public string IdhStatus { get; set; }
        public string IdhMutation { get; set; }
        public string MethylationStatus { get; set; }
        public string MethylationType { get; set; }
        public bool? GcimpMethylation { get; set; }


        public MolecularDataResource(MolecularDataIndex index)
        {
            GeneExpressionSubtype = index.GeneExpressionSubtype;
            IdhStatus = index.IdhStatus;
            IdhMutation = index.IdhMutation;
            MethylationStatus = index.MethylationStatus;
            MethylationType = index.MethylationType;
            GcimpMethylation = index.GcimpMethylation;
        }
    }
}
