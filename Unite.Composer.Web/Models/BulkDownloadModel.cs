using Unite.Composer.Download.Models;
using Unite.Composer.Search.Services.Criteria;

namespace Unite.Composer.Web.Models;

public record BulkDownloadModel
{
    public SearchCriteria Criteria { get; init; }
    public DataTypes Data { get; init; }
}
