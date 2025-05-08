using Application.Interfaces;

using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MySqlConnection _connection;
        private readonly MySqlTransaction _transaction;

        public UnitOfWork(IMySqlDatabaseConnection dbFactory)
        {
            _connection = dbFactory.CreateOpenConnectionAsync().Result;
            _transaction = _connection.BeginTransaction();
        }

        public async Task CommitAsync()
        {
            await _transaction.CommitAsync();
        }

        public async Task RollbackAsync()
        {
            await _transaction.RollbackAsync();
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _connection?.Dispose();
        }
    }
}
