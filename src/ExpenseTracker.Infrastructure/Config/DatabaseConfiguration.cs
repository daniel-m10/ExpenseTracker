namespace ExpenseTracker.Infrastructure.Config
{
    public class DatabaseConfiguration
    {
        public string Provider { get; init; } = string.Empty;
        public string ConnectionString { get; init; } = string.Empty;
    }
}
