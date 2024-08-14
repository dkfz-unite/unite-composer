using Unite.Essentials.Extensions;
using Unite.Indices.Entities.Donors;

namespace Unite.Composer.Web.Resources.Domain.Donors;

public class DonorSampleResource : Basic.SampleResource
{
    public DonorSampleResource(SpecimenIndex specimenIndex, SampleIndex[] sampleIndices = null) : base(specimenIndex, sampleIndices)
    {
        Analyses = sampleIndices?.Select(GetAnalysisResource).ToArrayOrNull();
    }


    private static Basic.AnalysisResource GetAnalysisResource(SampleIndex index)
    {
        var analysisResource =  Basic.AnalysisResource.CreateFrom(index);
        analysisResource.Data = Basic.AnalysisDataResource.CreateFrom(index.Data);
        analysisResource.Files = Basic.FileResource.CreateFrom(index.Resources);

        return analysisResource;
    }
}
