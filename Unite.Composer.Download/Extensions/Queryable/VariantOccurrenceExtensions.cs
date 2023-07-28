using Unite.Data.Entities.Genome.Variants;

using SSM = Unite.Data.Entities.Genome.Variants.SSM;
using CNV = Unite.Data.Entities.Genome.Variants.CNV;
using SV = Unite.Data.Entities.Genome.Variants.SV;

namespace Unite.Composer.Download.Extensions.Queryable;

public static class VariantOccurrenceExtensions
{
    public static IQueryable<TVO> FilterByVariantIds<TVO, TV>(this IQueryable<TVO> query, IEnumerable<long> ids)
        where TVO : VariantOccurrence<TV>
        where TV : Variant
    {
        return query.Where(entity => ids.Contains(entity.VariantId));
    }

    public static IQueryable<SSM.VariantOccurrence> FilterByVariantIds(this IQueryable<SSM.VariantOccurrence> query, IEnumerable<long> ids)
    {
        return query.FilterByVariantIds<SSM.VariantOccurrence, SSM.Variant>(ids);
    }

    public static IQueryable<CNV.VariantOccurrence> FilterByVariantIds(this IQueryable<CNV.VariantOccurrence> query, IEnumerable<long> ids)
    {
        return query.FilterByVariantIds<CNV.VariantOccurrence, CNV.Variant>(ids);
    }

    public static IQueryable<SV.VariantOccurrence> FilterByVariantIds(this IQueryable<SV.VariantOccurrence> query, IEnumerable<long> ids)
    {
        return query.FilterByVariantIds<SV.VariantOccurrence, SV.Variant>(ids);
    }


    public static IQueryable<TVO> FilterBySpecimenIds<TVO, TV>(this IQueryable<TVO> query, IEnumerable<int> ids)
        where TVO : VariantOccurrence<TV>
        where TV : Variant
    {
        return query.Where(entity => ids.Contains(entity.AnalysedSample.Sample.SpecimenId));
    }

    public static IQueryable<SSM.VariantOccurrence> FilterBySpecimenIds(this IQueryable<SSM.VariantOccurrence> query, IEnumerable<int> ids)
    {
        return query.FilterBySpecimenIds<SSM.VariantOccurrence, SSM.Variant>(ids);
    }

    public static IQueryable<CNV.VariantOccurrence> FilterBySpecimenIds(this IQueryable<CNV.VariantOccurrence> query, IEnumerable<int> ids)
    {
        return query.FilterBySpecimenIds<CNV.VariantOccurrence, CNV.Variant>(ids);
    }

    public static IQueryable<SV.VariantOccurrence> FilterBySpecimenIds(this IQueryable<SV.VariantOccurrence> query, IEnumerable<int> ids)
    {
        return query.FilterBySpecimenIds<SV.VariantOccurrence, SV.Variant>(ids);
    }


    public static IQueryable<TVO> FilterByAffectedGeneIds<TVO, TV>(this IQueryable<TVO> query, IEnumerable<int> ids)
        where TVO : VariantOccurrence<TV>
        where TV : Variant
    {
        if (typeof(TVO) == typeof(SSM.VariantOccurrence))
        {
            return ((IQueryable<SSM.VariantOccurrence>)query).FilterByAffectedGeneIds(ids).Cast<TVO>();
        }
        else if (typeof(TVO) == typeof(CNV.VariantOccurrence))
        {
            return ((IQueryable<CNV.VariantOccurrence>)query).FilterByAffectedGeneIds(ids).Cast<TVO>();
        }
        else if (typeof(TVO) == typeof(SV.VariantOccurrence))
        {
            return ((IQueryable<SV.VariantOccurrence>)query).FilterByAffectedGeneIds(ids).Cast<TVO>();
        }

        throw new NotSupportedException("Unsupported variant type.");
    }

    public static IQueryable<SSM.VariantOccurrence> FilterByAffectedGeneIds(this IQueryable<SSM.VariantOccurrence> query, IEnumerable<int> ids)
    {
        return query.Where(entity => entity.Variant.AffectedTranscripts.Any(entity => ids.Contains(entity.Feature.GeneId.Value)));
    }

    public static IQueryable<CNV.VariantOccurrence> FilterByAffectedGeneIds(this IQueryable<CNV.VariantOccurrence> query, IEnumerable<int> ids)
    {
        return query.Where(entity => entity.Variant.AffectedTranscripts.Any(entity => ids.Contains(entity.Feature.GeneId.Value)));
    }

    public static IQueryable<SV.VariantOccurrence> FilterByAffectedGeneIds(this IQueryable<SV.VariantOccurrence> query, IEnumerable<int> ids)
    {
        return query.Where(entity => entity.Variant.AffectedTranscripts.Any(entity => ids.Contains(entity.Feature.GeneId.Value)));
    }
}
