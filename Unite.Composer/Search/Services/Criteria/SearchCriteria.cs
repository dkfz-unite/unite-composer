using Unite.Composer.Search.Services.Criteria.Visualizations;

namespace Unite.Composer.Search.Services.Criteria;

public record SearchCriteria
{
    public int From { get; set; }
    public int Size { get; set; }
    public string Term { get; set; }

    public DonorCriteria DonorFilters { get; set; }
    public ImageCriteria ImageFilters { get; set; }
    public MriImageCriteria MriImageFilters { get; set; }
    public SpecimenCriteria SpecimenFilters { get; set; }
    public TissueCriteria TissueFilters { get; set; }
    public CellLineCriteria CellLineFilters { get; set; }
    public OrganoidCriteria OrganoidFilters { get; set; }
    public XenograftCriteria XenograftFilters { get; set; }
    public SampleCriteria SampleFilters { get; set; }
    public GeneCriteria GeneFilters { get; set; }
    public VariantCriteria VariantFilters { get; set; }
    public MutationCriteria MutationFilters { get; set; }
    public CopyNumberVariantCriteria CopyNumberVariantFilters { get; set; }
    public StructuralVariantCriteria StructuralVariantFilters { get; set; }
    public OncoGridCriteria OncoGridFilters { get; set; }

    public bool HasGeneFilters => 
        GeneFilters?.IsNotEmpty() == true;

    public bool HasVariantFilters => 
        MutationFilters?.IsNotEmpty() == true || 
        CopyNumberVariantFilters?.IsNotEmpty() == true || 
        StructuralVariantFilters?.IsNotEmpty() == true;

    public SearchCriteria()
    {
        From = 0;
        Size = 20;
    }
}
