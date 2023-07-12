
using Unite.Data.Entities.Genome.Variants.Enums;

namespace Unite.Composer.Search.Services.Context;

public class VariantSearchContext
{
    public VariantType? VariantType { get; set; }


    public VariantSearchContext()
    {
    }

    public VariantSearchContext(VariantType variantType) : this()
    {
        VariantType = variantType;
    }
}
