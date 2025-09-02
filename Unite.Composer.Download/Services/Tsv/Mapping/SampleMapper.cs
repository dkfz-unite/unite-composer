using Unite.Data.Entities.Omics.Analysis;
using Unite.Essentials.Extensions;
using Unite.Essentials.Tsv;

namespace Unite.Composer.Download.Services.Tsv.Mapping;

public static class SampleMapper
{
    public static ClassMap<Sample> GetSampleMap()
    {
        return new ClassMap<Sample>()
            .Map(entity => entity.Specimen.Donor.ReferenceId, "donor_id")
            .Map(entity => entity.Specimen.ReferenceId, "specimen_id")
            .Map(entity => entity.Specimen.TypeId, "specimen_type")
            .Map(entity => entity.MatchedSample.Specimen.ReferenceId, "matched_specimen_id")
            .Map(entity => entity.MatchedSample.Specimen.TypeId, "matched_specimen_type")
            .Map(entity => entity.Analysis.TypeId, "analysis_type")
            .Map(entity => entity.Analysis.Day, "analysis_day")
            .Map(entity => entity.Purity, "purity")
            .Map(entity => entity.Ploidy, "ploidy")
            .Map(entity => entity.Genome, "genome")
            .Map(entity => entity.Cells, "cells");
    }

    public static string[] MapSample(Sample sample)
    {
        var comments = new List<string>();

        TryMapField(comments, "donor_id", sample.Specimen.Donor.ReferenceId);
        TryMapField(comments, "specimen_id", sample.Specimen.ReferenceId);
        TryMapField(comments, "specimen_type", sample.Specimen.TypeId);
        TryMapField(comments, "matched_specimen_id", sample.MatchedSample?.Specimen.ReferenceId);
        TryMapField(comments, "matched_specimen_type", sample.MatchedSample?.Specimen.TypeId);
        TryMapField(comments, "analysis_type", sample.Analysis.TypeId.ToDefinitionString());
        TryMapField(comments, "analysis_day", sample.Analysis.Day);
        TryMapField(comments, "purity", sample.Purity);
        TryMapField(comments, "ploidy", sample.Ploidy);
        TryMapField(comments, "genome", sample.Genome);
        TryMapField(comments, "cells", sample.Cells);

        return comments.ToArray();
    }

    private static void TryMapField<T>(List<string> fields, string fieldName, T value)
    {
        if (value != null)
            fields.Add($"{fieldName}: {value}");
    }
}
