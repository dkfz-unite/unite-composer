﻿using Unite.Indices.Entities.Variants;

namespace Unite.Composer.Web.Resources.Domain.Variants;

public class VariantResource : Basic.Genome.Variants.VariantResource
{
    public int NumberOfDonors { get; }
    public int NumberOfMris { get; set; }
    public int NumberOfCts { get; set; }
    public int NumberOfMaterials { get; set; }
    public int NumberOfLines { get; set; }
    public int NumberOfOrganoids { get; set; }
    public int NumberOfXenografts { get; set; }
    public int NumberOfGenes { get; set; }

    public VariantDataResource Data { get; set; }


    public VariantResource(VariantIndex index, bool includeAffectedFeatures = false) : base(index, includeAffectedFeatures)
    {
        NumberOfDonors = index.NumberOfDonors;
        NumberOfMris = index.NumberOfMris;
        NumberOfCts = index.NumberOfCts;
        NumberOfMaterials = index.NumberOfMaterials;
        NumberOfLines = index.NumberOfLines;
        NumberOfOrganoids = index.NumberOfOrganoids;
        NumberOfXenografts = index.NumberOfXenografts;
        NumberOfGenes = index.NumberOfGenes;

        if (index.Data != null)
        {
            Data = new VariantDataResource(index.Data, index.Type);
        }
    }
}
