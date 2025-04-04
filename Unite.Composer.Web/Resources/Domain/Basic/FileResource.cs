using Unite.Essentials.Extensions;

namespace Unite.Composer.Web.Resources.Domain.Basic;

public class FileResource
{
    public string Name { get; set; }
    public string Type { get; set; }
    public string Format { get; set; }
    public string Archive { get; set; }
    public string Url { get; set; }


    public static FileResource CreateFrom(Indices.Entities.Basic.Analysis.ResourceIndex index)
    {
        if (index == null)
            return null;

        return new FileResource
        {
            Name = index.Name,
            Type = index.Type,
            Format = index.Format,
            Archive = index.Archive,
            Url = index.Url
        };
    }

    public static FileResource[] CreateFrom(Indices.Entities.Basic.Analysis.ResourceIndex[] indices)
    {
        if (indices == null)
            return null;

        return indices.Select(CreateFrom).ToArrayOrNull();
    }
}
