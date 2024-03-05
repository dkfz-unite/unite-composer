using System.IO.Compression;
using Unite.Composer.Download.Models;
using Unite.Composer.Download.Tsv.Constants;
using Unite.Composer.Download.Tsv.Mapping;
using Unite.Data.Entities.Genome.Variants;
using Unite.Data.Entities.Genome.Variants.Enums;
using Unite.Data.Entities.Images.Enums;
using Unite.Data.Entities.Specimens.Enums;

using SSM = Unite.Data.Entities.Genome.Variants.SSM;
using CNV = Unite.Data.Entities.Genome.Variants.CNV;
using SV = Unite.Data.Entities.Genome.Variants.SV;

namespace Unite.Composer.Download.Tsv;

public class VariantsTsvDownloadService : TsvDownloadService
{
    private readonly DonorsTsvService _donorsTsvService;
    private readonly ImagesTsvService _imagesTsvService;
    private readonly SpecimensTsvService _specimensTsvService;
    private readonly VariantsTsvService _variantsTsvService;
    private readonly TranscriptomicsTsvService _transcriptomicsTsvService;


    public VariantsTsvDownloadService(
        DonorsTsvService donorsTsvService,
        ImagesTsvService imagesTsvService,
        SpecimensTsvService specimensTsvService,
        VariantsTsvService variantsTsvService,
        TranscriptomicsTsvService transcriptomicsTsvService)
    {
        _donorsTsvService = donorsTsvService;
        _imagesTsvService = imagesTsvService;
        _specimensTsvService = specimensTsvService;
        _variantsTsvService = variantsTsvService;
        _transcriptomicsTsvService = transcriptomicsTsvService;
    }


    public async Task<byte[]> Download(long id, VariantType type, DataTypesCriteria criteria)
    {
        var ids = new[] { id };

        return await Download(ids, type, criteria);
    }

    public async Task<byte[]> Download(IEnumerable<long> ids, VariantType type, DataTypesCriteria criteria)
    {
        if (type == VariantType.SSM)
            return await Download<SSM.Variant>(ids, type, criteria);
        else if (type == VariantType.CNV)
            return await Download<CNV.Variant>(ids, type, criteria);
        else if (type == VariantType.SV)
            return await Download<SV.Variant>(ids, type, criteria);
        else
            throw new ArgumentException($"Unknown variant type: {type}");
    }


    private async Task<byte[]> Download<TV>(IEnumerable<long> ids, VariantType type, DataTypesCriteria dataTypes)
        where TV : Variant
    {
        var archiveBytes = Array.Empty<byte>();
        using (var archiveStream = new MemoryStream())
        using (var archive = new ZipArchive(archiveStream, ZipArchiveMode.Create, true))
        {
            if (dataTypes.Donors == true)
                await CreateArchiveEntry(archive, TsvFileNames.Donors, _donorsTsvService.GetDataForVariants<TV>(ids));
            
            if (dataTypes.Clinical == true)
                await CreateArchiveEntry(archive, TsvFileNames.Clinical, _donorsTsvService.GetClinicalDataForVariants<TV>(ids));

            if (dataTypes.Treatments == true)
                await CreateArchiveEntry(archive, TsvFileNames.Treatments, _donorsTsvService.GetTreatmentsDataForVariants<TV>(ids));

            if (dataTypes.Mris == true)
                await CreateArchiveEntry(archive, TsvFileNames.Mris, _imagesTsvService.GetDataForVariants<TV>(ids, ImageType.MRI));

            if (dataTypes.Specimens == true)
            {
                await CreateArchiveEntry(archive, TsvFileNames.Materials, _specimensTsvService.GetDataForVariants<TV>(ids, SpecimenType.Material));
                await CreateArchiveEntry(archive, TsvFileNames.Lines, _specimensTsvService.GetDataForVariants<TV>(ids, SpecimenType.Line));
                await CreateArchiveEntry(archive, TsvFileNames.Organoids, _specimensTsvService.GetDataForVariants<TV>(ids, SpecimenType.Organoid));
                await CreateArchiveEntry(archive, TsvFileNames.Xenografts, _specimensTsvService.GetDataForVariants<TV>(ids, SpecimenType.Xenograft));
            }

            if (dataTypes.Interventions == true)
            {
                await CreateArchiveEntry(archive, TsvFileNames.LinesInterventions, _specimensTsvService.GetInterventionsDataForVariants<TV>(ids, SpecimenType.Line));
                await CreateArchiveEntry(archive, TsvFileNames.OrganoidsInterventions, _specimensTsvService.GetInterventionsDataForVariants<TV>(ids, SpecimenType.Organoid));
                await CreateArchiveEntry(archive, TsvFileNames.XenograftsInterventions, _specimensTsvService.GetInterventionsDataForVariants<TV>(ids, SpecimenType.Xenograft));
            }

            if (dataTypes.Drugs == true)
            {
                await CreateArchiveEntry(archive, TsvFileNames.LinesDrugs, _specimensTsvService.GetDrugsScreeningsDataForVariants<TV>(ids, SpecimenType.Line));
                await CreateArchiveEntry(archive, TsvFileNames.OrganoidsDrugs, _specimensTsvService.GetDrugsScreeningsDataForVariants<TV>(ids, SpecimenType.Organoid));
                await CreateArchiveEntry(archive, TsvFileNames.XenograftsDrugs, _specimensTsvService.GetDrugsScreeningsDataForVariants<TV>(ids, SpecimenType.Xenograft));
            }

            if (dataTypes.Ssms == true && type == VariantType.SSM)
            {
                // TODO: Find intersecting variants
                if (dataTypes.SsmsTranscriptsFull == true)
                    await CreateArchiveEntry(archive, TsvFileNames.Ssms, _variantsTsvService.GetFullData(ids, VariantType.SSM));
                else
                    await CreateArchiveEntry(archive, TsvFileNames.Ssms, _variantsTsvService.GetData(ids, VariantType.SSM, dataTypes.SsmsTranscriptsSlim ?? false));
            }

            if (dataTypes.Cnvs == true && type == VariantType.CNV)
            {
                // TODO: Find intersecting variants
                if (dataTypes.CnvsTranscriptsFull == true)
                    await CreateArchiveEntry(archive, TsvFileNames.Cnvs, _variantsTsvService.GetFullData(ids, VariantType.CNV));
                else
                    await CreateArchiveEntry(archive, TsvFileNames.Cnvs, _variantsTsvService.GetData(ids, VariantType.CNV, dataTypes.CnvsTranscriptsSlim ?? false));
            }

            if (dataTypes.Svs == true && type == VariantType.SV)
            {
                // TODO: Find intersecting variants
                if (dataTypes.SvsTranscriptsFull == true)
                    await CreateArchiveEntry(archive, TsvFileNames.Svs, _variantsTsvService.GetFullData(ids, VariantType.SV));
                else
                    await CreateArchiveEntry(archive, TsvFileNames.Svs, _variantsTsvService.GetData(ids, VariantType.SV, dataTypes.SvsTranscriptsSlim ?? false));
            }

            if (dataTypes.GeneExp == true)
                await CreateArchiveEntry(archive, TsvFileNames.GeneExp, _transcriptomicsTsvService.GetDataForVariants<TV>(ids));
                
            archive.Dispose();
            archiveStream.Close();
            archiveBytes = archiveStream.ToArray();
        }

        return archiveBytes;
    }
}
