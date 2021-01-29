using Unite.Indices.Entities.Basic.Donors;

namespace Unite.Composer.Resources.Donors
{
    public class WorkPackageResource
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public WorkPackageResource(WorkPackageIndex index)
        {
            Id = index.Id;
            Name = index.Name;
        }
    }
}
