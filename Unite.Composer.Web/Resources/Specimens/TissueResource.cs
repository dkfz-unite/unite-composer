using Unite.Indices.Entities.Basic.Specimens;

namespace Unite.Composer.Web.Resources.Specimens
{
    public class TissueResource
    {
        public string ReferenceId { get; set; }
        public string Type { get; set; }
        public string TumorType { get; set; }
        public string Source { get; set; }
        public int? ExtractionDay { get; set; }

        public MolecularDataResource MolecularData { get; set; }


        public TissueResource(TissueIndex index)
        {
            ReferenceId = index.ReferenceId;
            Type = index.Type;
            TumorType = index.TumorType;
            Source = index.Source;
            ExtractionDay = index.ExtractionDay;

            if (index.MolecularData != null)
            {
                MolecularData = new MolecularDataResource(index.MolecularData);
            }
        }
    }
}
