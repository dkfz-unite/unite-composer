using Unite.Composer.Search.Services.Criteria;
using Unite.Composer.Search.Services.Filters.Base;
using Unite.Indices.Entities.Specimens;

namespace Unite.Composer.Search.Services.Filters;

public class SpecimenIndexFiltersCollection : FiltersCollection<SpecimenIndex>
{
    public SpecimenIndexFiltersCollection(SearchCriteria criteria) : base()
    {
        var donorFilters = new DonorFilters<SpecimenIndex>(criteria.DonorFilters, specimen => specimen.Donor);
        var mriImageFilters = new MriImageFilters<SpecimenIndex>(criteria.MriImageFilters, specimen => specimen.Donor.Images.First());
        var geneFilters = new GeneFilters<SpecimenIndex>(criteria.GeneFilters, specimen => specimen.Mutations.First().AffectedTranscripts.First().Transcript.Gene);
        var mutationFilters = new MutationFilters<SpecimenIndex>(criteria.MutationFilters, specimen => specimen.Mutations.First());

        _filters.AddRange(donorFilters.All());
        _filters.AddRange(mriImageFilters.All());
        _filters.AddRange(geneFilters.All());
        _filters.AddRange(mutationFilters.All());
    }
}
