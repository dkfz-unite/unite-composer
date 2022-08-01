using Unite.Composer.Data.Specimens.Models;
using Unite.Indices.Entities.Specimens;

namespace Unite.Composer.Web.Resources.Specimens;

public class SpecimenResource : SpecimenBaseResource
{
    public int DonorId { get; set; }

    public SpecimenResource Parent { get; set; }
    public SpecimenResource[] Children { get; set; }

    public int NumberOfMutations { get; set; }
    public int NumberOfGenes { get; set; }
    public int NumberOfDrugs { get; set; }

    /// <summary>
    /// Initialises specimen resource with drugs screening data from the index.
    /// </summary>
    /// <param name="index">Specimen index</param>
    public SpecimenResource(SpecimenIndex index) : this(index, false, false)
    {
        DonorId = index.Donor.Id;
    }

    /// <summary>
    /// Initialises specimen resource with drug screening data from database model.
    /// </summary>
    /// <param name="index">Specimen index</param>
    /// <param name="drugScreenings">Drugs sreening data models</param>
    public SpecimenResource(SpecimenIndex index, DrugScreeningModel[] drugScreenings) : this(index, drugScreenings, false, false)
    {
        DonorId = index.Donor.Id;
    }


    private SpecimenResource(SpecimenIndex index, bool skipParent, bool skipChildren) : base(index)
    {
        Map(index, skipParent, skipChildren);
    }

    private SpecimenResource(SpecimenIndex index, DrugScreeningModel[] drugScreenings, bool skipParent, bool skipChildren) : base(index, drugScreenings)
    {
        Map(index, skipParent, skipChildren);
    }


    private void Map(SpecimenIndex index, bool skipParent, bool skipChildren)
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
        NumberOfDrugs = index.NumberOfDrugs;
    }
}
