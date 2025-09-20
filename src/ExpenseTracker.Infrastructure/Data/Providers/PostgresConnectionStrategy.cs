using ExpenseTracker.Infrastructure.Data.Providers.Interfaces;
using Npgsql;
using System.Data;

namespace ExpenseTracker.Infrastructure.Data.Providers
{
    public class PostgresConnectionStrategy : IDbConnectionStrategy
    {
        public IDbConnection CreateConnection(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentException("Connection string cannot be null or empty.", nameof(connectionString));

            return new NpgsqlConnection(connectionString);
        }
    }
}
