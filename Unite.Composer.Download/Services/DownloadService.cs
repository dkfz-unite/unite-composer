using System.IO.Compression;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Unite.Composer.Download.Models;
using Unite.Composer.Download.Repositories;
using Unite.Data.Context;

namespace Unite.Composer.Download.Services;

public abstract class DownloadService
{
    protected readonly IDbContextFactory<DomainDbContext> _dbContextFactory;
    protected readonly DonorDataRepository _donorDataRepository;
    protected readonly ImageDataRepository _imageDataRepository;
    protected readonly ImageAnalysisDataRepository _imageAnalysisDataRepository;
    protected readonly SpecimenDataRepository _specimenDataRepository;
    protected readonly SpecimenAnalysisDataRepository _specimenAnalysisDataRepository;
    protected readonly DnaAnalysisDataRepository _dnaAnalysisDataRepository;
    protected readonly RnaAnalysisDataRepository _rnaAnalysisDataRepository;


    public DownloadService(IDbContextFactory<DomainDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
        _donorDataRepository = new DonorDataRepository(_dbContextFactory);
        _imageDataRepository = new ImageDataRepository(_dbContextFactory);
        _imageAnalysisDataRepository = new ImageAnalysisDataRepository(_dbContextFactory);
        _specimenDataRepository = new SpecimenDataRepository(_dbContextFactory);
        _specimenAnalysisDataRepository = new SpecimenAnalysisDataRepository(_dbContextFactory);
        _dnaAnalysisDataRepository = new DnaAnalysisDataRepository(_dbContextFactory);
        _rnaAnalysisDataRepository = new RnaAnalysisDataRepository(_dbContextFactory);
    }


    public abstract Task Download(IEnumerable<int> ids, DataTypesCriteria criteria, ZipArchive archive);


    protected static StreamWriter CreateEntryWriter(ZipArchive archive, string name)
    {
        var entry = archive.CreateEntry(name);
        var entryStream = entry.Open();
        return new StreamWriter(entryStream, Encoding.UTF8);
    }
}
