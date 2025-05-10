using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Application.Common.Errors;
using System.Net;
using Application.Common.Responses;

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
            var correlationId = context.HttpContext.TraceIdentifier;

            if (!user.Identity?.IsAuthenticated ?? true)
            {
                var error = ApplicationError.CreateAuthenticationError(
                    ErrorDefinitions.Unauthorized,
                    correlationId
                );
                var problemDetails = new ProblemDetails
                {
                    Title = error.Title,
                    Detail = error.ErrorDescription,
                    Status = (int)HttpStatusCode.Unauthorized,
                    Type = error.ErrorCode, 
                    Extensions =
                    {
                        { "traceId", error.TraceId },
                        { "correlationId", error.CorrelationId }
                    }
                };
                context.Result = new JsonResult(ApiResponse<object>.CreateError(
                    error: problemDetails,
                    statusCode: System.Net.HttpStatusCode.Unauthorized
                ))
                {
                    StatusCode = StatusCodes.Status401Unauthorized
                };

                return;
            }
            var groupClaim = user.FindFirst(ClaimTypes.Role);
            if (groupClaim == null || !_allowedRoles.Contains(groupClaim.Value, StringComparer.OrdinalIgnoreCase))
            {
                var error = ApplicationError.CreateForbiddenError(
                    ErrorDefinitions.Forbidden,
                    correlationId,
                    groupClaim?.Value ?? "Unknown"
                );
                var problemDetails = new ProblemDetails
                {
                    Title = error.Title,
                    Detail = error.ErrorDescription,
                    Status = (int)HttpStatusCode.Unauthorized,
                    Type = error.ErrorCode,
                    Extensions =
                    {
                        { "traceId", error.TraceId },
                        { "correlationId", error.CorrelationId }
                    }
                };
                context.Result = new JsonResult(ApiResponse<object>.CreateError(
                    error: problemDetails,
                    statusCode: System.Net.HttpStatusCode.Forbidden
                ))
                {
                    StatusCode = StatusCodes.Status403Forbidden
                };
            }
        }
    }
}
