using System.Data;

namespace ExpenseTracker.Infrastructure.Data.Interfaces
{
    public interface IDbConnectionFactory
    {
        Task<IDbConnection> CreateAndOpenConnectionAsync(string provider, string connectionString, CancellationToken ct);
    }
}
