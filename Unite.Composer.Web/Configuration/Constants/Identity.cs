namespace Unite.Composer.Web.Configuration.Constants;

public static class Roles
{
    public const string Admin = "Root";
}

public static class Permissions
{
    public static class Data
    {
        public const string Read = "Data.Read";
        public const string Write = "Data.Write";
    }
}

public static class Policies
{
    public static class Data
    {
        public const string Writer = "Data.Writer";
    }
}
