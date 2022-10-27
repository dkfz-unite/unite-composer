using System.Linq.Expressions;
using Nest;
using Unite.Composer.Search.Engine.Extensions;
using Unite.Composer.Search.Engine.Filters;
using Unite.Composer.Search.Services.Criteria;
using Unite.Composer.Search.Services.Filters.Constants;
using Unite.Indices.Entities.Basic.Specimens;

namespace Unite.Composer.Search.Services.Filters.Base;

public class TissueFilters<TIndex> : SpecimenFilters<TIndex> where TIndex : class //FiltersCollection<TIndex> where TIndex : class
{
    public TissueFilters(TissueCriteria criteria, Expression<Func<TIndex, SpecimenIndex>> path) : base(criteria, path)
    {
        //if (criteria == null)
        //{
        //    return;
        //}

        //Add(new EqualityFilter<TIndex, int>(
        //    SpecimenFilterNames.Id,
        //    path.Join(specimen => specimen.Id),
        //    criteria.Id)
        //);

        Add(new SimilarityFilter<TIndex, string>(
            TissueFilterNames.ReferenceId,
            path.Join(specimen => specimen.Tissue.ReferenceId),
            criteria.ReferenceId)
        );

        Add(new EqualityFilter<TIndex, object>(
            TissueFilterNames.Type,
            path.Join(specimen => specimen.Tissue.Type.Suffix(_keywordSuffix)),
            criteria.Type)
        );

        Add(new EqualityFilter<TIndex, object>(
            TissueFilterNames.TumorType,
            path.Join(specimen => specimen.Tissue.TumorType.Suffix(_keywordSuffix)),
            criteria.TumorType)
        );

        Add(new SimilarityFilter<TIndex, string>(
            TissueFilterNames.Source,
            path.Join(specimen => specimen.Tissue.Source),
            criteria.Source)
        );


        //Add(new EqualityFilter<TIndex, object>(
        //    TissueFilterNames.MgmtStatus,
        //    path.Join(specimen => specimen.Tissue.MolecularData.MgmtStatus.Suffix(_keywordSuffix)),
        //    criteria.MgmtStatus)
        //);

        //Add(new EqualityFilter<TIndex, object>(
        //    TissueFilterNames.IdhStatus,
        //    path.Join(specimen => specimen.Tissue.MolecularData.IdhStatus.Suffix(_keywordSuffix)),
        //    criteria.IdhStatus)
        //);

        //Add(new EqualityFilter<TIndex, object>(
        //    TissueFilterNames.IdhMutation,
        //    path.Join(specimen => specimen.Tissue.MolecularData.IdhMutation.Suffix(_keywordSuffix)),
        //    criteria.IdhMutation)
        //);

        //Add(new EqualityFilter<TIndex, object>(
        //    TissueFilterNames.GeneExpressionSubtype,
        //    path.Join(specimen => specimen.Tissue.MolecularData.GeneExpressionSubtype.Suffix(_keywordSuffix)),
        //    criteria.GeneExpressionSubtype)
        //);

        //Add(new EqualityFilter<TIndex, object>(
        //    TissueFilterNames.MethylationSubtype,
        //    path.Join(specimen => specimen.Tissue.MolecularData.MethylationSubtype.Suffix(_keywordSuffix)),
        //    criteria.MethylationSubtype)
        //);

        //Add(new BooleanFilter<TIndex>(
        //    TissueFilterNames.GcimpMethylation,
        //    path.Join(specimen => specimen.Tissue.MolecularData.GcimpMethylation),
        //    criteria.GcimpMethylation)
        //);
    }
}
