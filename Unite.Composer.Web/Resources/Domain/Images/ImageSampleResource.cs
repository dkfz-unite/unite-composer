using Unite.Essentials.Extensions;
using Unite.Indices.Entities.Images;

namespace Unite.Composer.Web.Resources.Domain.Images;

public class ImageSampleResource : Basic.SampleResource
{
    public ImageSampleResource(SpecimenIndex specimenIndex, SampleIndex[] sampleIndices = null) : base(specimenIndex, sampleIndices)
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
