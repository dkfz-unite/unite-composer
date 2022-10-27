using System.Linq.Expressions;
using Unite.Composer.Search.Engine.Extensions;
using Unite.Composer.Search.Engine.Filters;
using Unite.Composer.Search.Services.Criteria;
using Unite.Composer.Search.Services.Filters.Constants;
using Unite.Indices.Entities.Basic.Specimens;

namespace Unite.Composer.Search.Services.Filters.Base;

public class OrganoidFilters<TIndex> : SpecimenFilters<TIndex> where TIndex : class //FiltersCollection<TIndex> where TIndex : class
{
    public OrganoidFilters(OrganoidCriteria criteria, Expression<Func<TIndex, SpecimenIndex>> path) : base(criteria, path)
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
            OrganoidFilterNames.ReferenceId,
            path.Join(specimen => specimen.Organoid.ReferenceId),
            criteria.ReferenceId)
        );

        Add(new SimilarityFilter<TIndex, string>(
            OrganoidFilterNames.Medium,
            path.Join(specimen => specimen.Organoid.Medium),
            criteria.Medium)
        );

        Add(new BooleanFilter<TIndex>(
            OrganoidFilterNames.Tumorigenicity,
            path.Join(specimen => specimen.Organoid.Tumorigenicity),
            criteria.Tumorigenicity)
        );

        Add(new SimilarityFilter<TIndex, string>(
            OrganoidFilterNames.Intervention,
            path.Join(specimen => specimen.Organoid.Interventions.First().Type),
            criteria.Intervention)
        );


        //Add(new EqualityFilter<TIndex, object>(
        //    OrganoidFilterNames.MgmtStatus,
        //    path.Join(specimen => specimen.Organoid.MolecularData.MgmtStatus.Suffix(_keywordSuffix)),
        //    criteria.MgmtStatus)
        //);

        //Add(new EqualityFilter<TIndex, object>(
        //    OrganoidFilterNames.IdhStatus,
        //    path.Join(specimen => specimen.Organoid.MolecularData.IdhStatus.Suffix(_keywordSuffix)),
        //    criteria.IdhStatus)
        //);

        //Add(new EqualityFilter<TIndex, object>(
        //    OrganoidFilterNames.IdhMutation,
        //    path.Join(specimen => specimen.Organoid.MolecularData.IdhMutation.Suffix(_keywordSuffix)),
        //    criteria.IdhMutation)
        //);

        //Add(new EqualityFilter<TIndex, object>(
        //    OrganoidFilterNames.GeneExpressionSubtype,
        //    path.Join(specimen => specimen.Organoid.MolecularData.GeneExpressionSubtype.Suffix(_keywordSuffix)),
        //    criteria.GeneExpressionSubtype)
        //);

        //Add(new EqualityFilter<TIndex, object>(
        //    OrganoidFilterNames.MethylationSubtype,
        //    path.Join(specimen => specimen.Organoid.MolecularData.MethylationSubtype.Suffix(_keywordSuffix)),
        //    criteria.MethylationSubtype)
        //);

        //Add(new BooleanFilter<TIndex>(
        //    OrganoidFilterNames.GcimpMethylation,
        //    path.Join(specimen => specimen.Organoid.MolecularData.GcimpMethylation),
        //    criteria.GcimpMethylation)
        //);


        //Add(new SimilarityFilter<TIndex, string>(
        //    OrganoidFilterNames.Drug,
        //    path.Join(specimen => specimen.Organoid.DrugScreenings.First().Drug),
        //    criteria.Drug)
        //);

        //Add(new RangeFilter<TIndex, double?>(
        //    OrganoidFilterNames.Dss,
        //    path.Join(specimen => specimen.Organoid.DrugScreenings.First().Dss),
        //    criteria.Dss.From,
        //    criteria.Dss.To)
        //);

        //Add(new RangeFilter<TIndex, double?>(
        //    OrganoidFilterNames.DssSelective,
        //    path.Join(specimen => specimen.Organoid.DrugScreenings.First().DssSelective),
        //    criteria.DssSelective.From,
        //    criteria.DssSelective.To)
        //);
    }
}
