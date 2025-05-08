using Application.Interfaces;
using Application.Interfaces.Xenforo;
using Domain.Enums;
using MySqlConnector;

namespace Infrastructure.Repositories.Xenforo
{
    public class ApplicationRepository : IApplicationRepository
    {
        private readonly IMySqlDatabaseConnection _db;

        public ApplicationRepository(IMySqlDatabaseConnection db)
        {
            _db = db;
        }

        public async Task<Domain.Entities.Application?> GetByIdAsync(uint id)
        {
            const string query = "SELECT * FROM xf_application WHERE id = @id LIMIT 1;";
            await using var conn = await _db.CreateOpenConnectionAsync();
            await using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", id);

            await using var reader = await cmd.ExecuteReaderAsync();
            return await reader.ReadAsync() ? Map(reader) : null;
        }

        public async Task<IEnumerable<Domain.Entities.Application>> GetAllAsync()
        {
            const string query = "SELECT * FROM xf_application ORDER BY created_at DESC;";
            await using var conn = await _db.CreateOpenConnectionAsync();
            await using var cmd = new MySqlCommand(query, conn);

            var apps = new List<Domain.Entities.Application>();
            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                apps.Add(Map(reader));
            }

            return apps;
        }
        public async Task<uint> AddAsync(Domain.Entities.Application app)
        {
            const string query = @"
        INSERT INTO xf_application (name, status, logo_url, description, created_at, is_active)
        VALUES (@name, @status, @logoUrl, @description, @createdAt, @isActive);
        SELECT LAST_INSERT_ID();";

            await using var conn = await _db.CreateOpenConnectionAsync();
            await using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@name", app.Name);
            cmd.Parameters.AddWithValue("@status", app.Status.ToString());
            cmd.Parameters.AddWithValue("@logoUrl", app.LogoUrl ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@description", app.Description ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@createdAt", app.CreatedAt);
            cmd.Parameters.AddWithValue("@isActive", app.IsActive);

            return Convert.ToUInt32(await cmd.ExecuteScalarAsync());
        }

 
        public async Task<bool> UpdateAsync(Domain.Entities.Application app)
        {
            const string query = @"
        UPDATE xf_application
        SET name = @name, status = @status, logo_url = @logoUrl, description = @description, is_active = @isActive
        WHERE id = @id;";

            await using var conn = await _db.CreateOpenConnectionAsync();
            await using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", app.Id);
            cmd.Parameters.AddWithValue("@name", app.Name);
            cmd.Parameters.AddWithValue("@status", app.Status.ToString());
            cmd.Parameters.AddWithValue("@logoUrl", app.LogoUrl ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@description", app.Description ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@isActive", app.IsActive);

            return await cmd.ExecuteNonQueryAsync() > 0;
        }


        public async Task<bool> DeleteAsync(uint id)
        {
            const string query = "DELETE FROM xf_application WHERE id = @id;";

            await using var conn = await _db.CreateOpenConnectionAsync();
            await using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", id);

            return await cmd.ExecuteNonQueryAsync() > 0;
        }
        private Domain.Entities.Application Map(MySqlDataReader reader)
        {
            return new Domain.Entities.Application
            {
                Id = reader.GetUInt32("id"),
                Name = reader.GetString("name"),
                Status = Enum.Parse<ApplicationStatus>(reader.GetString("status")),
                LogoUrl = reader.IsDBNull(reader.GetOrdinal("logo_url")) ? null : reader.GetString("logo_url"),
                Description = reader.IsDBNull(reader.GetOrdinal("description")) ? null : reader.GetString("description"),
                CreatedAt = reader.GetDateTime("created_at"),
                IsActive = reader.GetBoolean("is_active")
            };
        }
    }
}
