using Application.Interfaces;
using Microsoft.Extensions.Configuration;
using MySqlConnector;

namespace Infrastructure.Services
{
    public class MySqlDatabaseConnection : IMySqlDatabaseConnection
    {
        private readonly string _connectionString;

        public MySqlDatabaseConnection(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("XenForoDb");
        }

        public async Task<MySqlConnection> CreateOpenConnectionAsync()
        {
            var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();
            return connection;
        }
    }
}
