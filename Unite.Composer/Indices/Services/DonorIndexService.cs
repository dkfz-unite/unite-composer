using System.Linq;
using Nest;
using Unite.Composer.Indices.Criteria;
using Unite.Composer.Indices.Services.Extensions;
using Unite.Indices.Entities.Donors;
using Unite.Indices.Services.Configuration.Options;

namespace Unite.Composer.Indices.Services
{
    public class DonorIndexService : IndexService<DonorIndex>
    {
        private const string _keywordSuffix = "keyword";

        protected override string DefaultIndex => "donors";

        public DonorIndexService(IElasticOptions options) : base(options)
        {
        }

        protected override ISearchRequest<DonorIndex> CreateRequest(SearchCriteria criteria)
        {
            var request = base.CreateRequest(criteria);

            if (criteria.DonorFilters != null)
            {
                request.AddMatchQuery(
                    donor => donor.Id,
                    criteria.DonorFilters.Id
                );

                request.AddMatchQuery(
                    donor => donor.Diagnosis,
                    criteria.DonorFilters.Diagnosis
                );

                request.AddTermsQuery(
                    donor => donor.ClinicalData.Gender.Suffix(_keywordSuffix),
                    criteria.DonorFilters.Gender
                );

                request.AddRangeQuery(
                    donor => donor.ClinicalData.Age,
                    criteria.DonorFilters.Age?.From,
                    criteria.DonorFilters.Age?.To
                );

                request.AddTermsQuery(
                    donor => donor.ClinicalData.AgeCategory.Suffix(_keywordSuffix),
                    criteria.DonorFilters.AgeCategory
                );

                request.AddTermsQuery(
                    donor => donor.ClinicalData.VitalStatus.Suffix(_keywordSuffix),
                    criteria.DonorFilters.VitalStatus
                );

                request.AddTermsQuery(
                    donor => donor.EpigeneticsData.GeneExpressionSubtype.Suffix(_keywordSuffix),
                    criteria.DonorFilters.GeneExpressionSubtype
                );

                request.AddTermsQuery(
                    donor => donor.EpigeneticsData.IdhStatus.Suffix(_keywordSuffix),
                    criteria.DonorFilters.IdhStatus
                );

                request.AddTermsQuery(
                    donor => donor.EpigeneticsData.IdhMutation.Suffix(_keywordSuffix),
                    criteria.DonorFilters.IdhMutation
                );

                request.AddTermsQuery(
                    donor => donor.EpigeneticsData.MethylationStatus.Suffix(_keywordSuffix),
                    criteria.DonorFilters.MethylationStatus
                );

                request.AddTermsQuery(
                    donor => donor.EpigeneticsData.MethylationSubtype.Suffix(_keywordSuffix),
                    criteria.DonorFilters.MethylationSubtype
                );

                request.AddBoolQuery(
                    donor => donor.EpigeneticsData.GcimpMethylation,
                    criteria.DonorFilters.GcimpMethylation
                );
            }

            if (criteria.MutationFilters != null)
            {

                request.AddMatchQuery(
                    donor => donor.Mutations.First().Code,
                    criteria.MutationFilters.Code
                );

                request.AddTermsQuery(
                    donor => donor.Mutations.First().SequenceType.Suffix(_keywordSuffix),
                    criteria.MutationFilters.SequenceType
                );

                request.AddTermsQuery(
                    donor => donor.Mutations.First().Type.Suffix(_keywordSuffix),
                    criteria.MutationFilters.MutationType
                );

                request.AddTermsQuery(
                    donor => donor.Mutations.First().Chromosome.Suffix(_keywordSuffix),
                    criteria.MutationFilters.Chromosome
                );

                request.AddRangeQuery(
                    donor => donor.Mutations.First().Start,
                    criteria.MutationFilters.Position?.From,
                    criteria.MutationFilters.Position?.To
                );

                request.AddTermsQuery(
                    donor => donor.Mutations.First().AffectedTranscripts.First().Consequences.First().Impact.Suffix(_keywordSuffix),
                    criteria.MutationFilters.Impact
                );

                request.AddTermsQuery(
                    donor => donor.Mutations.First().AffectedTranscripts.First().Consequences.First().Type.Suffix(_keywordSuffix),
                    criteria.MutationFilters.Consequence
                );

                request.AddMatchQuery(
                    donor => donor.Mutations.First().AffectedTranscripts.First().Gene.Symbol,
                    criteria.MutationFilters.Gene
                );
            }

            request.OrderBy(
                donor => donor.NumberOfMutations,
                SortOrder.Descending
            );

            request.Exclude(
                donor => donor.Mutations
            );

            return request;
        }
    }
}
