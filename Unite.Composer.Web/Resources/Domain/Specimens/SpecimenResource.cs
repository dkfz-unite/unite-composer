using Unite.Composer.Data.Specimens.Models;
using Unite.Composer.Web.Resources.Domain.Basic;
using Unite.Essentials.Extensions;
using Unite.Indices.Entities.Specimens;

namespace Unite.Composer.Web.Resources.Domain.Specimens;

public class SpecimenResource : Basic.Specimens.SpecimenResource
{
    public int DonorId { get; set; }
    public int? ParentId { get; set; }
    public string ParentReferenceId { get; set; }
    public string ParentType { get; set; }

    public int NumberOfGenes { get; set; }
    public int NumberOfSsms { get; set; }
    public int NumberOfCnvs { get; set; }
    public int NumberOfSvs { get; set; }

    public SpecimenResource Parent { get; set; }
    public SpecimenDataResource Data { get; set; }
    public SampleResource Sample { get; set; }


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
        DonorId = index.DonorId;
        ParentId = index.ParentId;
        ParentReferenceId = index.ParentReferenceId;
        ParentType = index.ParentType;
        
        NumberOfGenes = index.NumberOfGenes;
        NumberOfSsms = index.NumberOfSsms;
        NumberOfCnvs = index.NumberOfCnvs;
        NumberOfSvs = index.NumberOfSvs;
        
        if (index.Data != null)
            Data = new SpecimenDataResource(index.Data, index.Type);

        if (index.Analyses.IsNotEmpty())
            Sample = new SampleResource(index, index.Analyses);
    }
}
