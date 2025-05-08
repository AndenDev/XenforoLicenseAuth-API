using Application.Interfaces;
using Application.Interfaces.Xenforo;
using Domain.Entities;
using Domain.Enums;
using MySqlConnector;

namespace Infrastructure.Repositories.Xenforo
{
    public class ClientBuildRepository : IClientBuildRepository
    {
        private readonly IMySqlDatabaseConnection _db;

        public ClientBuildRepository(IMySqlDatabaseConnection db)
        {
            _db = db;
        }
        public async Task<IEnumerable<ClientBuild>> GetAllAsync()
        {
            const string query = "SELECT * FROM xf_client_build ORDER BY released_at DESC;";

            await using var conn = await _db.CreateOpenConnectionAsync();
            await using var cmd = new MySqlCommand(query, conn);

            var builds = new List<ClientBuild>();
            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                builds.Add(Map(reader));
            }

            return builds;
        }

        public async Task<IEnumerable<ClientBuild>> GetByApplicationIdAsync(uint appId)
        {
            const string query = @"
                    SELECT * FROM xf_client_build
                    WHERE application_id = @appId
                    ORDER BY released_at DESC;";

            await using var conn = await _db.CreateOpenConnectionAsync();
            await using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@appId", appId);

            var builds = new List<ClientBuild>();
            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                builds.Add(Map(reader));
            }

            return builds;
        }

        public async Task<uint> AddAsync(ClientBuild build)
        {
            const string query = @"
                INSERT INTO xf_client_build (application_id, version, build_hash, status, released_at, notes)
                VALUES (@appId, @version, @buildHash, @status, @releasedAt, @notes);
                SELECT LAST_INSERT_ID();";

            await using var conn = await _db.CreateOpenConnectionAsync();
            await using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@appId", build.ApplicationId);
            cmd.Parameters.AddWithValue("@version", build.Version);
            cmd.Parameters.AddWithValue("@buildHash", build.BuildHash);
            cmd.Parameters.AddWithValue("@status", build.Status.ToString());
            cmd.Parameters.AddWithValue("@releasedAt", build.ReleasedAt);
            cmd.Parameters.AddWithValue("@notes", build.Notes ?? (object)DBNull.Value);

            return Convert.ToUInt32(await cmd.ExecuteScalarAsync());
        }

       
        public async Task<bool> UpdateAsync(ClientBuild build)
        {
            const string query = @"
                UPDATE xf_client_build
                SET version = @version, build_hash = @buildHash, status = @status, released_at = @releasedAt, notes = @notes
                WHERE id = @id;";

            await using var conn = await _db.CreateOpenConnectionAsync();
            await using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", build.Id);
            cmd.Parameters.AddWithValue("@version", build.Version);
            cmd.Parameters.AddWithValue("@buildHash", build.BuildHash);
            cmd.Parameters.AddWithValue("@status", build.Status.ToString());
            cmd.Parameters.AddWithValue("@releasedAt", build.ReleasedAt);
            cmd.Parameters.AddWithValue("@notes", build.Notes ?? (object)DBNull.Value);

            return await cmd.ExecuteNonQueryAsync() > 0;
        }

       
        public async Task<bool> DeleteAsync(uint id)
        {
            const string query = "DELETE FROM xf_client_build WHERE id = @id;";

            await using var conn = await _db.CreateOpenConnectionAsync();
            await using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", id);

            return await cmd.ExecuteNonQueryAsync() > 0;
        }
        private ClientBuild Map(MySqlDataReader reader)
        {
            return new ClientBuild
            {
                Id = reader.GetUInt32("id"),
                ApplicationId = reader.GetUInt32("application_id"),
                Version = reader.GetString("version"),
                BuildHash = reader.GetString("build_hash"),
                Status = Enum.Parse<ClientBuildStatus>(reader.GetString("status")),
                ReleasedAt = reader.GetDateTime("released_at"),
                Notes = reader.IsDBNull(reader.GetOrdinal("notes")) ? null : reader.GetString("notes")
            };
        }
    }
}
