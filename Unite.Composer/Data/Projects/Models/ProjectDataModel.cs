namespace Unite.Composer.Data.Projects.Models;

public class ProjectDataModel
{
    public int? Total { get; set; }

    public int? MRI { get; set; }
    public int? CT { get; set; }

    public int? Tissues { get; set; }
    public int? Cells { get; set; }
    public int? Organoids { get; set; }
    public int? Xenografts { get; set; }

    public int? SSM { get; set; }
    public int? CNV { get; set; }
    public int? SV { get; set; }
}
