using ExpenseTracker.Infrastructure.Config;
using System.Data;

namespace ExpenseTracker.Infrastructure.Data.Interfaces
{
    public interface IDbConnectionFactory
    {
        Task<IDbConnection> CreateAndOpenConnectionAsync(DatabaseConfiguration dbConfig, CancellationToken ct);
    }
}
