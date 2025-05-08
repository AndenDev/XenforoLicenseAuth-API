using System.Data;
using Application.Interfaces;
using Application.Interfaces.Xenforo;
using Domain.Entities;
using MySqlConnector;

namespace Infrastructure.Repositories.Xenforo
{
    public class LicenseActivationLogRepository : ILicenseActivationLogRepository
    {
        private readonly IMySqlDatabaseConnection _db;

        public LicenseActivationLogRepository(IMySqlDatabaseConnection db)
        {
            _db = db;
        }
        public async Task<IEnumerable<LicenseActivationLog>> GetByLicenseIdAsync(uint licenseId)
        {
            const string query = "SELECT * FROM xf_license_activation_log WHERE license_id = @licenseId ORDER BY activated_at DESC;";
            await using var conn = await _db.CreateOpenConnectionAsync();
            await using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@licenseId", licenseId);

            var logs = new List<LicenseActivationLog>();
            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                logs.Add(new LicenseActivationLog
                {
                    Id = reader.GetUInt32("id"),
                    LicenseId = reader.GetUInt32("license_id"),
                    UserId = reader.GetUInt32("user_id"),
                    Hwid = reader.IsDBNull("hwid") ? null : reader.GetString("hwid"),
                    IpAddress = reader.IsDBNull("ip_address") ? null : reader.GetString("ip_address"),
                    ActivatedAt = reader.GetDateTime("activated_at"),
                    Event = reader.GetString("event")
                });
            }

            return logs;
        }

        public async Task<IEnumerable<LicenseActivationLog>> GetByUserIdAsync(uint userId)
        {
            const string query = "SELECT * FROM xf_license_activation_log WHERE user_id = @userId ORDER BY activated_at DESC;";
            await using var conn = await _db.CreateOpenConnectionAsync();
            await using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@userId", userId);

            var logs = new List<LicenseActivationLog>();
            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                logs.Add(new LicenseActivationLog
                {
                    Id = reader.GetUInt32("id"),
                    LicenseId = reader.GetUInt32("license_id"),
                    UserId = reader.GetUInt32("user_id"),
                    Hwid = reader.IsDBNull("hwid") ? null : reader.GetString("hwid"),
                    IpAddress = reader.IsDBNull("ip_address") ? null : reader.GetString("ip_address"),
                    ActivatedAt = reader.GetDateTime("activated_at"),
                    Event = reader.GetString("event")
                });
            }

            return logs;
        }

        public async Task AddAsync(LicenseActivationLog log)
        {
            const string query = @"
                INSERT INTO xf_license_activation_log 
                (license_id, user_id, hwid, ip_address, activated_at, event)
                VALUES (@licenseId, @userId, @hwid, @ip, @activatedAt, @event);
            ";

            await using var conn = await _db.CreateOpenConnectionAsync();
            await using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@licenseId", log.LicenseId);
            cmd.Parameters.AddWithValue("@userId", log.UserId);
            cmd.Parameters.AddWithValue("@hwid", log.Hwid ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@ip", log.IpAddress ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@activatedAt", log.ActivatedAt);
            cmd.Parameters.AddWithValue("@event", log.Event);
            await cmd.ExecuteNonQueryAsync();
        }
    }
}
