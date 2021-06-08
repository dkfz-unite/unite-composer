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

        public int Mutations { get; set; }
        public int Genes { get; set; }


        public SpecimenResource(SpecimenIndex index) : this(index, false, false)
        {
        }

        private SpecimenResource(SpecimenIndex index, bool skipParent, bool skipChildren)
        {
            Id = index.Id;
            DonorId = index.Donor.Id;


            if (index.Parent != null && !skipParent)
            {
                Parent = new SpecimenResource(index.Parent, false, true);
            }

            if (index.Children != null && !skipChildren)
            {
                Children = index.Children
                    .Select(childIndex => new SpecimenResource(childIndex, true, false))
                    .ToArray();
            }


            if (index.Tissue != null)
            {
                Tissue = new TissueResource(index.Tissue);
            }

            if (index.CellLine != null)
            {
                CellLine = new CellLineResource(index.CellLine);
            }


            Mutations = index.NumberOfMutations;
            Genes = index.NumberOfGenes;
        }
    }
}
