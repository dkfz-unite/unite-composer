using Unite.Essentials.Extensions;
using Unite.Indices.Entities.Basic.Analysis;
using Unite.Indices.Entities.Basic.Specimens;

namespace Unite.Composer.Web.Resources.Domain.Basic;

public class SampleResource
{
    public int Id { get; set; }
    public string ReferenceId { get; set; }
    public string Type { get; set; }

    public AnalysisResource[] Analyses { get; set; }


    public SampleResource(SpecimenIndex specimenIndex, SampleIndex[] sampleIndices = null)
    {
        Id = specimenIndex.Id;
        ReferenceId = specimenIndex.ReferenceId;
        Type = specimenIndex.Type;

        Analyses = sampleIndices?.Select(GetAnalysisResource).ToArrayOrNull();
    }


    private static AnalysisResource GetAnalysisResource(SampleIndex index)
    {
        return AnalysisResource.CreateFrom(index);
    }
}
