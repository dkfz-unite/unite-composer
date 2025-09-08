using Unite.Composer.Download.Models;

namespace Unite.Composer.Web.Models;

public record SingleDownloadModel
{
    public DataTypesCriteria Data { get; init; }
}
