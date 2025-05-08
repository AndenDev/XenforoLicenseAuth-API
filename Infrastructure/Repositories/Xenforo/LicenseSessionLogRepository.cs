using System.Data;
using Application.Interfaces;
using Application.Interfaces.Xenforo;
using Domain.Entities;
using MySqlConnector;

namespace Infrastructure.Repositories.Xenforo
{
    public class LicenseSessionLogRepository : ILicenseSessionLogRepository
    {
        private readonly IMySqlDatabaseConnection _db;

        public LicenseSessionLogRepository(IMySqlDatabaseConnection db)
        {
            _db = db;
        }
        public async Task<IEnumerable<LicenseSessionLog>> GetByLicenseIdAsync(uint licenseId)
        {
            const string query = "SELECT * FROM xf_license_session_log WHERE license_id = @licenseId ORDER BY started_at DESC;";
            await using var conn = await _db.CreateOpenConnectionAsync();
            await using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@licenseId", licenseId);

            var sessions = new List<LicenseSessionLog>();
            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                sessions.Add(new LicenseSessionLog
                {
                    Id = reader.GetUInt32("id"),
                    LicenseId = reader.GetUInt32("license_id"),
                    SessionId = reader.GetString("session_id"),
                    StartedAt = reader.GetDateTime("started_at"),
                    EndedAt = reader.IsDBNull("ended_at") ? null : reader.GetDateTime("ended_at"),
                    EndedReason = reader.IsDBNull("ended_reason") ? null : reader.GetString("ended_reason"),
                    IpAddress = reader.IsDBNull("ip_address") ? null : reader.GetString("ip_address"),
                    Hwid = reader.IsDBNull("hwid") ? null : reader.GetString("hwid")
                });
            }

            return sessions;
        }

        public async Task<IEnumerable<LicenseSessionLog>> GetActiveSessionsAsync()
        {
            const string query = "SELECT * FROM xf_license_session_log WHERE ended_at IS NULL;";
            await using var conn = await _db.CreateOpenConnectionAsync();
            await using var cmd = new MySqlCommand(query, conn);

            var sessions = new List<LicenseSessionLog>();
            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                sessions.Add(new LicenseSessionLog
                {
                    Id = reader.GetUInt32("id"),
                    LicenseId = reader.GetUInt32("license_id"),
                    SessionId = reader.GetString("session_id"),
                    StartedAt = reader.GetDateTime("started_at"),
                    EndedAt = null,
                    EndedReason = null,
                    IpAddress = reader.IsDBNull("ip_address") ? null : reader.GetString("ip_address"),
                    Hwid = reader.IsDBNull("hwid") ? null : reader.GetString("hwid")
                });
            }

            return sessions;
        }

        public async Task AddAsync(LicenseSessionLog log)
        {
            const string query = @"
                INSERT INTO xf_license_session_log 
                (license_id, session_id, started_at, ip_address, hwid)
                VALUES (@licenseId, @sessionId, @startedAt, @ip, @hwid);
            ";

            await using var conn = await _db.CreateOpenConnectionAsync();
            await using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@licenseId", log.LicenseId);
            cmd.Parameters.AddWithValue("@sessionId", log.SessionId);
            cmd.Parameters.AddWithValue("@startedAt", log.StartedAt);
            cmd.Parameters.AddWithValue("@ip", log.IpAddress ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@hwid", log.Hwid ?? (object)DBNull.Value);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task EndSessionAsync(string sessionId, string endedReason)
        {
            const string query = @"
                UPDATE xf_license_session_log 
                SET ended_at = NOW(), ended_reason = @reason 
                WHERE session_id = @sessionId AND ended_at IS NULL;
            ";

            await using var conn = await _db.CreateOpenConnectionAsync();
            await using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@sessionId", sessionId);
            cmd.Parameters.AddWithValue("@reason", endedReason);
            await cmd.ExecuteNonQueryAsync();
        }
    }
}
