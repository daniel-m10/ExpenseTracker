using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using ExpenseTracker.Infrastructure.Abstractions;
using Microsoft.Extensions.Configuration;
using Serilog;
using System.Text.Json;

namespace ExpenseTracker.Infrastructure.Config
{
    public class CompositeConfigLoader(IConfiguration configuration, ILogger logger, IAmazonSecretsManager secretsManager) : IDbConfig
    {
        private readonly IConfiguration _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        private readonly ILogger _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        private readonly IAmazonSecretsManager _secretsManager = secretsManager ?? throw new ArgumentNullException(nameof(secretsManager));

        private const string DatabaseProviderKey = "Database:Provider";
        private const string DatabaseConnectionStringKey = "Database:ConnectionString";
        private const string AwsSecretNameKey = "AWS:SecretName";
        private const string ProviderJsonKey = "provider";
        private const string ConnectionStringJsonKey = "connectionString";

        public async Task<(string Provider, string ConnectionString)> LoadAsync()
        {
            _logger.Debug("Starting database configuration load process");

            // Try local config first
            var localConfig = TryLoadLocalConfiguration();
            if (localConfig.HasValue)
            {
                _logger.Information("Loaded DB config from local configuration.");
                return localConfig.Value;
            }

            _logger.Warning("Local DB config missing or invalid, falling back to AWS Secrets Manager.");

            // Load from AWS Secrets Manager
            return await LoadFromAwsSecretsManagerAsync();
        }

        private (string Provider, string ConnectionString)? TryLoadLocalConfiguration()
        {
            var provider = _configuration[DatabaseProviderKey];
            var connectionString = _configuration[DatabaseConnectionStringKey];

            if (!string.IsNullOrWhiteSpace(provider) && !string.IsNullOrWhiteSpace(connectionString))
            {
                _logger.Debug("Found valid local database configuration");
                return (provider, connectionString);
            }

            _logger.Debug("Local database configuration is incomplete or missing");
            return null;
        }

        private async Task<(string Provider, string ConnectionString)> LoadFromAwsSecretsManagerAsync()
        {
            var secretName = GetAwsSecretName();
            _logger.Debug("Attempting to load secret from AWS: {SecretName}", secretName);

            try
            {
                var secretValue = await RetrieveSecretValueAsync(secretName);
                var (provider, connectionString) = ParseSecretValue(secretValue);

                _logger.Information("Loaded DB config from AWS Secrets Manager.");
                return (provider, connectionString);
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            catch (JsonException ex)
            {
                _logger.Error("Failed to parse AWS secret JSON: {Error}", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                throw;
            }
        }

        private string GetAwsSecretName()
        {
            var secretName = _configuration[AwsSecretNameKey];
            if (string.IsNullOrWhiteSpace(secretName))
            {
                _logger.Error("Aws:SecretName is not configured.");
                throw new InvalidOperationException("Aws:SecretName is not configured.");
            }
            return secretName;
        }

        private async Task<string> RetrieveSecretValueAsync(string secretName)
        {
            var request = new GetSecretValueRequest { SecretId = secretName };
            var response = await _secretsManager.GetSecretValueAsync(request);

            if (string.IsNullOrWhiteSpace(response.SecretString))
            {
                _logger.Error("AWS secret value is empty.");
                throw new InvalidOperationException("AWS secret value is empty.");
            }

            return response.SecretString;
        }

        private (string Provider, string ConnectionString) ParseSecretValue(string secretValue)
        {
            using var doc = JsonDocument.Parse(secretValue);
            var root = doc.RootElement;

            var provider = TryGetStringProperty(root, ProviderJsonKey);
            var connectionString = TryGetStringProperty(root, ConnectionStringJsonKey);

            if (string.IsNullOrWhiteSpace(provider) || string.IsNullOrWhiteSpace(connectionString))
            {
                _logger.Error("AWS secret is missing required fields.");
                throw new InvalidOperationException("AWS secret is missing required fields.");
            }

            _logger.Debug("Successfully parsed AWS secret configuration");
            return (provider, connectionString);
        }

        private static string? TryGetStringProperty(JsonElement element, string propertyName)
        {
            return element.TryGetProperty(propertyName, out var property) ? property.GetString() : null;
        }
    }
}
