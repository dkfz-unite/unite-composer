namespace Unite.Composer.Data.Datasets.Models;

public record DatasetModel
{
    private string _id;
    private string _userId;
    private string _domain;
    private string _name;
    private string _description;
    private string _date;
    private string _criteria;

    public string Id { get => _id?.Trim(); set => _id = value; }

    public string UserId { get => _userId?.Trim(); set => _userId = value; }

    public string Domain { get => _domain?.Trim(); set => _domain = value; }

    public string Name { get => _name?.Trim(); set => _name = value; }

    public string Description { get => _description?.Trim(); set => _description = value; }

    public string Date { get => _date; set => _date = value; }

    public string Criteria { get => _criteria?.Trim(); set => _criteria = value; }
}
