namespace Unite.Composer.Data.Datasets.Models;

public record SearchModel
{
    private string _userId;

    public string UserId { get => _userId?.Trim(); set => _userId = value; }
}
