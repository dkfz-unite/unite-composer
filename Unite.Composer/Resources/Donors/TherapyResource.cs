using Unite.Indices.Entities;

namespace Unite.Composer.Resources.Donors
{
    public class TherapyResource
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public TherapyResource(TherapyIndex index)
        {
            Id = index.Id;
            Name = index.Name;
        }
    }
}
