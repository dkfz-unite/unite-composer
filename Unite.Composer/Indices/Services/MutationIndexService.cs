using System.Linq;
using Nest;
using Unite.Composer.Indices.Criteria;
using Unite.Composer.Indices.Services.Extensions;
using Unite.Indices.Entities.Mutations;
using Unite.Indices.Services.Configuration.Options;

namespace Unite.Composer.Indices.Services
{
    public class MutationIndexService : IndexService<MutationIndex>
    {
        private const string _keywordSuffix = "keyword";

        protected override string DefaultIndex => "mutations";

        public MutationIndexService(IElasticOptions options) : base(options)
        {
        }

        protected override ISearchRequest<MutationIndex> CreateRequest(SearchCriteria criteria)
        {
            var request = base.CreateRequest(criteria);

            if (criteria.MutationFilters != null)
            {
                request.AddMatchQuery(
                    mutation => mutation.Code,
                    criteria.MutationFilters.Code
                );

                request.AddTermsQuery(
                    mutation => mutation.Type.Suffix(_keywordSuffix),
                    criteria.MutationFilters.MutationType
                );

                request.AddTermsQuery(
                    mutation => mutation.Chromosome.Suffix(_keywordSuffix),
                    criteria.MutationFilters.Chromosome
                );

                request.AddRangeQuery(
                    mutation => mutation.Start,
                    criteria.MutationFilters.Position?.From,
                    criteria.MutationFilters.Position?.To
                );

                request.AddTermsQuery(
                    mutation => mutation.AffectedTranscripts.First().Consequences.First().Impact.Suffix(_keywordSuffix),
                    criteria.MutationFilters.Impact
                );

                request.AddTermsQuery(
                    mutation => mutation.AffectedTranscripts.First().Consequences.First().Type.Suffix(_keywordSuffix),
                    criteria.MutationFilters.Consequence
                );

                request.AddMatchQuery(
                    mutation => mutation.AffectedTranscripts.First().Gene.Symbol,
                    criteria.MutationFilters.Gene
                );
            }

            if (criteria.DonorFilters != null)
            {
                request.AddMatchQuery(
                    mutation => mutation.Donors.First().ReferenceId,
                    criteria.DonorFilters.ReferenceId
                );

                request.AddMatchQuery(
                    mutation => mutation.Donors.First().ClinicalData.Diagnosis,
                    criteria.DonorFilters.Diagnosis
                );

                request.AddTermsQuery(
                    mutation => mutation.Donors.First().ClinicalData.Gender.Suffix(_keywordSuffix),
                    criteria.DonorFilters.Gender
                );

                request.AddRangeQuery(
                    mutation => mutation.Donors.First().ClinicalData.Age,
                    criteria.DonorFilters.Age?.From,
                    criteria.DonorFilters.Age?.To
                );

                request.AddBoolQuery(
                    mutation => mutation.Donors.First().ClinicalData.VitalStatus,
                    criteria.DonorFilters.VitalStatus
                );
            }

            if (criteria.CellLineFilters != null)
            {
                request.AddMatchQuery(
                    mutation => mutation.Donors.First().Specimens.First().ReferenceId,
                    criteria.CellLineFilters.ReferenceId
                );

                request.AddMatchQuery(
                    mutation => mutation.Donors.First().Specimens.First().CellLine.Name,
                    criteria.CellLineFilters.Name
                );

                request.AddTermsQuery(
                    mutation => mutation.Donors.First().Specimens.First().CellLine.Type.Suffix(_keywordSuffix),
                    criteria.CellLineFilters.Type
                );

                request.AddTermsQuery(
                    mutation => mutation.Donors.First().Specimens.First().CellLine.Species.Suffix(_keywordSuffix),
                    criteria.CellLineFilters.Species
                );
            }

            request.OrderBy(
                mutation => mutation.NumberOfDonors,
                SortOrder.Descending
            );

            request.Exclude(
                mutation => mutation.Donors
            );

            return request;
        }
    }
}
