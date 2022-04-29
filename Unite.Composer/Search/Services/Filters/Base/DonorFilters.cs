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

            Add(new EqualityFilter<TIndex, int>(
                DonorFilterNames.Id,
                path.Join(donor => donor.Id),
                criteria.Id)
            );

            Add(new SimilarityFilter<TIndex, string>(
                DonorFilterNames.ReferenceId,
                path.Join(donor => donor.ReferenceId),
                criteria.ReferenceId)
            );

            Add(new SimilarityFilter<TIndex, string>(
                DonorFilterNames.Diagnosis,
                path.Join(donor => donor.ClinicalData.Diagnosis),
                criteria.Diagnosis)
            );

            Add(new EqualityFilter<TIndex, object>(
                DonorFilterNames.Gender,
                path.Join(donor => donor.ClinicalData.Gender.Suffix(_keywordSuffix)),
                criteria.Gender)
            );

            Add(new RangeFilter<TIndex, int?>(
                DonorFilterNames.Age,
                path.Join(donor => donor.ClinicalData.Age),
                criteria.Age?.From,
                criteria.Age?.To)
            );

            Add(new BooleanFilter<TIndex>(
                DonorFilterNames.VitalStatus,
                path.Join(donor => donor.ClinicalData.VitalStatus),
                criteria.VitalStatus)
            );

            Add(new BooleanFilter<TIndex>(
                DonorFilterNames.ProgressionStatus,
                path.Join(donor => donor.ClinicalData.ProgressionStatus),
                criteria.VitalStatus)
            );

            Add(new SimilarityFilter<TIndex, object>(
               DonorFilterNames.Therapy,
               path.Join(donor => donor.Treatments.First().Therapy.Suffix(_keywordSuffix)),
               criteria.Therapy)
           );

            Add(new BooleanFilter<TIndex>(
                DonorFilterNames.MtaProtected,
                path.Join(donor => donor.MtaProtected),
                criteria.MtaProtected)
            );

            Add(new SimilarityFilter<TIndex, object>(
               DonorFilterNames.WorkPackage,
               path.Join(donor => donor.WorkPackages.First().Name.Suffix(_keywordSuffix)),
               criteria.WorkPackage)
           );
        }
    }
}
