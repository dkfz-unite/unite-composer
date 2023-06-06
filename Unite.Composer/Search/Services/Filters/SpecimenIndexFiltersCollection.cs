using Unite.Composer.Search.Engine.Filters;
using Unite.Composer.Search.Services.Criteria;
using Unite.Composer.Search.Services.Filters.Base;
using Unite.Composer.Search.Services.Filters.Constants;
using Unite.Indices.Entities.Specimens;

namespace Unite.Composer.Search.Services.Filters;

public class SpecimenIndexFiltersCollection : FiltersCollection<SpecimenIndex>
{
    public SpecimenIndexFiltersCollection(SearchCriteria criteria) : base()
    {
        var donorFilters = new DonorFilters<SpecimenIndex>(criteria.Donor, specimen => specimen.Donor);
        var mriImageFilters = new MriImageFilters<SpecimenIndex>(criteria.Mri, specimen => specimen.Images.First());
        
        _filters.AddRange(donorFilters.All());
        _filters.AddRange(mriImageFilters.All());

        _filters.Add(new BooleanFilter<SpecimenIndex>(
            SpecimenFilterNames.HasDrugs,
            specimen => specimen.Data.Drugs,
            criteria.Tissue.HasDrugs
        ));

        _filters.Add(new BooleanFilter<SpecimenIndex>(
            SpecimenFilterNames.HasSsms,
            specimen => specimen.Data.Ssms,
            criteria.Tissue.HasSsms
        ));

        _filters.Add(new BooleanFilter<SpecimenIndex>(
            SpecimenFilterNames.HasCnvs,
            specimen => specimen.Data.Cnvs,
            criteria.Tissue.HasCnvs
        ));

        _filters.Add(new BooleanFilter<SpecimenIndex>(
            SpecimenFilterNames.HasSvs,
            specimen => specimen.Data.Svs,
            criteria.Tissue.HasSvs
        ));

        _filters.Add(new BooleanFilter<SpecimenIndex>(
            SpecimenFilterNames.HasGeneExp,
            specimen => specimen.Data.GeneExp,
            criteria.Tissue.HasGeneExp
        ));
    }
}
