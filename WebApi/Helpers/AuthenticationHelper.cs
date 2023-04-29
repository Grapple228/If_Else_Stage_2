using System.Security.Claims;
using Database.Enums;
using static System.Enum;

namespace WebApi.Helpers;

internal static class AuthenticationHelper
{
    public static bool IsAuthenticated(this ClaimsPrincipal user)
    {
        return user.Identity?.IsAuthenticated == true;
    }

    public static Role? GetRole(this ClaimsPrincipal user)
    {
        var claim = user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role);
        if (claim == null) return null;
        var role = Parse<Role>(claim.Value);
        return role;
    }
    
    public static bool IsAdmin(this Role? role) =>
        role == Role.Admin;
    
    public static bool IsUser(this Role? role) =>
        role == Role.User;
    
    public static bool IsChipper(this Role? role) =>
        role == Role.Chipper;
}