using Unite.Composer.Search.Services.Context.Enums;

namespace Unite.Composer.Search.Services.Context;

public class SpecimenSearchContext
{
    public SpecimenType? SpecimenType { get; set; }


    public SpecimenSearchContext()
    {
    }

    public SpecimenSearchContext(SpecimenType specimenType) : this()
    {
        SpecimenType = specimenType;
    }
}
