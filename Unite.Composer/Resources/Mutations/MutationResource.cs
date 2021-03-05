using Unite.Indices.Entities.Mutations;

namespace Unite.Composer.Resources.Mutations
{
    public class MutationResource
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Type { get; set; }
        public string Chromosome { get; set; }
        public string SequenceType { get; set; }
        public string Position { get; set; }
        public string Ref { get; set; }
        public string Alt { get; set; }

        public int Donors { get; set; }


        public MutationResource(MutationIndex index)
        {
            Id = index.Id;
            Code = index.Code;
            Type = index.Type;
            Chromosome = index.Chromosome;
            SequenceType = index.SequenceType;
            Position = GetPosition(index.Start, index.End);
            Ref = index.Ref;
            Alt = index.Alt;

            Donors = index.NumberOfDonors;
        }

        private string GetPosition(int start, int end)
        {
            return start == end ? $"{start}" : $"{start}-{end}";
        }
    }
}
