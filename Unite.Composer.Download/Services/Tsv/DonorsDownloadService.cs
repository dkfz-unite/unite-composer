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
            await WriteData(archive, TsvFileNames.Donor, _donorDataRepository.GetDonors(ids), DonorMapper.GetDonorMap());

        if (criteria.Treatments == true)
            await WriteData(archive, TsvFileNames.Treatment, _donorDataRepository.GetTreatments(ids), DonorMapper.GetTreatmentMap());

        if (criteria.Mrs == true)
            await WriteData(archive, TsvFileNames.Mr, _imageDataRepository.GetImagesForDonors(ids, ImageType.MR), ImageMapper.GetMrMap());

        // if (criteria.Ct == true)
        //     await WriteData(archive, TsvFileNames.Ct, _imagesDataRepository.GetImagesForDonors(ids, ImageType.CT), ImageMapper.GetMrMap());

        if (criteria.Specimens == true)
        {
            await WriteData(archive, TsvFileNames.Material, _specimenDataRepository.GetSpecimensForDonors(ids, SpecimenType.Material), SpecimenMapper.GetMaterialMap());
            await WriteData(archive, TsvFileNames.Line, _specimenDataRepository.GetSpecimensForDonors(ids, SpecimenType.Line), SpecimenMapper.GetLineMap());
            await WriteData(archive, TsvFileNames.Organoid, _specimenDataRepository.GetSpecimensForDonors(ids, SpecimenType.Organoid), SpecimenMapper.GetOrganoidMap());
            await WriteData(archive, TsvFileNames.Xenograft, _specimenDataRepository.GetSpecimensForDonors(ids, SpecimenType.Xenograft), SpecimenMapper.GetXenograftMap());
        }

        if (criteria.Interventions == true)
        {
            await WriteData(archive, TsvFileNames.LineIntervention, _specimenDataRepository.GetInterventionsForDonors(ids, SpecimenType.Line), SpecimenMapper.GetInterventionMap());
            await WriteData(archive, TsvFileNames.OrganoidIntervention, _specimenDataRepository.GetInterventionsForDonors(ids, SpecimenType.Organoid), SpecimenMapper.GetInterventionMap());
            await WriteData(archive, TsvFileNames.XenograftIntervention, _specimenDataRepository.GetInterventionsForDonors(ids, SpecimenType.Xenograft), SpecimenMapper.GetInterventionMap());
        }

        if (criteria.Drugs == true)
        {
            foreach (var chunk in ids.Chunk(100))
            {
                var drugs = await _specimenAnalysisDataRepository.GetDrugsForDonors(chunk);
                var groups = drugs.GroupBy(entry => entry.SampleId);
                var samples = (await _specimenAnalysisDataRepository.GetSamples(groups.Select(group => group.Key))).ToDictionary(sample => sample.Id);

                foreach (var group in groups)
                {
                    var sample = samples[group.Key];
                    var entryName = string.Format(TsvFileNames.DrugScreening, sample.Specimen.Donor.ReferenceId, sample.Specimen.ReferenceId, sample.Specimen.TypeId.ToDefinitionString());
                    WriteData(archive, entryName, group.ToArray(), SpecimenAnalysisMapper.MapSample(sample), SpecimenAnalysisMapper.GetDrugScreeningMap());
                }
            }
        }

        if (criteria.Sms == true)
        {
            foreach (var chunk in ids.Chunk(50))
            {
                var variants = await _dnaAnalysisDataRepository.GetVariantsForDonors<SM.VariantEntry, SM.Variant>(chunk);
                var groups = variants.GroupBy(entry => entry.SampleId);
                var samples = (await _dnaAnalysisDataRepository.GetSamples(groups.Select(group => group.Key))).ToDictionary(sample => sample.Id);

                foreach (var group in groups)
                {
                    var sample = samples[group.Key];
                    var entryName = string.Format(TsvFileNames.Sm, sample.Specimen.Donor.ReferenceId, sample.Specimen.ReferenceId, sample.Specimen.TypeId.ToDefinitionString());
                    WriteData(archive, entryName, group.ToArray(), OmicsAnalysisMapper.MapSample(sample), DnaAnalysisMapper.GetVariantMap<SM.VariantEntry, SM.Variant>(criteria.SmsTranscriptsSlim ?? false));
                }
            }
        }

        if (criteria.Cnvs == true)
        {
            foreach (var chunk in ids.Chunk(25))
            {
                var variants = await _dnaAnalysisDataRepository.GetVariantsForDonors<CNV.VariantEntry, CNV.Variant>(chunk);
                var groups = variants.GroupBy(entry => entry.SampleId);
                var samples = (await _dnaAnalysisDataRepository.GetSamples(groups.Select(group => group.Key))).ToDictionary(sample => sample.Id);

                foreach (var group in groups)
                {
                    var sample = samples[group.Key];
                    var entryName = string.Format(TsvFileNames.Cnv, sample.Specimen.Donor.ReferenceId, sample.Specimen.ReferenceId, sample.Specimen.TypeId.ToDefinitionString());
                    WriteData(archive, entryName, group.ToArray(), OmicsAnalysisMapper.MapSample(sample), DnaAnalysisMapper.GetVariantMap<CNV.VariantEntry, CNV.Variant>(criteria.CnvsTranscriptsSlim ?? false));
                }
            }
        }

        if (criteria.Svs == true)
        {
            foreach (var chunk in ids.Chunk(25))
            {
                var variants = await _dnaAnalysisDataRepository.GetVariantsForDonors<SV.VariantEntry, SV.Variant>(chunk);
                var groups = variants.GroupBy(entry => entry.SampleId);
                var samples = (await _dnaAnalysisDataRepository.GetSamples(groups.Select(group => group.Key))).ToDictionary(sample => sample.Id);

                foreach (var group in groups)
                {
                    var sample = samples[group.Key];
                    var entryName = string.Format(TsvFileNames.Sv, sample.Specimen.Donor.ReferenceId, sample.Specimen.ReferenceId, sample.Specimen.TypeId.ToDefinitionString());
                    WriteData(archive, entryName, group.ToArray(), OmicsAnalysisMapper.MapSample(sample), DnaAnalysisMapper.GetVariantMap<SV.VariantEntry, SV.Variant>(criteria.SvsTranscriptsSlim ?? false));
                }
            }
        }

        if (criteria.GeneExp == true)
        {
            foreach (var chunk in ids.Chunk(25))
            {
                var expressions = await _rnaAnalysisDataRepository.GetExpressionsForDonors(chunk);
                var groups = expressions.GroupBy(entry => entry.SampleId);
                var samples = (await _rnaAnalysisDataRepository.GetSamples(groups.Select(group => group.Key))).ToDictionary(sample => sample.Id);

                foreach (var group in groups)
                {
                    var sample = samples[group.Key];
                    var entryName = string.Format(TsvFileNames.GeneExp, sample.Specimen.Donor.ReferenceId, sample.Specimen.ReferenceId, sample.Specimen.TypeId.ToDefinitionString());
                    WriteData(archive, entryName, group.ToArray(), OmicsAnalysisMapper.MapSample(sample), RnaAnalysisMapper.GetExpressionMap());
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
