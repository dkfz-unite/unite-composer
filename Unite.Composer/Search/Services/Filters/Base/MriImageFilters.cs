using System.Linq.Expressions;
using Unite.Composer.Search.Engine.Extensions;
using Unite.Composer.Search.Engine.Filters;
using Unite.Composer.Search.Services.Criteria;
using Unite.Composer.Search.Services.Filters.Constants;
using Unite.Indices.Entities.Basic.Images;

namespace Unite.Composer.Search.Services.Filters.Base;

public class MriImageFilters<TIndex> : FiltersCollection<TIndex> where TIndex : class
{
    public MriImageFilters(in MriImageCriteria criteria, in Expression<Func<TIndex, ImageIndex>> path) : base()
    {
        if (criteria == null)
        {
            return;
        }

        Add(new EqualityFilter<TIndex, int>(
            ImageFilterNames.Id,
            path.Join(image => image.Id),
            criteria.Id)
        );

        Add(new SimilarityFilter<TIndex, string>(
            MriImageFilterNames.ReferenceId,
            path.Join(image => image.MriImage.ReferenceId),
            criteria.ReferenceId)
        );

        Add(new RangeFilter<TIndex, double?>(
            MriImageFilterNames.WholeTumor,
            path.Join(image => image.MriImage.WholeTumor),
            criteria.WholeTumor.From,
            criteria.WholeTumor.To
        ));

        Add(new RangeFilter<TIndex, double?>(
            MriImageFilterNames.ContrastEnhancing,
            path.Join(image => image.MriImage.ContrastEnhancing),
            criteria.ContrastEnhancing.From,
            criteria.ContrastEnhancing.To
        ));

        Add(new RangeFilter<TIndex, double?>(
            MriImageFilterNames.NonContrastEnhancing,
            path.Join(image => image.MriImage.NonContrastEnhancing),
            criteria.NonContrastEnhancing.From,
            criteria.NonContrastEnhancing.To
        ));
    }
}
