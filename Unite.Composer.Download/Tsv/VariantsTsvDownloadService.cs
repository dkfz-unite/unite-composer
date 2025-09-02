using System.IO.Compression;
using Unite.Composer.Download.Tsv.Constants;
using Unite.Composer.Download.Tsv.Mapping;
using Unite.Data.Entities.Omics.Analysis.Dna;
using Unite.Data.Entities.Omics.Analysis.Dna.Enums;
using Unite.Data.Entities.Images.Enums;
using Unite.Data.Entities.Specimens.Enums;

using SM = Unite.Data.Entities.Omics.Analysis.Dna.Sm;
using CNV = Unite.Data.Entities.Omics.Analysis.Dna.Cnv;
using SV = Unite.Data.Entities.Omics.Analysis.Dna.Sv;
using Unite.Composer.Download.Tsv.Models;

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


    public async Task<byte[]> Download(int id, VariantType type, DataTypesCriteria criteria)
    {
        var ids = new[] { id };

        return await Download(ids, type, criteria);
    }

    public async Task<byte[]> Download(IEnumerable<int> ids, VariantType type, DataTypesCriteria criteria)
    {
        if (type == VariantType.SM)
            return await Download<SM.Variant>(ids, type, criteria);
        else if (type == VariantType.CNV)
            return await Download<CNV.Variant>(ids, type, criteria);
        else if (type == VariantType.SV)
            return await Download<SV.Variant>(ids, type, criteria);
        else
            throw new ArgumentException($"Unknown variant type: {type}");
    }


    private async Task<byte[]> Download<TV>(IEnumerable<int> ids, VariantType type, DataTypesCriteria dataTypes)
        where TV : Variant
    {
        var archiveBytes = Array.Empty<byte>();
        using (var archiveStream = new MemoryStream())
        using (var archive = new ZipArchive(archiveStream, ZipArchiveMode.Create, true))
        {
            if (dataTypes.Donors == true)
                await CreateArchiveEntry(archive, TsvFileNames.Donor, _donorsTsvService.GetDataForVariants<TV>(ids));
            
            // if (dataTypes.Clinical == true)
            //     await CreateArchiveEntry(archive, TsvFileNames.Clinical, _donorsTsvService.GetClinicalDataForVariants<TV>(ids));

            if (dataTypes.Treatments == true)
                await CreateArchiveEntry(archive, TsvFileNames.Treatment, _donorsTsvService.GetTreatmentsDataForVariants<TV>(ids));

            if (dataTypes.Mrs == true)
                await CreateArchiveEntry(archive, TsvFileNames.Mr, _imagesTsvService.GetDataForVariants<TV>(ids, ImageType.MR));

            if (dataTypes.Specimens == true)
            {
                await CreateArchiveEntry(archive, TsvFileNames.Material, _specimensTsvService.GetDataForVariants<TV>(ids, SpecimenType.Material));
                await CreateArchiveEntry(archive, TsvFileNames.Line, _specimensTsvService.GetDataForVariants<TV>(ids, SpecimenType.Line));
                await CreateArchiveEntry(archive, TsvFileNames.Organoid, _specimensTsvService.GetDataForVariants<TV>(ids, SpecimenType.Organoid));
                await CreateArchiveEntry(archive, TsvFileNames.Xenograft, _specimensTsvService.GetDataForVariants<TV>(ids, SpecimenType.Xenograft));
            }

            if (dataTypes.Interventions == true)
            {
                await CreateArchiveEntry(archive, TsvFileNames.LineIntervention, _specimensTsvService.GetInterventionsDataForVariants<TV>(ids, SpecimenType.Line));
                await CreateArchiveEntry(archive, TsvFileNames.OrganoidIntervention, _specimensTsvService.GetInterventionsDataForVariants<TV>(ids, SpecimenType.Organoid));
                await CreateArchiveEntry(archive, TsvFileNames.XenograftIntervention, _specimensTsvService.GetInterventionsDataForVariants<TV>(ids, SpecimenType.Xenograft));
            }

            if (dataTypes.Drugs == true)
            {
                await CreateArchiveEntry(archive, TsvFileNames.LineDrug, _specimensTsvService.GetDrugsScreeningsDataForVariants<TV>(ids, SpecimenType.Line));
                await CreateArchiveEntry(archive, TsvFileNames.OrganoidDrug, _specimensTsvService.GetDrugsScreeningsDataForVariants<TV>(ids, SpecimenType.Organoid));
                await CreateArchiveEntry(archive, TsvFileNames.XenograftDrug, _specimensTsvService.GetDrugsScreeningsDataForVariants<TV>(ids, SpecimenType.Xenograft));
            }

            if (dataTypes.Sms == true && type == VariantType.SM)
            {
                // TODO: Find intersecting variants
                if (dataTypes.SmsTranscriptsFull == true)
                    await CreateArchiveEntry(archive, TsvFileNames.Sm, _variantsTsvService.GetFullData(ids, VariantType.SM));
                else
                    await CreateArchiveEntry(archive, TsvFileNames.Sm, _variantsTsvService.GetData(ids, VariantType.SM, dataTypes.SmsTranscriptsSlim ?? false));
            }

            if (dataTypes.Cnvs == true && type == VariantType.CNV)
            {
                // TODO: Find intersecting variants
                if (dataTypes.CnvsTranscriptsFull == true)
                    await CreateArchiveEntry(archive, TsvFileNames.Cnv, _variantsTsvService.GetFullData(ids, VariantType.CNV));
                else
                    await CreateArchiveEntry(archive, TsvFileNames.Cnv, _variantsTsvService.GetData(ids, VariantType.CNV, dataTypes.CnvsTranscriptsSlim ?? false));
            }

            if (dataTypes.Svs == true && type == VariantType.SV)
            {
                // TODO: Find intersecting variants
                if (dataTypes.SvsTranscriptsFull == true)
                    await CreateArchiveEntry(archive, TsvFileNames.Sv, _variantsTsvService.GetFullData(ids, VariantType.SV));
                else
                    await CreateArchiveEntry(archive, TsvFileNames.Sv, _variantsTsvService.GetData(ids, VariantType.SV, dataTypes.SvsTranscriptsSlim ?? false));
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
