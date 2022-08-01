using Unite.Composer.Data.Specimens.Models;
using Unite.Indices.Entities.Specimens;

namespace Unite.Composer.Web.Resources.Specimens;

public class SpecimenResource : SpecimenBaseResource
{
    public int DonorId { get; set; }

    public SpecimenResource Parent { get; set; }

    public int NumberOfMutations { get; set; }
    public int NumberOfGenes { get; set; }

    /// <summary>
    /// Initialises specimen resource with drugs screening data from the index.
    /// </summary>
    /// <param name="index">Specimen index</param>
    public SpecimenResource(SpecimenIndex index) : base(index)
    {
        Map(index);
    }

    /// <summary>
    /// Initialises specimen resource with drug screening data from database model.
    /// </summary>
    /// <param name="index">Specimen index</param>
    /// <param name="drugScreenings">Drugs sreening data models</param>
    public SpecimenResource(SpecimenIndex index, DrugScreeningModel[] drugScreenings) : base(index, drugScreenings)
    {
        Map(index);
    }


    private void Map(SpecimenIndex index)
    {
        DonorId = index.Donor.Id;

        if (index.Parent != null)
        {
            Parent = new SpecimenResource(index.Parent);
        }

        NumberOfMutations = index.NumberOfMutations;
        NumberOfGenes = index.NumberOfGenes;
    }
}
