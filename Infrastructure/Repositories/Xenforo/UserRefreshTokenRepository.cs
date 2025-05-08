using Application.Interfaces;
using Application.Interfaces.Xenforo;
using Domain.Entities;
using MySqlConnector;

namespace Infrastructure.Repositories.Xenforo
{
    public class UserRefreshTokenRepository : IUserRefreshTokenRepository
    {
        private readonly IMySqlDatabaseConnection _db;

        public UserRefreshTokenRepository(IMySqlDatabaseConnection db)
        {
            _db = db;
        }

        public async Task<UserRefreshToken?> GetByTokenAsync(string refreshToken)
        {
            const string query = "SELECT * FROM xf_user_refresh_token WHERE refresh_token = @token LIMIT 1;";

            await using var conn = await _db.CreateOpenConnectionAsync();
            await using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@token", refreshToken);

            await using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return Map(reader);
            }

            return null;
        }

        public async Task AddAsync(UserRefreshToken token)
        {
            const string query = @"
                INSERT INTO xf_user_refresh_token (user_id, refresh_token, expires_at, created_at)
                VALUES (@userId, @refreshToken, @expiresAt, @createdAt);
            ";

            await using var conn = await _db.CreateOpenConnectionAsync();
            await using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@userId", token.UserId);
            cmd.Parameters.AddWithValue("@refreshToken", token.RefreshToken);
            cmd.Parameters.AddWithValue("@expiresAt", token.ExpiresAt);
            cmd.Parameters.AddWithValue("@createdAt", token.CreatedAt);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync(string refreshToken)
        {
            const string query = "DELETE FROM xf_user_refresh_token WHERE refresh_token = @token;";
            await using var conn = await _db.CreateOpenConnectionAsync();
            await using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@token", refreshToken);
            await cmd.ExecuteNonQueryAsync();
        }

        private UserRefreshToken Map(MySqlDataReader reader)
        {
            return new UserRefreshToken
            {
                Id = reader.GetUInt32("id"),
                UserId = reader.GetUInt32("user_id"),
                RefreshToken = reader.GetString("refresh_token"),
                ExpiresAt = reader.GetDateTime("expires_at"),
                CreatedAt = reader.GetDateTime("created_at")
            };
        }
    }
}
