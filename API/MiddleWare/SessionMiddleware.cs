using System.Collections;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Application.Interfaces;
using Microsoft.AspNetCore.Http;
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
            var unitOfWork = context.RequestServices.GetRequiredService<IUnitOfWork>();

            var sessionIdHex = context.Request.Headers["X-Session-Id"].FirstOrDefault();
            if (string.IsNullOrEmpty(sessionIdHex) || sessionIdHex.Length != 64 || !sessionIdHex.All("0123456789abcdefABCDEF".Contains))
            {
                await RespondUnauthorizedAsync(context, "Invalid or missing session ID.");
                return;
            }

            var sessionIdBytes = HexUtils.StringToByteArray(sessionIdHex);

            var session = await unitOfWork.Repository<Domain.Entities.Session>()
                .GetAsync(s => s.SessionId == sessionIdBytes);

            if (session == null)
            {
                await RespondUnauthorizedAsync(context, "Invalid session.");
                return;
            }

            var nowUnix = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            if (session.ExpiryDate < nowUnix)
            {
                await RespondUnauthorizedAsync(context, "Session expired.");
                return;
            }

            var serializedData = Encoding.UTF8.GetString(session.SessionData);
            var sessionData = _serializer.Deserialize(serializedData) as Hashtable;

            if (sessionData == null || !sessionData.ContainsKey("userId"))
            {
                await RespondUnauthorizedAsync(context, "Session data is corrupted or missing userId.");
                return;
            }

            int userId;
            try
            {
                userId = Convert.ToInt32(sessionData["userId"]);
            }
            catch
            {
                await RespondUnauthorizedAsync(context, "Session data userId type mismatch.");
                return;
            }

            var user = await unitOfWork.Repository<Domain.Entities.User>()
                .GetAsync(
                    u => u.UserId == userId,
                    include: q => q.Include(u => u.UserGroup)
                );

            if (user == null)
            {
                await RespondUnauthorizedAsync(context, "User not found.");
                return;
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.UserGroup?.Title ?? string.Empty)
            };

            var identity = new ClaimsIdentity(claims, "XenForoSession");
            context.User = new ClaimsPrincipal(identity);

            await _next(context);
        }

        private async Task RespondUnauthorizedAsync(HttpContext context, string message)
        {
            context.Response.StatusCode = 401;
            context.Response.ContentType = "application/json";
            var response = JsonSerializer.Serialize(new { error = message });
            await context.Response.WriteAsync(response);
        }
    }
}
