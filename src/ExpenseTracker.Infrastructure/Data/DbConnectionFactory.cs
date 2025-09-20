using ExpenseTracker.Infrastructure.Config;
using ExpenseTracker.Infrastructure.Data.Interfaces;
using ExpenseTracker.Infrastructure.Data.Providers.Interfaces;
using Serilog;
using System.Data;
using System.Data.Common;

namespace ExpenseTracker.Infrastructure.Data
{
    public class DbConnectionFactory : IDbConnectionFactory
    {
        private readonly Dictionary<string, IDbConnectionStrategy> _strategies;
        private readonly ILogger _logger;

        public DbConnectionFactory(Dictionary<string, IDbConnectionStrategy> strategies, ILogger logger)
        {
            _strategies = strategies ?? throw new ArgumentNullException(nameof(strategies));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            var supportedProviders = string.Join(", ", _strategies.Keys);
            _logger.Information("Supported providers: {SupportedProviders}", supportedProviders);
        }

        public async Task<IDbConnection> CreateAndOpenConnectionAsync(DatabaseConfiguration dbConfig, CancellationToken ct)
        {
            if (dbConfig == null)
            {
                _logger.Error("dbConfig cannot be null.");
                throw new ArgumentNullException(nameof(dbConfig));
            }

            if (string.IsNullOrWhiteSpace(dbConfig.Provider))
            {
                _logger.Error("Provider cannot be null or empty.");
                throw new ArgumentException("Provider cannot be null or empty.", nameof(dbConfig));
            }

            string normalizedProvider = dbConfig.Provider.ToLowerInvariant().Trim();
            _logger.Debug("Creating connection for provider: {Provider}", normalizedProvider);

            if (!_strategies.TryGetValue(normalizedProvider, out IDbConnectionStrategy? strategy))
            {
                _logger.Error("Provider '{Provider}' is not supported.", normalizedProvider);
                throw new NotSupportedException($"Provider '{normalizedProvider}' is not supported.");
            }

            var connection = strategy.CreateConnection(dbConfig.ConnectionString);

            if (connection is DbConnection dbConn)
            {
                _logger.Debug("Opening database connection asynchronously.");
                await dbConn.OpenAsync(ct);
            }
            else
            {
                _logger.Debug("Opening database connection synchronously.");
                connection.Open();
            }

            _logger.Information("Database connection opened successfully.");

            return connection;
        }
    }
}
