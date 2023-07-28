using System.IO.Compression;

namespace Unite.Composer.Download.Tsv;

public class TsvDownloadService
{
    protected static Task CreateArchiveEntry(ZipArchive arhive, string name, string content)
    {
        var file = arhive.CreateEntry(name);
        using var stream = file.Open();
        using var writer = new StreamWriter(stream);

        return writer.WriteAsync(content);
    }

    protected static async Task CreateArchiveEntry(ZipArchive arhive, string name, Task<string> task)
    {
        var content = await task;

        if (!string.IsNullOrEmpty(content))
        {
            var file = arhive.CreateEntry(name);
            using var stream = file.Open();
            using var writer = new StreamWriter(stream);

            await writer.WriteAsync(content);
        }
    }
}
