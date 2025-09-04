using Unite.Composer.Download.Tsv.Mapping.Extensions;
using Unite.Data.Entities.Specimens;
using Unite.Data.Entities.Specimens.Enums;
using Unite.Essentials.Tsv;

namespace Unite.Composer.Download.Services.Tsv.Mapping;

public static class SpecimenMapper
{

    public static ClassMap<Specimen> GetSpecimenMap(SpecimenType type)
    {
        if (type == SpecimenType.Material)
            return GetMaterialMap();
        else if (type == SpecimenType.Line)
            return GetLineMap();
        else if (type == SpecimenType.Organoid)
            return GetOrganoidMap();
        else if (type == SpecimenType.Xenograft)
            return GetXenograftMap();
        else
            throw new NotSupportedException($"Specimen type '{type}' is not supported.");
    }

    public static ClassMap<Specimen> GetMaterialMap()
    {
        return new ClassMap<Specimen>().MapMaterials();
    }

    public static ClassMap<Specimen> GetLineMap()
    {
        return new ClassMap<Specimen>().MapLines();
    }

    public static ClassMap<Specimen> GetOrganoidMap()
    {
        return new ClassMap<Specimen>().MapOrganoids();
    }

    public static ClassMap<Specimen> GetXenograftMap()
    {
        return new ClassMap<Specimen>().MapXenografts();
    }

    public static ClassMap<Intervention> GetInterventionMap()
    {
        return new ClassMap<Intervention>().MapInterventions();
    }
}
