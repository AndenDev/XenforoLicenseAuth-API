
using MySqlConnector;

namespace Application.Interfaces
{
    public interface IMySqlDatabaseConnection
    {
        Task<MySqlConnection> CreateOpenConnectionAsync();
    }
}
