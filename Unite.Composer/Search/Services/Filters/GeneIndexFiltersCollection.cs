using System.Linq;
using Unite.Composer.Search.Engine.Filters;
using Unite.Composer.Search.Services.Criteria;
using Unite.Composer.Search.Services.Filters.Base;
using Unite.Composer.Search.Services.Filters.Constants;
using Unite.Indices.Entities.Genes;

namespace Unite.Composer.Search.Services.Filters
{
    public class GeneIndexFiltersCollection : FiltersCollection<GeneIndex>
    {
        public GeneIndexFiltersCollection(SearchCriteria criteria) : base()
        {
            var donorFilters = new DonorFilters<GeneIndex>(criteria.DonorFilters, gene => gene.Mutations.First().Donors.First());
            var mriImageFilters = new MriImageFilters<GeneIndex>(criteria.MriImageFilters, gene => gene.Mutations.First().Donors.First().Images.First());
            var tissueFilters = new TissueFilters<GeneIndex>(criteria.TissueFilters, gene => gene.Mutations.First().Donors.First().Specimens.First());
            var cellLineFilters = new CellLineFilters<GeneIndex>(criteria.CellLineFilters, gene => gene.Mutations.First().Donors.First().Specimens.First());
            var organoidFilters = new OrganoidFilters<GeneIndex>(criteria.OrganoidFilters, gene => gene.Mutations.First().Donors.First().Specimens.First());
            var xenograftFilters = new XenograftFilters<GeneIndex>(criteria.XenograftFilters, gene => gene.Mutations.First().Donors.First().Specimens.First());
            var geneFilters = new GeneFilters<GeneIndex>(criteria.GeneFilters, gene => gene);
            var mutationFilters = new MutationFilters<GeneIndex>(criteria.MutationFilters, gene => gene.Mutations.First());

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
                _filters.Add(new EqualityFilter<GeneIndex, int>(
                  ImageFilterNames.Id,
                  gene => gene.Mutations.First().Donors.First().Images.First().Id,
                  criteria.ImageFilters.Id)
                );
            }

            if (criteria.SpecimenFilters != null)
            {
                _filters.Add(new EqualityFilter<GeneIndex, int>(
                    SpecimenFilterNames.Id,
                    gene => gene.Mutations.First().Donors.First().Specimens.First().Id,
                    criteria.SpecimenFilters.Id)
                );
            }
        }
    }
}
