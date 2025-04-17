using Unite.Composer.Download.Tsv.Models;
using Unite.Indices.Search.Services.Filters.Criteria;

namespace Unite.Composer.Web.Models;

public record BulkDownloadModel
{
    public SearchCriteria Criteria { get; init; }
    public DataTypesCriteria Data { get; init; }
}
