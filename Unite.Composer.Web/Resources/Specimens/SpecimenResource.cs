using Unite.Indices.Entities.Specimens;

namespace Unite.Composer.Web.Resources.Specimens;

public class SpecimenResource : SpecimenBaseResource
{
    public int DonorId { get; }

    public SpecimenResource Parent { get; }
    public SpecimenResource[] Children { get; }

    public int NumberOfMutations { get; }
    public int NumberOfGenes { get; }


    public SpecimenResource(SpecimenIndex index) : this(index, false, false)
    {
        DonorId = index.Donor.Id;
    }

    private SpecimenResource(SpecimenIndex index, bool skipParent, bool skipChildren) : base(index)
    {
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


        NumberOfMutations = index.NumberOfMutations;
        NumberOfGenes = index.NumberOfGenes;
    }
}
