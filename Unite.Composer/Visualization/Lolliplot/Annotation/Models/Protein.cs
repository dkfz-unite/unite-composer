﻿namespace Unite.Composer.Visualization.Lolliplot.Annotation.Models;

public class Protein
{
    public string Id { get; set; }
    public string Symbol { get; set; }
    public string Description { get; set; }
    public int Length { get; set; }

    public IEnumerable<ProteinDomain> Domains { get; set; }
}
