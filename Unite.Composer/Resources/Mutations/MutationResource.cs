using System.Linq;
using Unite.Indices.Entities.Mutations;

namespace Unite.Composer.Resources.Mutations
{
    public class MutationResource
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Type { get; set; }
        public string Ref { get; set; }
        public string Alt { get; set; }

        public GeneResource Gene { get; set; }

        public int Donors { get; set; }


        public MutationResource(MutationIndex index)
        {
            Id = index.Id;
            Name = index.Name;
            Code = index.Code;
            Type = index.Type;
            Ref = index.Ref;
            Alt = index.Alt;

            if(index.Gene != null)
            {
                Gene = new GeneResource(index.Gene);
            }

            if(index.Donors != null && index.Donors.Length > 0)
            {
                Donors = index.Donors
                    .Count();
            }
        }
    }
}
