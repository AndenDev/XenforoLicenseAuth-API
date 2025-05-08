using Application.Interfaces;
using Application.Interfaces.Xenforo;
using Domain.Entities;
using MySqlConnector;

namespace Infrastructure.Repositories.Xenforo
{
    public class LicensePlanRepository : ILicensePlanRepository
    {
        private readonly IMySqlDatabaseConnection _dbConnection;

        public LicensePlanRepository(IMySqlDatabaseConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<LicensePlan?> GetByIdAsync(uint id)
        {
            const string query = "SELECT * FROM xf_license_plan WHERE id = @id AND is_active = 1 LIMIT 1;";

            await using var connection = await _dbConnection.CreateOpenConnectionAsync();
            await using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@id", id);

            await using var reader = await cmd.ExecuteReaderAsync();
            return await reader.ReadAsync() ? Map(reader) : null;
        }

        public async Task<IEnumerable<LicensePlan>> GetAllAsync()
        {
            const string query = "SELECT * FROM xf_license_plan WHERE is_active = 1;";

            await using var connection = await _dbConnection.CreateOpenConnectionAsync();
            await using var cmd = new MySqlCommand(query, connection);

            var plans = new List<LicensePlan>();
            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                plans.Add(Map(reader));
            }

            return plans;
        }

        private LicensePlan Map(MySqlDataReader reader)
        {
            return new LicensePlan
            {
                Id = reader.GetUInt32("id"),
                ApplicationId = reader.GetUInt32("application_id"),
                Name = reader.GetString("name"),
                Price = reader.GetDecimal("price"),
                DurationDays = reader.GetInt32("duration_days"),
                IsActive = reader.GetBoolean("is_active"),
                CreatedAt = reader.GetDateTime("created_at")
            };
        }
    }
}
