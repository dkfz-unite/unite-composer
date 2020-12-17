using Unite.Indices.Entities;

namespace Unite.Composer.Resources.Mutations
{
    public class GeneResource
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public GeneResource(GeneIndex index)
        {
            Id = index.Id;
            Name = index.Name;
        }
    }
}
