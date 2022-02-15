using System;
using System.Linq;
using System.Linq.Expressions;
using Nest;
using Unite.Composer.Search.Engine.Extensions;
using Unite.Composer.Search.Engine.Filters;
using Unite.Composer.Search.Services.Criteria;
using Unite.Composer.Search.Services.Filters.Constants;
using Unite.Indices.Entities.Basic.Donors;

namespace Unite.Composer.Search.Services.Filters.Base
{
    public class DonorFilters<TIndex> : FiltersCollection<TIndex>
        where TIndex : class
    {
        public DonorFilters(DonorCriteria criteria, Expression<Func<TIndex, DonorIndex>> path)
        {
            if (criteria == null)
            {
                return;
            }

            _filters.Add(new EqualityFilter<TIndex, int>(
                DonorFilterNames.Id,
                path.Join(donor => donor.Id),
                criteria.Id)
            );

            _filters.Add(new SimilarityFilter<TIndex, string>(
                DonorFilterNames.ReferenceId,
                path.Join(donor => donor.ReferenceId),
                criteria.ReferenceId)
            );

            _filters.Add(new SimilarityFilter<TIndex, string>(
                DonorFilterNames.Diagnosis,
                path.Join(donor => donor.ClinicalData.Diagnosis),
                criteria.Diagnosis)
            );

            _filters.Add(new EqualityFilter<TIndex, object>(
                DonorFilterNames.Gender,
                path.Join(donor => donor.ClinicalData.Gender.Suffix(_keywordSuffix)),
                criteria.Gender)
            );

            _filters.Add(new RangeFilter<TIndex, int?>(
                DonorFilterNames.Age,
                path.Join(donor => donor.ClinicalData.Age),
                criteria.Age?.From,
                criteria.Age?.To)
            );

            _filters.Add(new BooleanFilter<TIndex>(
                DonorFilterNames.VitalStatus,
                path.Join(donor => donor.ClinicalData.VitalStatus),
                criteria.VitalStatus)
            );

            _filters.Add(new SimilarityFilter<TIndex, object>(
               DonorFilterNames.Therapy,
               path.Join(donor => donor.Treatments.First().Therapy.Suffix(_keywordSuffix)),
               criteria.Therapy)
           );

            _filters.Add(new BooleanFilter<TIndex>(
                DonorFilterNames.MtaProtected,
                path.Join(donor => donor.MtaProtected),
                criteria.MtaProtected)
            );

            _filters.Add(new SimilarityFilter<TIndex, object>(
               DonorFilterNames.WorkPackage,
               path.Join(donor => donor.WorkPackages.First().Name.Suffix(_keywordSuffix)),
               criteria.WorkPackage)
           );
        }
    }
}
