using Unite.Composer.Download.Tsv.Mapping.Extensions;
using Unite.Data.Entities.Donors;
using Unite.Data.Entities.Donors.Clinical;
using Unite.Essentials.Tsv;

namespace Unite.Composer.Download.Services.Tsv.Mapping;

public static class DonorMapper
{
    public static ClassMap<Donor> GetDonorMap()
    {
        return new ClassMap<Donor>().MapDonors();
    }

    public static ClassMap<Treatment> GetTreatmentMap()
    {
        return new ClassMap<Treatment>().MapTreatments();
    }
}
