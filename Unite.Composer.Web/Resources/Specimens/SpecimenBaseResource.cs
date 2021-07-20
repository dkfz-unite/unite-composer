using Unite.Indices.Entities.Basic.Specimens;

namespace Unite.Composer.Web.Resources.Specimens
{
    public class SpecimenBaseResource
    {
        public int Id { get; set; }

        public TissueResource Tissue { get; set; }
        public CellLineResource CellLine { get; set; }
        public OrganoidResource Organoid { get; set; }
        public XenograftResource Xenograft { get; set; }


        public SpecimenBaseResource(SpecimenIndex index)
        {
            Id = index.Id;

            if (index.Tissue != null)
            {
                Tissue = new TissueResource(index.Tissue);
            }
            else if (index.CellLine != null)
            {
                CellLine = new CellLineResource(index.CellLine);
            }
            else if (index.Organoid != null)
            {
                Organoid = new OrganoidResource(index.Organoid);
            }
            else if (index.Xenograft != null)
            {
                Xenograft = new XenograftResource(index.Xenograft);
            }
        }
    }
}
