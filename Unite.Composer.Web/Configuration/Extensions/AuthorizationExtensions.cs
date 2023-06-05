using Microsoft.AspNetCore.Authorization;
// using Unite.Identity.Entities.Constants;

namespace Unite.Composer.Web.Configuration.Extensions;

public static class AuthorizationExtensions
{
    public static void AddAuthorizationOptions(this AuthorizationOptions options)
    {
        // options.AddPolicy(Policies.Data.Manager, policy => policy
        //     .RequireClaim(ClaimsHelper.PermissionClaimType, Permissions.Data.Write)
        //     .RequireClaim(ClaimsHelper.PermissionClaimType, Permissions.Data.Edit)
        //     .RequireClaim(ClaimsHelper.PermissionClaimType, Permissions.Data.Delete)
        // );
    }
}

// public static class Policies
// {
//     public static class Data
//     {
//         public const string Manager = "Data.Manager";
//     }
// }
