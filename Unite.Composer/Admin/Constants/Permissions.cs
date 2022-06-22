using Unite.Identity.Entities.Enums;

namespace Unite.Composer.Admin.Constants;

public static class Permissions
{
    public static readonly Permission[] DefaultPermissions =
    {
            Permission.DataRead
        };

    public static readonly Permission[] RootPermissions = Enum.GetValues<Permission>();
}
