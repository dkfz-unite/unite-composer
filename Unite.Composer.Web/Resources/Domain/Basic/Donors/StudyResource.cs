﻿using Unite.Indices.Entities.Basic.Donors;

namespace Unite.Composer.Web.Resources.Domain.Basic.Donors;

public class StudyResource
{
    public int Id { get; set; }
    public string Name { get; set; }


    public StudyResource(StudyIndex index)
    {
        Id = index.Id;
        Name = index.Name;
    }
}
