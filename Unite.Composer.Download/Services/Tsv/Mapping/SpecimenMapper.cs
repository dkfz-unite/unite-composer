using Unite.Composer.Download.Tsv.Mapping.Extensions;
using Unite.Data.Entities.Specimens;
using Unite.Essentials.Tsv;

namespace Unite.Composer.Download.Services.Tsv.Mapping;

public static class SpecimenMapper
{
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
