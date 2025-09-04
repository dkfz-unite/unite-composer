using System.IO.Compression;
using Microsoft.EntityFrameworkCore;
using Unite.Composer.Download.Services.Tsv.Mapping;
using Unite.Composer.Download.Tsv.Constants;
using Unite.Composer.Download.Tsv.Models;
using Unite.Data.Context;
using Unite.Essentials.Extensions;
using Unite.Essentials.Tsv;

using SM = Unite.Data.Entities.Omics.Analysis.Dna.Sm;
using CNV = Unite.Data.Entities.Omics.Analysis.Dna.Cnv;
using SV = Unite.Data.Entities.Omics.Analysis.Dna.Sv;

namespace Unite.Composer.Download.Services.Tsv;

public class DonorsDownloadService : DownloadService
{
    public DonorsDownloadService(IDbContextFactory<DomainDbContext> dbContextFactory) : base(dbContextFactory) { }

    // public override async Task Download(IEnumerable<int> ids, DownloadCriteria criteria, ZipArchive archive)
    public override async Task Download(IEnumerable<int> ids, DataTypesCriteria criteria, ZipArchive archive)
    {
        if (criteria.Donors == true)
        {
            var donors = await _donorDataRepository.GetDonors(ids);
            WriteData(archive, TsvFileNames.Donor, donors, DonorMapper.GetDonorMap());
        }

        if (criteria.Treatments == true)
        {
            var treatments = await _donorDataRepository.GetTreatments(ids);
            WriteData(archive, TsvFileNames.Treatment, treatments, DonorMapper.GetTreatmentMap());
        }

        if (criteria.Mrs == true)
        {
            var images = await _imageDataRepository.GetImagesForDonors(ids);
            var groups = images.GroupBy(entity => entity.TypeId);

            foreach (var group in groups)
            {
                var type = group.Key.ToDefinitionString().ToLower();
                WriteData(archive, string.Format(TsvFileNames.Image, type), group.ToArray(), ImageMapper.GetImageMap(group.Key));
            }
        }

        if (criteria.Specimens == true)
        {
            var specimens = await _specimenDataRepository.GetSpecimensForDonors(ids);
            var groups = specimens.GroupBy(entity => entity.TypeId);

            foreach (var group in groups)
            {
                var type = group.Key.ToDefinitionString().ToLower();
                WriteData(archive, string.Format(TsvFileNames.Specimen, type), group.ToArray(), SpecimenMapper.GetSpecimenMap(group.Key));
            }
        }

        if (criteria.Interventions == true)
        {
            var interventions = await _specimenDataRepository.GetInterventionsForDonors(ids);
            var groups = interventions.GroupBy(entity => entity.Specimen.TypeId);

            foreach (var group in groups)
            {
                var type = group.Key.ToDefinitionString().ToLower();
                WriteData(archive, string.Format(TsvFileNames.Intervention, type), group.ToArray(), SpecimenMapper.GetInterventionMap());
            }
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
                    WriteData(archive, entryName, group.ToArray(), SpecimenAnalysisMapper.GetDrugScreeningMap(), comments: SpecimenAnalysisMapper.MapSample(sample));
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
                    WriteData(archive, entryName, group.ToArray(), DnaAnalysisMapper.GetVariantMap<SM.VariantEntry, SM.Variant>(criteria.SmsTranscriptsSlim ?? false), comments: OmicsAnalysisMapper.MapSample(sample));
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
                    WriteData(archive, entryName, group.ToArray(), DnaAnalysisMapper.GetVariantMap<CNV.VariantEntry, CNV.Variant>(criteria.CnvsTranscriptsSlim ?? false), comments: OmicsAnalysisMapper.MapSample(sample));
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
                    WriteData(archive, entryName, group.ToArray(), DnaAnalysisMapper.GetVariantMap<SV.VariantEntry, SV.Variant>(criteria.SvsTranscriptsSlim ?? false), comments: OmicsAnalysisMapper.MapSample(sample));
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
                    WriteData(archive, entryName, group.ToArray(), RnaAnalysisMapper.GetExpressionMap(), comments: OmicsAnalysisMapper.MapSample(sample));
                }
            }
        }
    }

    private static void WriteData<T>(ZipArchive archive, string name, T[] data, ClassMap<T> map, string[] comments = null) where T : class
    {
        if (data == null || data.Length == 0)
            return;

        using var writer = CreateEntryWriter(archive, name);
        TsvWriter.Write(writer, data, map: map, comments: comments);
    }
}
