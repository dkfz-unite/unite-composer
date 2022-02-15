using Unite.Composer.Search.Services.Criteria;
using Unite.Composer.Search.Services.Filters.Base;
using Unite.Indices.Entities.Specimens;

namespace Unite.Composer.Search.Services
{
    public class TissueIndexFiltersCollection : SpecimenIndexFiltersCollection
    {
        public TissueIndexFiltersCollection(SearchCriteria criteria) : base(criteria)
        {
            var filters = new TissueFilters<SpecimenIndex>(criteria.TissueFilters, specimen => specimen);

            _filters.AddRange(filters.All());


            //_filters.Add(new NotNullFilter<SpecimenIndex, Indices.Entities.Basic.Specimens.TissueIndex>(
            //    SpecimenFilterNames.Type,
            //    specimen => specimen.Tissue)
            //);

            //if (criteria.TissueFilters != null)
            //{
            //    _filters.Add(new EqualityFilter<SpecimenIndex, int>(
            //        SpecimenFilterNames.Id,
            //        specimen => specimen.Id,
            //        criteria.TissueFilters.Id)
            //    );

            //    _filters.Add(new SimilarityFilter<SpecimenIndex, string>(
            //        TissueFilterNames.ReferenceId,
            //        specimen => specimen.Tissue.ReferenceId,
            //        criteria.TissueFilters.ReferenceId)
            //    );

            //    _filters.Add(new EqualityFilter<SpecimenIndex, object>(
            //        TissueFilterNames.Type,
            //        specimen => specimen.Tissue.Type.Suffix(_keywordSuffix),
            //        criteria.TissueFilters.Type)
            //    );

            //    _filters.Add(new EqualityFilter<SpecimenIndex, object>(
            //        TissueFilterNames.TumorType,
            //        specimen => specimen.Tissue.TumorType.Suffix(_keywordSuffix),
            //        criteria.TissueFilters.TumorType)
            //    );

            //    _filters.Add(new SimilarityFilter<SpecimenIndex, string>(
            //        TissueFilterNames.Source,
            //        specimen => specimen.Tissue.Source,
            //        criteria.TissueFilters.Source)
            //    );


            //    _filters.Add(new EqualityFilter<SpecimenIndex, object>(
            //        TissueFilterNames.MgmtStatus,
            //        specimen => specimen.Tissue.MolecularData.MgmtStatus.Suffix(_keywordSuffix),
            //        criteria.TissueFilters.MgmtStatus)
            //    );

            //    _filters.Add(new EqualityFilter<SpecimenIndex, object>(
            //        TissueFilterNames.IdhStatus,
            //        specimen => specimen.Tissue.MolecularData.IdhStatus.Suffix(_keywordSuffix),
            //        criteria.TissueFilters.IdhStatus)
            //    );

            //    _filters.Add(new EqualityFilter<SpecimenIndex, object>(
            //        TissueFilterNames.IdhMutation,
            //        specimen => specimen.Tissue.MolecularData.IdhMutation.Suffix(_keywordSuffix),
            //        criteria.TissueFilters.IdhMutation)
            //    );

            //    _filters.Add(new EqualityFilter<SpecimenIndex, object>(
            //        TissueFilterNames.GeneExpressionSubtype,
            //        specimen => specimen.Tissue.MolecularData.GeneExpressionSubtype.Suffix(_keywordSuffix),
            //        criteria.TissueFilters.GeneExpressionSubtype)
            //    );

            //    _filters.Add(new EqualityFilter<SpecimenIndex, object>(
            //        TissueFilterNames.MethylationSubtype,
            //        specimen => specimen.Tissue.MolecularData.MethylationSubtype.Suffix(_keywordSuffix),
            //        criteria.TissueFilters.MethylationSubtype)
            //    );

            //    _filters.Add(new BooleanFilter<SpecimenIndex>(
            //        TissueFilterNames.GcimpMethylation,
            //        specimen => specimen.Tissue.MolecularData.GcimpMethylation,
            //        criteria.TissueFilters.GcimpMethylation)
            //    );
            //}
        }
    }
}
