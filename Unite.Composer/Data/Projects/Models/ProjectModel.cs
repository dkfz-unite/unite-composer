﻿namespace Unite.Composer.Data.Projects.Models;

public class ProjectModel
{
    public int? Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

    public ProjectDataModel Data { get; set; }
}
