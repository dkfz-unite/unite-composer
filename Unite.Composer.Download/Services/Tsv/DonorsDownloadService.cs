using System.IO.Compression;
using Microsoft.EntityFrameworkCore;
using Unite.Composer.Download.Services.Tsv.Mapping;
using Unite.Composer.Download.Tsv.Constants;
using Unite.Data.Context;
using Unite.Data.Entities.Images.Enums;
using Unite.Data.Entities.Specimens.Enums;
using Unite.Essentials.Extensions;
using Unite.Essentials.Tsv;

using SM = Unite.Data.Entities.Omics.Analysis.Dna.Sm;
using CNV = Unite.Data.Entities.Omics.Analysis.Dna.Cnv;
using SV = Unite.Data.Entities.Omics.Analysis.Dna.Sv;
using Unite.Composer.Download.Tsv.Models;


namespace Unite.Composer.Download.Services.Tsv;

public class DonorsDownloadService : DownloadService
{
    public DonorsDownloadService(IDbContextFactory<DomainDbContext> dbContextFactory) : base(dbContextFactory) { }

    // public override async Task Download(IEnumerable<int> ids, DownloadCriteria criteria, ZipArchive archive)
    public override async Task Download(IEnumerable<int> ids, DataTypesCriteria criteria, ZipArchive archive)
    {
        if (criteria.Donors == true)
            await WriteData(archive, TsvFileNames.Donor, _donorsDataRepository.GetDonors(ids), DonorMapper.GetDonorMap());

        if (criteria.Treatments == true)
            await WriteData(archive, TsvFileNames.Treatment, _donorsDataRepository.GetTreatments(ids), DonorMapper.GetTreatmentMap());

        if (criteria.Mrs == true)
            await WriteData(archive, TsvFileNames.Mr, _imagesDataRepository.GetImagesForDonors(ids, ImageType.MR), ImageMapper.GetMrMap());

        // if (criteria.Ct == true)
        //     await WriteData(archive, TsvFileNames.Ct, _imagesDataRepository.GetImagesForDonors(ids, ImageType.CT), ImageMapper.GetMrMap());

        if (criteria.Specimens == true)
        {
            await WriteData(archive, TsvFileNames.Material, _specimensDataRepository.GetSpecimensForDonors(ids, SpecimenType.Material), SpecimenMapper.GetMaterialMap());
            await WriteData(archive, TsvFileNames.Line, _specimensDataRepository.GetSpecimensForDonors(ids, SpecimenType.Line), SpecimenMapper.GetLineMap());
            await WriteData(archive, TsvFileNames.Organoid, _specimensDataRepository.GetSpecimensForDonors(ids, SpecimenType.Organoid), SpecimenMapper.GetOrganoidMap());
            await WriteData(archive, TsvFileNames.Xenograft, _specimensDataRepository.GetSpecimensForDonors(ids, SpecimenType.Xenograft), SpecimenMapper.GetXenograftMap());
        }

        if (criteria.Interventions == true)
        {
            await WriteData(archive, TsvFileNames.LineIntervention, _specimensDataRepository.GetInterventionsForDonors(ids, SpecimenType.Line), SpecimenMapper.GetInterventionMap());
            await WriteData(archive, TsvFileNames.OrganoidIntervention, _specimensDataRepository.GetInterventionsForDonors(ids, SpecimenType.Organoid), SpecimenMapper.GetInterventionMap());
            await WriteData(archive, TsvFileNames.XenograftIntervention, _specimensDataRepository.GetInterventionsForDonors(ids, SpecimenType.Xenograft), SpecimenMapper.GetInterventionMap());
        }

        if (criteria.Drugs == true)
        {

        }

        if (criteria.Sms == true)
        {
            var size = criteria.SmsTranscriptsSlim == true ? 50 : 100;
            foreach (var chunk in ids.Chunk(size))
            {
                var variants = await _variantsDataRepository.GetVariantsForDonors<SM.VariantEntry, SM.Variant>(chunk);
                var groups = variants.GroupBy(entry => entry.SampleId);

                foreach (var group in groups)
                {
                    var sample = (await _samplesDataRepository.GetSamples([group.Key])).First();
                    var entryName = string.Format(TsvFileNames.Sm, sample.Specimen.Donor.ReferenceId, sample.Specimen.ReferenceId, sample.Specimen.TypeId.ToDefinitionString());
                    WriteData(archive, entryName, group.ToArray(), SampleMapper.MapSample(sample), VariantMapper.GetVariantMap<SM.VariantEntry, SM.Variant>(criteria.SmsTranscriptsSlim ?? false));
                }
            }
        }

        if (criteria.Cnvs == true)
        {
            var size = criteria.CnvsTranscriptsSlim == true ? 10 : 50;
            foreach (var chunk in ids.Chunk(size))
            {
                var variants = await _variantsDataRepository.GetVariantsForDonors<CNV.VariantEntry, CNV.Variant>(chunk);
                var groups = variants.GroupBy(entry => entry.SampleId);

                foreach (var group in groups)
                {
                    var sample = (await _samplesDataRepository.GetSamples([group.Key])).First();
                    var entryName = string.Format(TsvFileNames.Cnv, sample.Specimen.Donor.ReferenceId, sample.Specimen.ReferenceId, sample.Specimen.TypeId.ToDefinitionString());
                    WriteData(archive, entryName, group.ToArray(), SampleMapper.MapSample(sample), VariantMapper.GetVariantMap<CNV.VariantEntry, CNV.Variant>(criteria.CnvsTranscriptsSlim ?? false));
                }
            }
        }

        if (criteria.Svs == true)
        {
            var size = criteria.SvsTranscriptsSlim == true ? 10 : 50;
            foreach (var chunk in ids.Chunk(size))
            {
                var variants = await _variantsDataRepository.GetVariantsForDonors<SV.VariantEntry, SV.Variant>(chunk);
                var groups = variants.GroupBy(entry => entry.SampleId);

                foreach (var group in groups)
                {
                    var sample = (await _samplesDataRepository.GetSamples([group.Key])).First();
                    var entryName = string.Format(TsvFileNames.Sv, sample.Specimen.Donor.ReferenceId, sample.Specimen.ReferenceId, sample.Specimen.TypeId.ToDefinitionString());
                    WriteData(archive, entryName, group.ToArray(), SampleMapper.MapSample(sample), VariantMapper.GetVariantMap<SV.VariantEntry, SV.Variant>(criteria.SvsTranscriptsSlim ?? false));
                }
            }
        }

        if (criteria.GeneExp == true)
        {
            var size = 50;
            foreach (var chunk in ids.Chunk(size))
            {
                var expressions = await _geneExpressionsDataRepository.GetExpressionsForDonors(chunk);
                var groups = expressions.GroupBy(entry => entry.SampleId);

                foreach (var group in groups)
                {
                    var sample = (await _samplesDataRepository.GetSamples([group.Key])).First();
                    var entryName = string.Format(TsvFileNames.GeneExp, sample.Specimen.Donor.ReferenceId, sample.Specimen.ReferenceId, sample.Specimen.TypeId.ToDefinitionString());
                    WriteData(archive, entryName, group.ToArray(), SampleMapper.MapSample(sample), GeneExpressionMapper.GetGeneExpressionMap());
                }
            }
        }
    }


    private static async Task WriteData<T>(ZipArchive archive, string name, Task<T[]> task, ClassMap<T> map) where T : class
    {
        var data = await task;

        if (data == null || data.Length == 0)
            return;

        using var writer = CreateEntryWriter(archive, name);
        TsvWriter.Write(writer, data, map);
    }

    private static void WriteData<T>(ZipArchive archive, string name, T[] data, string[] comments, ClassMap<T> map) where T : class
    {
        if (data == null || data.Length == 0)
            return;

        using var writer = CreateEntryWriter(archive, name);
        TsvWriter.Write(writer, data, map: map, comments: comments);
    }
}
