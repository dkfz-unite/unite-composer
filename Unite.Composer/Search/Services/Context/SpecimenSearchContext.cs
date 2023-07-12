
using Unite.Data.Entities.Specimens.Enums;

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
