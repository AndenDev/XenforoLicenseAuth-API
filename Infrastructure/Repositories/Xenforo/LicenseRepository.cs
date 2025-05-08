using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces;
using Application.Interfaces.Xenforo;
using Domain.Entities;
using Domain.Enums;
using MySqlConnector;

namespace Infrastructure.Repositories.Xenforo
{
    public class LicenseRepository : ILicenseRepository
    {
        private readonly IMySqlDatabaseConnection _db;

        public LicenseRepository(IMySqlDatabaseConnection db)
        {
            _db = db;
        }
        public async Task<IEnumerable<License>> GetAllAsync()
        {
            const string query = "SELECT * FROM xf_license;";
            await using var conn = await _db.CreateOpenConnectionAsync();
            await using var cmd = new MySqlCommand(query, conn);

            var list = new List<License>();
            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
                list.Add(Map(reader));

            return list;
        }

        public async Task<License?> GetByKeyAsync(string licenseKey)
        {
            const string query = "SELECT * FROM xf_license WHERE license_key = @key LIMIT 1;";
            await using var conn = await _db.CreateOpenConnectionAsync();
            await using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@key", licenseKey);
            await using var reader = await cmd.ExecuteReaderAsync();
            return await reader.ReadAsync() ? Map(reader) : null;
        }

        public async Task<IEnumerable<License>> GetByUserIdAsync(uint userId)
        {
            const string query = "SELECT * FROM xf_license WHERE user_id = @uid;";
            await using var conn = await _db.CreateOpenConnectionAsync();
            await using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@uid", userId);
            var list = new List<License>();
            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
                list.Add(Map(reader));
            return list;
        }

        public async Task AddAsync(License license)
        {
            const string query = "INSERT INTO xf_license (user_id, license_plan_id, license_key, hwids, status, expires_at, created_at) VALUES (@userId, @planId, @key, @hwids, @status, @expires, NOW());";
            await using var conn = await _db.CreateOpenConnectionAsync();
            await using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@userId", license.UserId);
            cmd.Parameters.AddWithValue("@planId", license.LicensePlanId);
            cmd.Parameters.AddWithValue("@key", license.LicenseKey);
            cmd.Parameters.AddWithValue("@hwids", license.Hwids);
            cmd.Parameters.AddWithValue("@status", license.Status.ToString());
            cmd.Parameters.AddWithValue("@expires", license.ExpiresAt);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task UpdateAsync(License license)
        {
            const string query = "UPDATE xf_license SET hwids = @hwids, status = @status, expires_at = @expires WHERE id = @id;";
            await using var conn = await _db.CreateOpenConnectionAsync();
            await using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@hwids", license.Hwids);
            cmd.Parameters.AddWithValue("@status", license.Status.ToString());
            cmd.Parameters.AddWithValue("@expires", license.ExpiresAt);
            cmd.Parameters.AddWithValue("@id", license.Id);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync(uint id)
        {
            const string query = "DELETE FROM xf_license WHERE id = @id;";
            await using var conn = await _db.CreateOpenConnectionAsync();
            await using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", id);
            await cmd.ExecuteNonQueryAsync();
        }
        private License Map(MySqlDataReader reader)
        {
            return new License
            {
                Id = reader.GetUInt32("id"),
                UserId = reader.IsDBNull(reader.GetOrdinal("user_id")) ? null : reader.GetUInt32("user_id"),
                LicensePlanId = reader.GetUInt32("license_plan_id"),
                LicenseKey = reader.GetString("license_key"),
                Hwids = reader.IsDBNull(reader.GetOrdinal("hwids")) ? null : reader.GetString("hwids"),
                Status = Enum.Parse<LicenseStatus>(reader.GetString("status")),
                ExpiresAt = reader.IsDBNull(reader.GetOrdinal("expires_at")) ? null : reader.GetDateTime("expires_at"),
                CreatedAt = reader.GetDateTime("created_at")
            };
        }

    }
}
