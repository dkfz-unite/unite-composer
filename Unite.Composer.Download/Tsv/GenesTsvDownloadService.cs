using System.IO.Compression;
using Unite.Composer.Download.Models;
using Unite.Composer.Download.Tsv.Constants;
using Unite.Composer.Download.Tsv.Mapping;
using Unite.Data.Entities.Genome.Variants.Enums;
using Unite.Data.Entities.Images.Enums;
using Unite.Data.Entities.Specimens.Enums;

namespace Unite.Composer.Download.Tsv;

public class GenesTsvDownloadService : TsvDownloadService
{
    private readonly DonorsTsvService _donorsTsvService;
    private readonly ImagesTsvService _imagesTsvService;
    private readonly SpecimensTsvService _specimensTsvService;
    private readonly VariantsTsvService _variantsTsvService;
    private readonly TranscriptomicsTsvService _transcriptomicsTsvService;


    public GenesTsvDownloadService(
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


    public async Task<byte[]> Download(int id, DataTypesCriteria criteria)
    {
        var ids = new[] { id };

        return await Download(ids, criteria);
    }

    public async Task<byte[]> Download(IEnumerable<int> ids, DataTypesCriteria criteria)
    {
        var archiveBytes = Array.Empty<byte>();
        using (var archiveStream = new MemoryStream())
        using (var archive = new ZipArchive(archiveStream, ZipArchiveMode.Create, true))
        {
            if (criteria.Donors == true)
                await CreateArchiveEntry(archive, TsvFileNames.Donors, _donorsTsvService.GetDataForGenes(ids));
            
            if (criteria.Clinical == true)
                 await CreateArchiveEntry(archive, TsvFileNames.Clinical, _donorsTsvService.GetClinicalDataForGenes(ids));

            if (criteria.Treatments == true)
                await CreateArchiveEntry(archive, TsvFileNames.Treatments, _donorsTsvService.GetTreatmentsDataForGenes(ids));

            if (criteria.Mris == true)
                await CreateArchiveEntry(archive, TsvFileNames.Mris, _imagesTsvService.GetDataForGenes(ids, ImageType.MRI));

            if (criteria.Specimens == true)
            {
                await CreateArchiveEntry(archive, TsvFileNames.Materials, _specimensTsvService.GetDataForGenes(ids, SpecimenType.Material));
                await CreateArchiveEntry(archive, TsvFileNames.Lines, _specimensTsvService.GetDataForGenes(ids, SpecimenType.Line));
                await CreateArchiveEntry(archive, TsvFileNames.Organoids, _specimensTsvService.GetDataForGenes(ids, SpecimenType.Organoid));
                await CreateArchiveEntry(archive, TsvFileNames.Xenografts, _specimensTsvService.GetDataForGenes(ids, SpecimenType.Xenograft));
            }

            if (criteria.Interventions == true)
            {
                await CreateArchiveEntry(archive, TsvFileNames.LinesInterventions, _specimensTsvService.GetInterventionsDataForGenes(ids, SpecimenType.Line));
                await CreateArchiveEntry(archive, TsvFileNames.OrganoidsInterventions, _specimensTsvService.GetInterventionsDataForGenes(ids, SpecimenType.Organoid));
                await CreateArchiveEntry(archive, TsvFileNames.XenograftsInterventions, _specimensTsvService.GetInterventionsDataForGenes(ids, SpecimenType.Xenograft));
            }

            if (criteria.Drugs == true)
            {
                await CreateArchiveEntry(archive, TsvFileNames.LinesDrugs, _specimensTsvService.GetDrugsScreeningsDataForGenes(ids, SpecimenType.Line));
                await CreateArchiveEntry(archive, TsvFileNames.OrganoidsDrugs, _specimensTsvService.GetDrugsScreeningsDataForGenes(ids, SpecimenType.Organoid));
                await CreateArchiveEntry(archive, TsvFileNames.XenograftsDrugs, _specimensTsvService.GetDrugsScreeningsDataForGenes(ids, SpecimenType.Xenograft));
            }

            if (criteria.Ssms == true)
            {
                if (criteria.SsmsTranscriptsFull == true)
                    await CreateArchiveEntry(archive, TsvFileNames.Ssms, _variantsTsvService.GetFullDataForGenes(ids, VariantType.SSM));
                else
                    await CreateArchiveEntry(archive, TsvFileNames.Ssms, _variantsTsvService.GetDataForGenes(ids, VariantType.SSM, criteria.SsmsTranscriptsSlim ?? false));
            }

            if (criteria.Cnvs == true)
            {
                if (criteria.CnvsTranscriptsFull == true)
                    await CreateArchiveEntry(archive, TsvFileNames.Cnvs, _variantsTsvService.GetFullDataForGenes(ids, VariantType.CNV));
                else
                    await CreateArchiveEntry(archive, TsvFileNames.Cnvs, _variantsTsvService.GetDataForGenes(ids, VariantType.CNV, criteria.CnvsTranscriptsSlim ?? false));
            }

            if (criteria.Svs == true)
            {
                if (criteria.SvsTranscriptsFull == true)
                    await CreateArchiveEntry(archive, TsvFileNames.Svs, _variantsTsvService.GetFullDataForGenes(ids, VariantType.SV));
                else
                    await CreateArchiveEntry(archive, TsvFileNames.Svs, _variantsTsvService.GetDataForGenes(ids, VariantType.SV, criteria.SvsTranscriptsSlim ?? false));
            }

            if (criteria.GeneExp == true)
                await CreateArchiveEntry(archive, TsvFileNames.GeneExp, _transcriptomicsTsvService.GetData(ids));
                
            archive.Dispose();
            archiveStream.Close();
            archiveBytes = archiveStream.ToArray();
        }

        return archiveBytes;
    }
}
