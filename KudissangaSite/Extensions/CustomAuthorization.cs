using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace KudissangaSite.Extensions;

public class CustomAuthorization
{ 
    public static bool ValidarClaimsUsuario(HttpContext context, string claimName, string claimValue)
    {
        return context.User.Identity.IsAuthenticated &&
            context.User.Claims.Any(user => user.Type.Contains(claimName) && user.Value.Contains(claimValue));
    }
}

public class ClaimsAuthorizeAttribute : TypeFilterAttribute
{
    public ClaimsAuthorizeAttribute(string claimName, string claimValue) : base(typeof(RequisitoClaimFilter))
    {
        Arguments = new object[] { new Claim(claimName, claimValue) };
    }
}

public class RequisitoClaimFilter : IAuthorizationFilter
{
    public RequisitoClaimFilter(Claim claim)
    {
        _claim = claim;
    }

    private readonly Claim _claim;

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (!CustomAuthorization.ValidarClaimsUsuario(context.HttpContext, _claim.Type, _claim.Value))
        {
            context.Result = new ForbidResult();
        }
    }
}
