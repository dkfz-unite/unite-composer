using Unite.Composer.Web.Resources.Domain.Basic.Specimens;
using Unite.Indices.Entities.Basic.Genome.Transcriptomics;
using Unite.Indices.Entities.Basic.Specimens;

namespace Unite.Composer.Web.Resources.Domain.Basic.Genome;

public class GeneSpecimenResource : SpecimenResource
{
    public BulkExpressionResource Expression { get; }

    public GeneSpecimenResource(SpecimenIndex specimenIndex, BulkExpressionIndex expressionIndex) : base(specimenIndex)
    {
        if (expressionIndex != null)
            Expression = new BulkExpressionResource(expressionIndex);
    }
}
