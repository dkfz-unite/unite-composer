using Unite.Composer.Web.Resources.Search.Basic.Specimens;
using Unite.Indices.Entities.Basic.Genome.Transcriptomics;
using Unite.Indices.Entities.Basic.Specimens;

namespace Unite.Composer.Web.Resources.Search.Basic.Genome;

public class GeneSpecimenResource : SpecimenResource
{
    public GeneExpressionResource Expression { get; }

    public GeneSpecimenResource(SpecimenIndex specimenIndex, GeneExpressionIndex expressionIndex) : base(specimenIndex)
    {
        if (expressionIndex != null)
        {
            Expression = new GeneExpressionResource(expressionIndex);
        }
    }
}
