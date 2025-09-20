namespace ExpenseTracker.Infrastructure.Config.Interfaces
{
    public interface ICompositeConfigLoader
    {
        Task<(string Provider, string ConnectionString)> LoadAsync();
    }
}
