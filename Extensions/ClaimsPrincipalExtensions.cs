using System.Security.Claims;

namespace Project.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static Guid GetUserId(this ClaimsPrincipal user)
    {
        var userIdClaim = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
        {
            throw new Exception("noUserExaption");
        }
        return Guid.Parse(userIdClaim.Value);
    }
}
