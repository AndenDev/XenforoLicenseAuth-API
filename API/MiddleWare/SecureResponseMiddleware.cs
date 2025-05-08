using Application.Interfaces;
using System.Text.Json;
using System.Text;

namespace API.MiddleWare
{
    public class SecureResponseMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ICryptoService _cryptoService;
        private readonly IEd25519SigningService _signingService;

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
            var expected = ApiRoutes.AuthRoutes.Login.StartsWith("/")
                ? ApiRoutes.AuthRoutes.Login
                : "/" + ApiRoutes.AuthRoutes.Login;

            if (path.StartsWithSegments(expected, StringComparison.OrdinalIgnoreCase))
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

            var jsonString = JsonSerializer.Serialize(encryptedPayload); // outputs: "base64string"

            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
            var signature = _signingService.Sign(timestamp + encryptedPayload);

            context.Response.Headers["X-Signature-Ed25519"] = signature;
            context.Response.Headers["X-Signature-Timestamp"] = timestamp;

            var jsonBytes = Encoding.UTF8.GetBytes(jsonString);

            context.Response.Body = originalBody;
            context.Response.ContentType = "application/json";

            // ✅ either write full buffer (auto content-length):
            await context.Response.Body.WriteAsync(jsonBytes);

            // OR explicitly set Content-Length (optional):
            // context.Response.ContentLength = jsonBytes.Length;
            // await context.Response.Body.WriteAsync(jsonBytes, 0, jsonBytes.Length);
        }
    }
}
