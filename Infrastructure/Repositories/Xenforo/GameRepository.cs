using Application.Interfaces;
using Domain.Entities;
using Application.Interfaces.Xenforo;
using MySqlConnector;
using Domain.Enums;


namespace Infrastructure.Repositories
{
    public class GameRepository : IGameRepository
    {
        private readonly IMySqlDatabaseConnection _db;

        public GameRepository(IMySqlDatabaseConnection db)
        {
            _db = db;
        }

        public async Task<Game?> GetByIdAsync(uint id)
        {
            const string query = "SELECT * FROM xf_game WHERE id = @id LIMIT 1;";
            await using var conn = await _db.CreateOpenConnectionAsync();
            await using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", id);

            await using var reader = await cmd.ExecuteReaderAsync();
            return await reader.ReadAsync() ? Map(reader) : null;
        }

        public async Task<IEnumerable<Game>> GetAllAsync()
        {
            const string query = "SELECT * FROM xf_game ORDER BY name;";
            await using var conn = await _db.CreateOpenConnectionAsync();
            await using var cmd = new MySqlCommand(query, conn);

            var results = new List<Game>();
            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                results.Add(Map(reader));
            }

            return results;
        }
        public async Task<uint> Addsync(Game game)
        {
            const string query = @"
        INSERT INTO xf_game (name, status, logo_url, last_updated, is_active)
        VALUES (@name, @status, @logoUrl, @lastUpdated, @isActive);
        SELECT LAST_INSERT_ID();";

            await using var conn = await _db.CreateOpenConnectionAsync();
            await using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@name", game.Name);
            cmd.Parameters.AddWithValue("@status", game.Status.ToString());
            cmd.Parameters.AddWithValue("@logoUrl", game.LogoUrl ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@lastUpdated", game.LastUpdated);
            cmd.Parameters.AddWithValue("@isActive", game.IsActive);

            return Convert.ToUInt32(await cmd.ExecuteScalarAsync());
        }

        // Update
        public async Task<bool> UpdateAsync(Game game)
        {
            const string query = @"
                UPDATE xf_game
                SET name = @name, status = @status, logo_url = @logoUrl, last_updated = @lastUpdated, is_active = @isActive
                WHERE id = @id;";

            await using var conn = await _db.CreateOpenConnectionAsync();
            await using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", game.Id);
            cmd.Parameters.AddWithValue("@name", game.Name);
            cmd.Parameters.AddWithValue("@status", game.Status.ToString());
            cmd.Parameters.AddWithValue("@logoUrl", game.LogoUrl ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@lastUpdated", game.LastUpdated);
            cmd.Parameters.AddWithValue("@isActive", game.IsActive);

            return await cmd.ExecuteNonQueryAsync() > 0;
        }

        // Delete
        public async Task<bool> DeleteAsync(uint id)
        {
            const string query = "DELETE FROM xf_game WHERE id = @id;";

            await using var conn = await _db.CreateOpenConnectionAsync();
            await using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", id);

            return await cmd.ExecuteNonQueryAsync() > 0;
        }
        private Game Map(MySqlDataReader reader)
        {
            return new Game
            {
                Id = reader.GetUInt32("id"),
                Name = reader.GetString("name"),
                Status = Enum.Parse<GameStatus>(reader.GetString("status")),
                LogoUrl = reader.IsDBNull(reader.GetOrdinal("logo_url")) ? null : reader.GetString("logo_url"),
                LastUpdated = reader.GetDateTime("last_updated"),
                IsActive = reader.GetBoolean("is_active")
            };
        }
    }
}
