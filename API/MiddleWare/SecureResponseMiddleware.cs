using Application.Interfaces;
using System.Text.Json;
using System.Text;
using System.Text.RegularExpressions;

namespace API.MiddleWare
{
    public class SecureResponseMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ICryptoService _cryptoService;
        private readonly IEd25519SigningService _signingService;


        private readonly HashSet<PathString> _excludedPaths = new()
        {
            ApiRoutes.AuthRoutes.Login,
            ApiRoutes.AuthRoutes.Logout,
            ApiRoutes.AuthRoutes.ValidateSession,
            ApiRoutes.UserRoutes.Profile

        };

        public SecureResponseMiddleware(
            RequestDelegate next,
            ICryptoService cryptoService,
            IEd25519SigningService signingService)
        {
            _next = next;
            _cryptoService = cryptoService;
            _signingService = signingService;
        }

        public async Task Invoke(HttpContext context)
        {
            var path = context.Request.Path;

            if (IsExcludedPath(path))
            {
                await _next(context);
                return;
            }

            var originalBody = context.Response.Body;
            var memoryStream = new MemoryStream();
            context.Response.Body = memoryStream;

            await _next(context);

            memoryStream.Seek(0, SeekOrigin.Begin);
            var plainResponse = await new StreamReader(memoryStream).ReadToEndAsync();

            var encryptedPayload = _cryptoService.Encrypt(plainResponse);

            var jsonString = JsonSerializer.Serialize(encryptedPayload); 

            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
            var signature = _signingService.Sign(timestamp + encryptedPayload);

            context.Response.Headers["X-Signature-Ed25519"] = signature;
            context.Response.Headers["X-Signature-Timestamp"] = timestamp;

            var jsonBytes = Encoding.UTF8.GetBytes(jsonString);

            context.Response.Body = originalBody;
            context.Response.ContentType = "application/json";

            await context.Response.Body.WriteAsync(jsonBytes);
        }
        private static readonly string[] _excludedRoutePrefixes = new[]
         {
            "/api/auth",
            "/api/user"
        };
        private static readonly Regex _excludedPathRegex = new(
            $"^({string.Join("|", _excludedRoutePrefixes.Select(Regex.Escape))})(/.*)?$",
            RegexOptions.IgnoreCase | RegexOptions.Compiled
        );
        private bool IsExcludedPath(PathString path)
        {
            return _excludedPathRegex.IsMatch(path);
        }
    }
}
