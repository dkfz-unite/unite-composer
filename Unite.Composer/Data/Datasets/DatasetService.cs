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

	public async Task DeleteUser(string userId)
	{
	 	var datasets = await _datasetsRepository.WhereAsync(item =>item.Document.UserId == userId);
		var userDatasets = datasets.Select(dataset => _datasetsRepository.DeleteAsync(dataset.Id));
		await Task.WhenAll(userDatasets);
	}
}