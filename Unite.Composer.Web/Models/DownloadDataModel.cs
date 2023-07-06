using Unite.Composer.Download.Models;
using Unite.Composer.Search.Services.Criteria;

namespace Unite.Composer.Web.Models;

public record DownloadDataModel
{
    public DataTypes Data { get; init; }
    public SearchCriteria Criteria { get; init; }
}
