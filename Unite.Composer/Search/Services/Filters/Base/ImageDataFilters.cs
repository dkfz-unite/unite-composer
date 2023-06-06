using System.Linq.Expressions;
using Unite.Composer.Search.Engine.Extensions;
using Unite.Composer.Search.Engine.Filters;
using Unite.Composer.Search.Services.Criteria;
using Unite.Composer.Search.Services.Filters.Constants;
using Unite.Indices.Entities.Images;

namespace Unite.Composer.Search.Services.Filters.Base;

public class ImageDataFilters<TIndex> : FiltersCollection<TIndex> where TIndex : class
{
    public ImageDataFilters(ImageCriteriaBase criteria, Expression<Func<TIndex, DataIndex>> path)
    {
        _filters.Add(new BooleanFilter<TIndex>(
            ImageFilterNames.HasSsms,
            path.Join(data => data.Ssms),
            criteria.HasSsms)
        );

        _filters.Add(new BooleanFilter<TIndex>(
            ImageFilterNames.HasCnvs,
            path.Join(data => data.Cnvs),
            criteria.HasCnvs)
        );

        _filters.Add(new BooleanFilter<TIndex>(
            ImageFilterNames.HasSvs,
            path.Join(data => data.Svs),
            criteria.HasSvs)
        );

        _filters.Add(new BooleanFilter<TIndex>(
            ImageFilterNames.HasGeneExp,
            path.Join(data => data.GeneExp),
            criteria.HasGeneExp)
        );
    }
}
