using System.Linq;
using Unite.Indices.Entities.Mutations;

namespace Unite.Composer.Resources.Mutations
{
    public class MutationResource
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Type { get; set; }

        public GeneResource Gene { get; set; }

        public int Donors { get; set; }


        public MutationResource(MutationIndex index)
        {
            Id = index.Id;
            Code = index.Code;
            Type = index.Type;

            if(index.Gene != null)
            {
                Gene = new GeneResource(index.Gene);
            }

            if(index.Samples!= null && index.Samples.Length > 0)
            {
                Donors = index.Samples
                    .Select(sample => sample.Donor.Id)
                    .Distinct()
                    .Count();
            }
        }
    }
}
