using System.Data;
using Application.Interfaces;
using Application.Interfaces.Xenforo;
using Domain.Entities;
using MySqlConnector;

namespace Infrastructure.Repositories.Xenforo
{
    public class HwidBanListRepository : IHwidBanListRepository
    {
        private readonly IMySqlDatabaseConnection _db;

        public HwidBanListRepository(IMySqlDatabaseConnection db)
        {
            _db = db;
        }
        public async Task<IEnumerable<HwidBanEntry>> GetAllBansAsync()
        {
            const string query = "SELECT * FROM xf_hwid_ban_list WHERE is_active = 1 ORDER BY banned_at DESC;";
            await using var conn = await _db.CreateOpenConnectionAsync();
            await using var cmd = new MySqlCommand(query, conn);

            var bans = new List<HwidBanEntry>();
            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                bans.Add(new HwidBanEntry
                {
                    Id = reader.GetUInt32("id"),
                    Hwid = reader.GetString("hwid"),
                    Reason = reader.IsDBNull("reason") ? null : reader.GetString("reason"),
                    BannedByUsername = reader.IsDBNull("banned_by_username") ? null : reader.GetString("banned_by_username"),
                    BannedAt = reader.GetDateTime("banned_at"),
                    IsActive = reader.GetBoolean("is_active")
                });
            }

            return bans;
        }

        public async Task<bool> IsBannedAsync(string hwid)
        {
            const string query = "SELECT COUNT(*) FROM xf_hwid_ban_list WHERE hwid = @hwid AND is_active = 1;";
            await using var conn = await _db.CreateOpenConnectionAsync();
            await using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@hwid", hwid);
            var result = await cmd.ExecuteScalarAsync();
            return Convert.ToInt32(result) > 0;
        }

        public async Task AddBanAsync(string hwid, string? reason, string? bannedByUsername)
        {
            const string query = @"
                INSERT INTO xf_hwid_ban_list (hwid, reason, banned_by_username, banned_at, is_active)
                VALUES (@hwid, @reason, @bannedBy, NOW(), 1)
                ON DUPLICATE KEY UPDATE is_active = 1, reason = @reason, banned_by_username = @bannedBy, banned_at = NOW();
            ";
            await using var conn = await _db.CreateOpenConnectionAsync();
            await using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@hwid", hwid);
            cmd.Parameters.AddWithValue("@reason", reason ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@bannedBy", bannedByUsername ?? (object)DBNull.Value);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task RemoveBanAsync(string hwid)
        {
            const string query = "UPDATE xf_hwid_ban_list SET is_active = 0 WHERE hwid = @hwid;";
            await using var conn = await _db.CreateOpenConnectionAsync();
            await using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@hwid", hwid);
            await cmd.ExecuteNonQueryAsync();
        }
    }
}
