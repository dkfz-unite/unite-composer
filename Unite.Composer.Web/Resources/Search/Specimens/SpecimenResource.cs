using Unite.Composer.Data.Specimens.Models;
using Unite.Indices.Entities.Specimens;

namespace Unite.Composer.Web.Resources.Search.Specimens;

public class SpecimenResource : Basic.Specimens.SpecimenResource
{
    public int DonorId { get; set; }

    public SpecimenResource Parent { get; set; }

    public int NumberOfDrugs { get; set; }
    public int NumberOfGenes { get; set; }
    public int NumberOfMutations { get; set; }
    public int NumberOfCopyNumberVariants { get; set; }
    public int NumberOfStructuralVariants { get; set; }


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
        if (index.Donor != null)
        {
            DonorId = index.Donor.Id;
        }

        if (index.Parent != null)
        {
            Parent = new SpecimenResource(index.Parent);
        }

        NumberOfDrugs = index.NumberOfDrugs;
        NumberOfGenes = index.NumberOfGenes;
        NumberOfMutations = index.NumberOfMutations;
        NumberOfCopyNumberVariants = index.NumberOfCopyNumberVariants;
        NumberOfStructuralVariants = index.NumberOfStructuralVariants;
    }
}
