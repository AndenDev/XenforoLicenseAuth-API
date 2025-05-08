using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Attributes
{
    public class XenForoAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        private readonly string[] _allowedRoles;

        public XenForoAuthorizeAttribute(params string[] allowedRoles)
        {
            _allowedRoles = allowedRoles;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;

            if (!user.Identity?.IsAuthenticated ?? true)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var groupClaim = user.FindFirst(ClaimTypes.Role);
            if (groupClaim == null || !_allowedRoles.Contains(groupClaim.Value, StringComparer.OrdinalIgnoreCase))
            {
                context.Result = new ForbidResult();
            }
        }
    }
}
