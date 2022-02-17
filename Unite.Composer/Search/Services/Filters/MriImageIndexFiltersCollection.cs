using Unite.Composer.Search.Engine.Filters;
using Unite.Composer.Search.Services.Criteria;
using Unite.Composer.Search.Services.Filters.Base;
using Unite.Composer.Search.Services.Filters.Constants;
using Unite.Indices.Entities.Images;

namespace Unite.Composer.Search.Services.Filters
{
    public class MriImageIndexFiltersCollection : ImageIndexFiltersCollection
    {
        public MriImageIndexFiltersCollection(SearchCriteria criteria) : base(criteria)
        {
            var filters = new MriImageFilters<ImageIndex>(criteria.MriImageFilters, image => image);

            _filters.AddRange(filters.All());

            Add(new NotNullFilter<ImageIndex, Indices.Entities.Basic.Images.MriImageIndex>(
                ImageFilterNames.Type,
                image => image.MriImage)
            );
        }
    }
}
