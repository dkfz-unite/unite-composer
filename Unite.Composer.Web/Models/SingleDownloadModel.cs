using Unite.Composer.Download.Tsv.Models;

namespace Unite.Composer.Web.Models;

public record SingleDownloadModel
{
    public DataTypesCriteria Data { get; init; }
}
