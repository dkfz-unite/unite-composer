using Unite.Cache.Configuration.Options;
using Unite.Composer.Data.Datasets.Models;

namespace Unite.Composer.Data.Datasets;

public class DatasetService
{
    private readonly Repositories.DatasetsRepository _datasetsRepository;

    public DatasetService(IMongoOptions options)
	{
		_datasetsRepository = new Repositories.DatasetsRepository(options);
	}

	public async Task<string> Add(DatasetModel data)
	{
		return await _datasetsRepository.AddAsync(data);
	}

	public async Task Delete(string id)
	{
	 	await _datasetsRepository.DeleteAsync(id);
	}
}