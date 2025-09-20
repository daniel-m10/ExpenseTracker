using System.Data;

namespace ExpenseTracker.Infrastructure.Data.Providers.Interfaces
{
    public interface IDbConnectionStrategy
    {
        IDbConnection CreateConnection(string connectionString);
    }
}
