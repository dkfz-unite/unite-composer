using Unite.Composer.Download.Tsv.Mapping.Extensions;
using Unite.Data.Entities.Specimens.Analysis;
using Unite.Data.Entities.Specimens.Analysis.Drugs;
using Unite.Essentials.Extensions;
using Unite.Essentials.Tsv;

namespace Unite.Composer.Download.Services.Tsv.Mapping;

public static class SpecimenAnalysisMapper
{
    public static ClassMap<Sample> GetSampleMap()
    {
        return new ClassMap<Sample>()
            .Map(entity => entity.Specimen.DonorId, "donor_id")
            .Map(entity => entity.Specimen.ReferenceId, "specimen_id")
            .Map(entity => entity.Specimen.TypeId, "specimen_type")
            .Map(entity => entity.Analysis.TypeId, "analysis_type")
            .Map(entity => entity.Analysis.Day, "analysis_day");
    }

    public static string[] MapSample(Sample sample)
    {
        var comments = new List<string>();

        TryMapField(comments, "donor_id", sample.Specimen.Donor.ReferenceId);
        TryMapField(comments, "specimen_id", sample.Specimen.ReferenceId);
        TryMapField(comments, "specimen_type", sample.Specimen.TypeId);
        TryMapField(comments, "analysis_type", sample.Analysis.TypeId.ToDefinitionString());
        TryMapField(comments, "analysis_day", sample.Analysis.Day);

        return comments.ToArray();
    }


    public static ClassMap<DrugScreening> GetDrugScreeningMap()
    {
        return new ClassMap<DrugScreening>().MapDrugScreenings();
    }


    private static void TryMapField<T>(List<string> fields, string fieldName, T value)
    {
        if (value != null)
            fields.Add($"{fieldName}: {value}");
    }
}
