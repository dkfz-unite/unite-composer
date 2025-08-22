using System.IO.Compression;
using System.Text;

namespace Unite.Composer.Download.Tsv;

public abstract class TsvDownloadService
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
        var contentStream = new MemoryStream(Encoding.UTF8.GetBytes(content));

        if (!string.IsNullOrEmpty(content))
        {
            var entry = arhive.CreateEntry(name);
            await using var entryStream = entry.Open();
            await contentStream.CopyToAsync(entryStream);
        }
    }
}
