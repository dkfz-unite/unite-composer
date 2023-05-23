using Unite.Composer.Search.Services.Criteria.Visualizations;

namespace Unite.Composer.Search.Services.Criteria;

public record SearchCriteria
{
    public int From { get; set; }
    public int Size { get; set; }
    public string Term { get; set; }

    public DonorCriteria Donor { get; set; }
    public ImageCriteria Image { get; set; }
    public MriImageCriteria Mri { get; set; }
    public SpecimenCriteria Specimen { get; set; }
    public TissueCriteria Tissue { get; set; }
    public CellLineCriteria Cell { get; set; }
    public OrganoidCriteria Organoid { get; set; }
    public XenograftCriteria Xenograft { get; set; }
    public SampleCriteria Sample { get; set; }
    public GeneCriteria Gene { get; set; }
    public VariantCriteria Variant { get; set; }
    public MutationCriteria Ssm { get; set; }
    public CopyNumberVariantCriteria Cnv { get; set; }
    public StructuralVariantCriteria Sv { get; set; }
    public OncoGridCriteria OncoGrid { get; set; }

    public bool HasGeneFilters => 
        Gene?.IsNotEmpty() == true;

    public bool HasVariantFilters => 
        Ssm?.IsNotEmpty() == true || 
        Cnv?.IsNotEmpty() == true || 
        Sv?.IsNotEmpty() == true;

    public SearchCriteria()
    {
        From = 0;
        Size = 20;
    }
}
