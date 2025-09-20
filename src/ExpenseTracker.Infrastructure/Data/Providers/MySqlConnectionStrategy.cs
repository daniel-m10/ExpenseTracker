using ExpenseTracker.Infrastructure.Data.Providers.Interfaces;
using MySqlConnector;
using System.Data;

namespace ExpenseTracker.Infrastructure.Data.Providers
{
    public class MySqlConnectionStrategy : IDbConnectionStrategy
    {
        public IDbConnection CreateConnection(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentException("Connection string cannot be null or empty.", nameof(connectionString));

            return new MySqlConnection(connectionString);
        }
    }
}
