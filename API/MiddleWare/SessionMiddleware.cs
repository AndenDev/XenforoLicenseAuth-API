using System.Collections;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Utils;

namespace API.MiddleWare
{
    public class SessionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly PHPSerializer _serializer = new PHPSerializer();
        private readonly HashSet<PathString> _excludedPaths = new()
        {
            ApiRoutes.AuthRoutes.Login
        };

        public SessionMiddleware(RequestDelegate next)
        {
            _next = next;

        }

        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path;

            if (_excludedPaths.Any(p => path.StartsWithSegments(p, StringComparison.OrdinalIgnoreCase)))
            {
                await _next(context);
                return;
            }

            var sessionIdHex = context.Request.Headers["X-Session-Id"].FirstOrDefault();
            if (string.IsNullOrEmpty(sessionIdHex) || sessionIdHex.Length != 64 || !sessionIdHex.All("0123456789abcdefABCDEF".Contains))
            {
                await RespondUnauthorizedAsync(context, "Invalid or missing session ID.");
                return;
            }

            var sessionIdBytes = HexUtils.StringToByteArray(sessionIdHex);

            try
            {
                var unitOfWork = context.RequestServices.GetRequiredService<IUnitOfWork>();
                var session = await unitOfWork.Repository<Session>()
                    .GetAsync(s => s.SessionId == sessionIdBytes);

                if (session == null || session.ExpiryDate < DateTimeOffset.UtcNow.ToUnixTimeSeconds())
                {
                    await RespondUnauthorizedAsync(context, "Invalid or expired session.");
                    return;
                }

                var rawSessionData = Encoding.UTF8.GetString(session.SessionData);
                Hashtable sessionData;

                try
                {
                    sessionData = _serializer.Deserialize(rawSessionData) as Hashtable ?? throw new FormatException("Session deserialization returned null.");
                }
                catch
                {
                    await RespondUnauthorizedAsync(context, "Malformed session data.");
                    return;
                }

                if (!sessionData.ContainsKey("userId") || !sessionData.ContainsKey("userGroupName"))
                {
                    await RespondUnauthorizedAsync(context, "Session missing required fields.");
                    return;
                }

                var userId = sessionData["userId"];
                var userGroupName = sessionData["userGroupName"]?.ToString();

                if (string.IsNullOrWhiteSpace(userGroupName))
                {
                    await RespondUnauthorizedAsync(context, "Session missing group.");
                    return;
                }

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                    new Claim(ClaimTypes.Role, userGroupName)
                };

                var identity = new ClaimsIdentity(claims, "XenForoSession");
                context.User = new ClaimsPrincipal(identity);

                await _next(context);
            }
            catch
            {
                await RespondUnauthorizedAsync(context, "Session error.");
            }
        }


        private async Task RespondUnauthorizedAsync(HttpContext context, string message)
        {
            var correlationId = context.TraceIdentifier;
            var problemDetails = new ProblemDetails
            {
                Type = "https://httpstatuses.com/401",
                Title = "Unauthorized",
                Status = StatusCodes.Status401Unauthorized,
                Detail = message,
                Instance = context.Request.Path,
                Extensions = { ["correlationId"] = correlationId }
            };

            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Response.ContentType = "application/problem+json";

            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = false
            };

            var response = JsonSerializer.Serialize(problemDetails, jsonOptions);
            await context.Response.WriteAsync(response);
        }

    }
}
