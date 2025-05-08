using System.Collections;
using System.Security.Claims;
using System.Text;
using Application.Interfaces;
using MySqlConnector;
using Shared.Utils;

namespace API.MiddleWare
{
    public class SessionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IMySqlDatabaseConnection _dbConnection;
        private readonly PHPSerializer _serializer = new PHPSerializer();

        public SessionMiddleware(RequestDelegate next, IMySqlDatabaseConnection dbConnection)
        {
            _next = next;
            _dbConnection = dbConnection;
        }

        public async Task InvokeAsync(HttpContext context)
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

            var sessionIdHex = context.Request.Headers["X-Session-Id"].FirstOrDefault();
            if (string.IsNullOrEmpty(sessionIdHex) || sessionIdHex.Length != 64 || !sessionIdHex.All("0123456789abcdefABCDEF".Contains))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Invalid or missing session ID.");
                return;
            }

            var sessionIdBytes = Enumerable.Range(0, sessionIdHex.Length / 2)
                .Select(x => Convert.ToByte(sessionIdHex.Substring(x * 2, 2), 16))
                .ToArray();

            await using var connection = await _dbConnection.CreateOpenConnectionAsync();
            var query = @"
                SELECT 
                    s.session_data, 
                    s.expiry_date, 
                    u.username, 
                    ug.title AS user_group_name
                FROM xf_session s
                JOIN xf_user u ON (CAST(JSON_EXTRACT(CONVERT(s.session_data USING utf8), '$.userId') AS UNSIGNED) = u.user_id)
                JOIN xf_user_group ug ON u.user_group_id = ug.user_group_id
                WHERE s.session_id = @sessionId
                LIMIT 1;
            ";

            await using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.Add("@sessionId", MySqlDbType.VarBinary).Value = sessionIdBytes;

            await using var reader = await cmd.ExecuteReaderAsync();
            if (!await reader.ReadAsync())
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Invalid session.");
                return;
            }

            var expiryUnix = reader.GetInt64("expiry_date");
            var nowUnix = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            if (expiryUnix < nowUnix)
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Session expired.");
                return;
            }

            var sessionBlob = (byte[])reader["session_data"];
            var serializedData = Encoding.UTF8.GetString(sessionBlob);
            var sessionData = _serializer.Deserialize(serializedData) as Hashtable;
            var userId = (int)(sessionData?["userId"] ?? 0);
            var username = sessionData?["username"]?.ToString() ?? "";
            var groupName = reader.GetString("user_group_name");

            if (userId == 0)
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Invalid session data.");
                return;
            }
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, groupName) 
            };

            var identity = new ClaimsIdentity(claims, "XenForoSession");
            context.User = new ClaimsPrincipal(identity);
            await _next(context);
        }
    }
}
