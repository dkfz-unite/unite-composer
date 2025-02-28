using Unite.Composer.Data.Specimens.Models;
using Unite.Essentials.Extensions;
using Unite.Indices.Entities.Specimens;

namespace Unite.Composer.Web.Resources.Domain.Specimens;

public class SpecimenResource : Basic.Specimens.SpecimenResource
{
    public int DonorId { get; set; }
    public int? ParentId { get; set; }
    public string ParentReferenceId { get; set; }
    public string ParentType { get; set; }

    public SpecimenParentResource Parent { get; set; }
    public SpecimenStatsResource Stats { get; set; }
    public SpecimenDataResource Data { get; set; }
    public SpecimenSampleResource Sample { get; set; }


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
        ParentId = index.Parent?.Id;
        ParentReferenceId = index.Parent?.ReferenceId;
        ParentType = index.Parent?.Type;

        if (index.Parent != null)
            Parent = new SpecimenParentResource(index.Parent);
        
        if (index.Stats != null)
            Stats = new SpecimenStatsResource(index.Stats);
        
        if (index.Data != null)
            Data = new SpecimenDataResource(index.Data, index.Type);

        if (index.Samples.IsNotEmpty())
            Sample = new SpecimenSampleResource(index, index.Samples);
    }


}
