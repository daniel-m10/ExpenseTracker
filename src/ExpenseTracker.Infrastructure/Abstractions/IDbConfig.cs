namespace ExpenseTracker.Infrastructure.Abstractions
{
    public interface IDbConfig
    {
        Task<(string Provider, string ConnectionString)> LoadAsync();
    }
}
