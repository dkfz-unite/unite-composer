using Unite.Composer.Search.Engine.Filters;
using Unite.Composer.Search.Services.Criteria;
using Unite.Composer.Search.Services.Filters.Base;
using Unite.Composer.Search.Services.Filters.Constants;
using Unite.Indices.Entities.Mutations;

namespace Unite.Composer.Search.Services.Filters;

public class MutationIndexFiltersCollection : FiltersCollection<MutationIndex>
{
    public MutationIndexFiltersCollection(SearchCriteria criteria) : base()
    {
        var donorFilters = new DonorFilters<MutationIndex>(criteria.DonorFilters, mutation => mutation.Donors.First());
        var mriImageFilters = new MriImageFilters<MutationIndex>(criteria.MriImageFilters, mutation => mutation.Donors.First().Images.First());
        var tissueFilters = new TissueFilters<MutationIndex>(criteria.TissueFilters, mutation => mutation.Donors.First().Specimens.First());
        var cellLineFilters = new CellLineFilters<MutationIndex>(criteria.CellLineFilters, mutation => mutation.Donors.First().Specimens.First());
        var organoidFilters = new OrganoidFilters<MutationIndex>(criteria.OrganoidFilters, mutation => mutation.Donors.First().Specimens.First());
        var xenograftFilters = new XenograftFilters<MutationIndex>(criteria.XenograftFilters, mutation => mutation.Donors.First().Specimens.First());
        var geneFilters = new GeneFilters<MutationIndex>(criteria.GeneFilters, mutation => mutation.AffectedTranscripts.First().Transcript.Gene);
        var mutationFilters = new MutationFilters<MutationIndex>(criteria.MutationFilters, mutation => mutation);

        _filters.AddRange(donorFilters.All());
        _filters.AddRange(mriImageFilters.All());
        _filters.AddRange(tissueFilters.All());
        _filters.AddRange(cellLineFilters.All());
        _filters.AddRange(organoidFilters.All());
        _filters.AddRange(xenograftFilters.All());
        _filters.AddRange(geneFilters.All());
        _filters.AddRange(mutationFilters.All());

        if (criteria.ImageFilters != null)
        {
            _filters.Add(new EqualityFilter<MutationIndex, int>(
              ImageFilterNames.Id,
              mutation => mutation.Donors.First().Images.First().Id,
              criteria.ImageFilters.Id)
            );
        }

        if (criteria.SpecimenFilters != null)
        {
            _filters.Add(new EqualityFilter<MutationIndex, int>(
                SpecimenFilterNames.Id,
                mutation => mutation.Donors.First().Specimens.First().Id,
                criteria.SpecimenFilters.Id)
            );
        }
    }
}
