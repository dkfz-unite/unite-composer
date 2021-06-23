using System.Linq;
using Unite.Indices.Entities.Specimens;

namespace Unite.Composer.Web.Resources.Specimens
{
    public class SpecimenResource
    {
        public int Id { get; set; }
        public int DonorId { get; set; }

        public SpecimenResource Parent { get; set; }
        public SpecimenResource[] Children { get; set; }

        public TissueResource Tissue { get; set; }
        public CellLineResource CellLine { get; set; }
        public OrganoidResource Organoid { get; set; }
        public XenograftResource Xenograft { get; set; }

        public MolecularDataResource MolecularData { get; set; }

        public int Mutations { get; set; }
        public int Genes { get; set; }


        public SpecimenResource(SpecimenIndex index) : this(index, index.Donor.Id, false, false)
        {
        }

        private SpecimenResource(SpecimenIndex index, int donorId, bool skipParent, bool skipChildren)
        {
            Id = index.Id;
            DonorId = donorId;


            if (index.Parent != null && !skipParent)
            {
                Parent = new SpecimenResource(index.Parent, donorId, false, true);
            }

            if (index.Children != null && !skipChildren)
            {
                Children = index.Children
                    .Select(childIndex => new SpecimenResource(childIndex, donorId, true, false))
                    .ToArray();
            }


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


            if (index.MolecularData != null)
            {
                MolecularData = new MolecularDataResource(index.MolecularData);
            }


            Mutations = index.NumberOfMutations;
            Genes = index.NumberOfGenes;
        }
    }
}
