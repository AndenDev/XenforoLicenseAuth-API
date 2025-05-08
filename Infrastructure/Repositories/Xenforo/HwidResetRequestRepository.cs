using Application.Interfaces;
using Application.Interfaces.Xenforo;
using Domain.Entities;
using Domain.Enums;
using MySqlConnector;

namespace Infrastructure.Repositories
{
    public class HwidResetRequestRepository : IHwidResetRequestRepository
    {
        private readonly IMySqlDatabaseConnection _db;

        public HwidResetRequestRepository(IMySqlDatabaseConnection db)
        {
            _db = db;
        }

        public async Task AddRequestAsync(HwidResetRequest request)
        {
            const string query = @"
                INSERT INTO xf_hwid_reset_request (license_id, requested_at, reason, status)
                VALUES (@licenseId, @requestedAt, @reason, @status);";

            await using var conn = await _db.CreateOpenConnectionAsync();
            await using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@licenseId", request.LicenseId);
            cmd.Parameters.AddWithValue("@requestedAt", request.RequestedAt);
            cmd.Parameters.AddWithValue("@reason", request.Reason ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@status", request.Status.ToString());
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<IEnumerable<HwidResetRequest>> GetPendingAsync()
        {
            const string query = "SELECT * FROM xf_hwid_reset_request WHERE status = 'pending';";
            await using var conn = await _db.CreateOpenConnectionAsync();
            await using var cmd = new MySqlCommand(query, conn);
            await using var reader = await cmd.ExecuteReaderAsync();

            var list = new List<HwidResetRequest>();
            while (await reader.ReadAsync())
            {
                list.Add(Map(reader));
            }

            return list;
        }

        public async Task ApproveAsync(uint id)
        {
            const string query = @"
                UPDATE xf_hwid_reset_request
                SET status = 'approved', approved_at = NOW()
                WHERE id = @id;";

            await using var conn = await _db.CreateOpenConnectionAsync();
            await using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", id);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task DenyAsync(uint id)
        {
            const string query = @"
                UPDATE xf_hwid_reset_request
                SET status = 'denied'
                WHERE id = @id;";

            await using var conn = await _db.CreateOpenConnectionAsync();
            await using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", id);
            await cmd.ExecuteNonQueryAsync();
        }

        private HwidResetRequest Map(MySqlDataReader reader)
        {
            return new HwidResetRequest
            {
                Id = reader.GetUInt32("id"),
                LicenseId = reader.GetUInt32("license_id"),
                RequestedAt = reader.GetDateTime("requested_at"),
                ApprovedAt = reader.IsDBNull(reader.GetOrdinal("approved_at")) ? null : reader.GetDateTime("approved_at"),
                Status = Enum.Parse<HwidResetRequestStatus>(reader.GetString("status")),
                Reason = reader.IsDBNull(reader.GetOrdinal("reason")) ? null : reader.GetString("reason")
            };
        }
    }
}
