using System.Collections;
using System.Text;
using Application.Interfaces;
using Shared.Utils;
using MySqlConnector;
using Application.DTOs.Response;
using MediatR;
using System.Net;
using System.Security.Cryptography;
using Shared.Constant;
using Application.Common.Results;
using Application.Common.Errors;


namespace Infrastructure.Services
{
    public class XenForoAuthService : IXenforoAuthService
    {
        private readonly IMySqlDatabaseConnection _dbConnection;
        private readonly IMediator _mediator;

        public XenForoAuthService(IMySqlDatabaseConnection dbConnection, IMediator mediator)
        {
            _dbConnection = dbConnection;
            _mediator = mediator;
        }

        public async Task<Result<XenforoAuthResponseDTO>> AuthenticateUserAsync(string username, string password, string clientIp)
        {
            const string userQuery = @"
        SELECT 
            u.user_id, 
            u.username, 
            u.user_group_id, 
            ug.title AS user_group_name,
            a.data
        FROM xf_user u
        JOIN xf_user_authenticate a ON a.user_id = u.user_id
        JOIN xf_user_group ug ON u.user_group_id = ug.user_group_id
        WHERE u.username = @username
        LIMIT 1;
    ";

            await using var connection = await _dbConnection.CreateOpenConnectionAsync();

            (int userId, int userGroupId, string usernameDb, string userGroupName, string authDataStr)? userRecord = await GetUserRecordAsync(connection, username, userQuery);

            if (userRecord == null)
                return Result<XenforoAuthResponseDTO>.Fail(ApplicationError.UserNotFound);

            var (userId, userGroupId, usernameDb, userGroupName, authDataStr) = userRecord.Value;

            // Deserialize and verify password
            if (!VerifyPassword(authDataStr, password))
                return Result<XenforoAuthResponseDTO>.Fail(ApplicationError.InvalidPassword);

            // Check if user is in allowed group(s)
            var allowedGroups = new[] { XenForoUserGroups.Admin };

            if (!allowedGroups.Contains(userGroupName, StringComparer.OrdinalIgnoreCase))
                return Result<XenforoAuthResponseDTO>.Fail(ApplicationError.UnauthorizedGroup);

            // Create session
            var sessionIdBytes = GenerateSessionId();
            var sessionIdHex = BitConverter.ToString(sessionIdBytes).Replace("-", "").ToLowerInvariant();

            var nowUnix = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            var expiryUnix = DateTimeOffset.UtcNow.AddHours(1).ToUnixTimeSeconds();

            var sessionData = CreateSessionData(userId, usernameDb, userGroupId, userGroupName, clientIp, nowUnix);

            var serializer = new PHPSerializer();
            var serializedSessionData = serializer.Serialize(sessionData);

            await InsertOrUpdateSessionAsync(connection, sessionIdBytes, serializedSessionData, expiryUnix);
            await InsertOrUpdateSessionActivityAsync(connection, userId, clientIp, nowUnix);

            var authResponse = new XenforoAuthResponseDTO
            {
                Success = true,
                User = new XenforoAuthResponseDTO.XenforoUserDTO
                {
                    UserId = userId,
                    UserName = usernameDb,
                    UserGroupId = userGroupId,
                },
                SessionId = sessionIdHex
            };

            return Result<XenforoAuthResponseDTO>.Ok(authResponse);
        }
        private async Task<(int userId, int userGroupId, string usernameDb, string userGroupName, string authDataStr)?>
    GetUserRecordAsync(MySqlConnection connection, string username, string userQuery)
        {
            await using var command = new MySqlCommand(userQuery, connection);
            command.Parameters.AddWithValue("@username", username);

            await using var reader = await command.ExecuteReaderAsync();
            if (!await reader.ReadAsync())
                return null;

            var userId = reader.GetInt32("user_id");
            var userGroupId = reader.GetInt32("user_group_id");
            var usernameDb = reader.GetString("username");
            var userGroupName = reader.GetString("user_group_name");
            var authBlob = (byte[])reader["data"];
            var authDataStr = Encoding.UTF8.GetString(authBlob);

            return (userId, userGroupId, usernameDb, userGroupName, authDataStr);
        }

