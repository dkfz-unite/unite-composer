namespace Unite.Composer.Clients.Identity;

public class UserResource
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string Provider { get; set; }
    public string[] Permissions { get; set; }
}