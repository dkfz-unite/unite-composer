using Unite.Composer.Data.Specimens.Models;
using Unite.Indices.Entities.Specimens;

namespace Unite.Composer.Web.Resources.Domain.Specimens;

public class SpecimenResource : Basic.Specimens.SpecimenResource
{
    public int DonorId { get; set; }
    public DateOnly? CollectionDate { get; set; }

    public SpecimenResource Parent { get; set; }

    public int NumberOfGenes { get; set; }
    public int NumberOfSsms { get; set; }
    public int NumberOfCnvs { get; set; }
    public int NumberOfSvs { get; set; }
    public SpecimenDataResource Data { get; set; }


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
            ParentId = index.Parent.Id;
            Parent = new SpecimenResource(index.Parent);
        }

        NumberOfGenes = index.NumberOfGenes;
        NumberOfSsms = index.NumberOfSsms;
        NumberOfCnvs = index.NumberOfCnvs;
        NumberOfSvs = index.NumberOfSvs;

        if (index.Data != null)
        {
            Data = new SpecimenDataResource(index.Data);
        }
    }
}