        private bool VerifyPassword(string authDataStr, string password)
        {
            var serializer = new PHPSerializer();
            var result = serializer.Deserialize(authDataStr);

            if (result is not Hashtable authData || !authData.ContainsKey("hash"))
                return false;

            var storedHash = authData["hash"].ToString()?.Replace("$2y$", "$2a$");
            return BCrypt.Net.BCrypt.Verify(password, storedHash);
        }

        private byte[] GenerateSessionId()
        {
            var sessionIdBytes = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(sessionIdBytes);
            return sessionIdBytes;
        }

        private Hashtable CreateSessionData(int userId, string username, int userGroupId, string userGroupName, string clientIp, long nowUnix)
        {
            var ipAddress = string.IsNullOrEmpty(clientIp) ? "0.0.0.0" : clientIp;
            var ipBytes = IPAddress.Parse(ipAddress).GetAddressBytes();
            var ipPackedString = Encoding.GetEncoding("ISO-8859-1").GetString(ipBytes);

            return new Hashtable
            {
                ["_ip"] = ipPackedString,
                ["userId"] = userId,
                ["username"] = username,
                ["userGroupId"] = userGroupId,
                ["userGroupName"] = userGroupName,
                ["passwordDate"] = nowUnix,
                ["dismissedNotices"] = new Hashtable(),
                ["lastNoticeUpdate"] = nowUnix,
                ["promotionChecked"] = true,
                ["trophyChecked"] = true,
                ["previousActivity"] = nowUnix
            };
        }

        private async Task InsertOrUpdateSessionAsync(MySqlConnection connection, byte[] sessionIdBytes, string serializedSessionData, long expiryUnix)
        {
            const string insertSessionQuery = @"
        INSERT INTO xf_session (session_id, session_data, expiry_date)
        VALUES (@sessionId, @sessionData, @expiryDate)
        ON DUPLICATE KEY UPDATE session_data = @sessionData, expiry_date = @expiryDate;
    ";

            await using var cmd = new MySqlCommand(insertSessionQuery, connection);
            cmd.Parameters.Add("@sessionId", MySqlDbType.VarBinary).Value = sessionIdBytes;
            cmd.Parameters.AddWithValue("@sessionData", serializedSessionData);
            cmd.Parameters.AddWithValue("@expiryDate", expiryUnix);
            await cmd.ExecuteNonQueryAsync();
        }

        private async Task InsertOrUpdateSessionActivityAsync(MySqlConnection connection, int userId, string clientIp, long nowUnix)
        {
            const string insertActivityQuery = @"
        INSERT INTO xf_session_activity (user_id, unique_key, ip, controller_name, controller_action, view_state, robot_key, params, view_date)
        VALUES (@userId, @uniqueKey, @ip, @controllerName, @controllerAction, 'valid', '', '', @viewDate)
        ON DUPLICATE KEY UPDATE
            user_id = VALUES(user_id),
            ip = VALUES(ip),
            controller_name = VALUES(controller_name),
            controller_action = VALUES(controller_action),
            view_state = VALUES(view_state),
            robot_key = VALUES(robot_key),
            params = VALUES(params),
            view_date = VALUES(view_date);
    ";

            var ipAddress = string.IsNullOrEmpty(clientIp) ? "0.0.0.0" : clientIp;
            var ipBytes = IPAddress.Parse(ipAddress).GetAddressBytes();
            var ipPacked = ipBytes.Length == 4 ? ipBytes : new byte[] { 0, 0, 0, 0 };

            await using var cmd = new MySqlCommand(insertActivityQuery, connection);
            cmd.Parameters.AddWithValue("@userId", userId);
            cmd.Parameters.Add("@uniqueKey", MySqlDbType.VarBinary).Value = ipPacked;
            cmd.Parameters.Add("@ip", MySqlDbType.VarBinary).Value = ipPacked;
            cmd.Parameters.AddWithValue("@controllerName", "XF\\Pub\\Controller\\ForumController");
            cmd.Parameters.AddWithValue("@controllerAction", "List");
            cmd.Parameters.AddWithValue("@viewDate", nowUnix);
            await cmd.ExecuteNonQueryAsync();
        }
    }
}
