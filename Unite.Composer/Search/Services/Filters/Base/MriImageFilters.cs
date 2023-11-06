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

        if (IsNotEmpty(criteria.Id))
        {
            Add(new EqualityFilter<TIndex, int>(
                ImageFilterNames.Id,
                path.Join(image => image.Id),
                criteria.Id)
            );
        }

        if (IsNotEmpty(criteria.ReferenceId))
        {
            Add(new SimilarityFilter<TIndex, string>(
                MriImageFilterNames.ReferenceId,
                path.Join(image => image.Mri.ReferenceId),
                criteria.ReferenceId)
            );
        }

        if (IsNotEmpty(criteria.WholeTumor))
        {
            Add(new RangeFilter<TIndex, double?>(
                MriImageFilterNames.WholeTumor,
                path.Join(image => image.Mri.WholeTumor),
                criteria.WholeTumor.From,
                criteria.WholeTumor.To
            ));
        }

        if (IsNotEmpty(criteria.ContrastEnhancing))
        {
            Add(new RangeFilter<TIndex, double?>(
                MriImageFilterNames.ContrastEnhancing,
                path.Join(image => image.Mri.ContrastEnhancing),
                criteria.ContrastEnhancing.From,
                criteria.ContrastEnhancing.To
            ));
        }

        if (IsNotEmpty(criteria.NonContrastEnhancing))
        {
            Add(new RangeFilter<TIndex, double?>(
                MriImageFilterNames.NonContrastEnhancing,
                path.Join(image => image.Mri.NonContrastEnhancing),
                criteria.NonContrastEnhancing.From,
                criteria.NonContrastEnhancing.To
            ));
        }
    }
}
