using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using ExpenseTracker.Infrastructure.Config;
using Microsoft.Extensions.Configuration;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Serilog;
using System.Text.Json;

namespace ExpenseTracker.Infrastructure.Tests.Config
{
    [TestFixture]
    public class CompositeConfigLoaderTests
    {
        private IConfiguration _configuration;
        private ILogger _logger;
        private IAmazonSecretsManager _secretsManager;

        [SetUp]
        public void SetUp()
        {
            _logger = Substitute.For<ILogger>();
            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    {"Database:Provider", "provider" },
                    {"Database:ConnectionString", "connectionString" }
                }).Build();
            _secretsManager = Substitute.For<IAmazonSecretsManager>();
        }

        [TearDown]
        public void TearDown()
        {
            _secretsManager.Dispose();
        }

        [Test]
        public async Task LoadAsync_ReturnsConfig_WhenLocalConfigIsPresent()
        {
            // Arrange
            var configLoader = new CompositeConfigLoader(_configuration, _logger, _secretsManager);

            // Act
            var (Provider, ConnectionString) = await configLoader.LoadAsync();

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(Provider, Is.Not.Null);
                Assert.That(Provider, Is.EqualTo("provider"));
                Assert.That(ConnectionString, Is.Not.Null);
                Assert.That(ConnectionString, Is.EqualTo("connectionString"));
            }
        }

        [Test]
        public async Task LoadAsync_LogsInformation_WhenLocalConfigIsUsed()
        {
            // Arrange
            var configLoader = new CompositeConfigLoader(_configuration, _logger, _secretsManager);

            // Act
            var (Provider, ConnectionString) = await configLoader.LoadAsync();

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(Provider, Is.Not.Null);
                Assert.That(ConnectionString, Is.Not.Null);
            }
            _logger.Received(1).Information("Loaded DB config from local configuration.");
        }

        [Test]
        public async Task LoadAsync_ReturnsAwsConfig_WhenLocalConfigIsMissing()
        {
            // Arrange
            var configuration = new ConfigurationBuilder()
                    .AddInMemoryCollection(new Dictionary<string, string?>()
                    {
                        {"Aws:SecretName", "secret-name-test" }
                    })
                    .Build(); ;

            _secretsManager.GetSecretValueAsync(
            Arg.Any<GetSecretValueRequest>(),
            Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(new GetSecretValueResponse
            {
                SecretString = "{\"provider\":\"postgres\",\"connectionString\":\"Host=...\"}"
            }));

            var configLoader = new CompositeConfigLoader(configuration, _logger, _secretsManager);

            // Act

            var (Provider, ConnectionString) = await configLoader.LoadAsync();

            // Assert
            _logger.Received(1).Warning("Local DB config missing or invalid, falling back to AWS Secrets Manager.");
            _logger.Received(1).Information("Loaded DB config from AWS Secrets Manager.");
            using (Assert.EnterMultipleScope())
            {
                Assert.That(Provider, Is.Not.Default);
                Assert.That(ConnectionString, Is.Not.Default);
            }
        }

        [Test]
        public async Task LoadAsync_ReturnsAwsConfig_WhenLocalConfigIsEmpty()
        {
            // Arrange
            _configuration["Database:Provider"] = string.Empty;
            _configuration["Database:ConnectionString"] = string.Empty;
            _configuration["Aws:SecretName"] = "secret-name-test";

            _secretsManager.GetSecretValueAsync(
            Arg.Any<GetSecretValueRequest>(),
            Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(new GetSecretValueResponse
            {
                SecretString = "{\"provider\":\"postgres\",\"connectionString\":\"Host=...\"}"
            }));

            var configLoader = new CompositeConfigLoader(_configuration, _logger, _secretsManager);

            // Act
            var (Provider, ConnectionString) = await configLoader.LoadAsync();

            // Assert
            _logger.Received(1).Warning("Local DB config missing or invalid, falling back to AWS Secrets Manager.");
            _logger.Received(1).Information("Loaded DB config from AWS Secrets Manager.");
            using (Assert.EnterMultipleScope())
            {
                Assert.That(Provider, Is.Not.Default);
                Assert.That(ConnectionString, Is.Not.Default);
            }
        }

        [Test]
        public async Task LoadAsync_LogsWarning_WhenFallingBackToAwsSecretsManager()
        {
            // Arrange
            var configuration = new ConfigurationBuilder()
                    .AddInMemoryCollection(new Dictionary<string, string?>
                    {
                        {"Aws:SecretName", "secret-name-test" }
                    })
                    .Build(); ;

            _secretsManager.GetSecretValueAsync(
                Arg.Any<GetSecretValueRequest>(),
                Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(new GetSecretValueResponse
                {
                    SecretString = "{\"provider\":\"postgres\",\"connectionString\":\"Host=...\"}"
                }));

            var configLoader = new CompositeConfigLoader(configuration, _logger, _secretsManager);

            // Act
            var result = await configLoader.LoadAsync();

            Assert.That(result, Is.Not.Default);
            _logger.Received(1).Warning("Local DB config missing or invalid, falling back to AWS Secrets Manager.");
            _logger.Received(1).Information("Loaded DB config from AWS Secrets Manager.");
        }

        [Test]
        public async Task LoadAsync_ReturnsAwsConfig_WhenProviderIsMissing()
        {
            // Arrange
            _configuration["Database:Provider"] = string.Empty;
            _configuration["Aws:SecretName"] = "secret-name-test";

            _secretsManager.GetSecretValueAsync(
            Arg.Any<GetSecretValueRequest>(),
            Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(new GetSecretValueResponse
            {
                SecretString = "{\"provider\":\"postgres\",\"connectionString\":\"Host=...\"}"
            }));

            var configLoader = new CompositeConfigLoader(_configuration, _logger, _secretsManager);

            // Act
            var (Provider, ConnectionString) = await configLoader.LoadAsync();

            // Assert
            _logger.Received(1).Warning("Local DB config missing or invalid, falling back to AWS Secrets Manager.");
            _logger.Received(1).Information("Loaded DB config from AWS Secrets Manager.");
            using (Assert.EnterMultipleScope())
            {
                Assert.That(Provider, Is.Not.Default);
                Assert.That(ConnectionString, Is.Not.Default);
            }
        }

        [Test]
        public async Task LoadAsync_ReturnsAwsConfig_WhenConnectionStringIsMissing()
        {
            // Arrange
            _configuration["Database:ConnectionString"] = string.Empty;
            _configuration["Aws:SecretName"] = "secret-name-test";
            _secretsManager.GetSecretValueAsync(
                Arg.Any<GetSecretValueRequest>(),
                Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(new GetSecretValueResponse
                {
                    SecretString = "{\"provider\":\"postgres\",\"connectionString\":\"Host=...\"}"
                }));

            var configLoader = new CompositeConfigLoader(_configuration, _logger, _secretsManager);

            // Act
            var (Provider, ConnectionString) = await configLoader.LoadAsync();

            // Assert
            _logger.Received(1).Warning("Local DB config missing or invalid, falling back to AWS Secrets Manager.");
            _logger.Received(1).Information("Loaded DB config from AWS Secrets Manager.");
            using (Assert.EnterMultipleScope())
            {
                Assert.That(Provider, Is.Not.Default);
                Assert.That(ConnectionString, Is.Not.Default);
            }
        }

        [Test]
        public void Constructor_ThrowsArgumentNullException_WhenConfigurationIsNull()
        {
            // Act & Assert
            var ex = Assert.Throws<ArgumentNullException>(() => new CompositeConfigLoader(null!, _logger, _secretsManager));
            Assert.That(ex.ParamName, Is.EqualTo("configuration"));
        }

        [Test]
        public void Constructor_ThrowsArgumentNullException_WhenLoggerIsNull()
        {
            // Act & Assert
            var ex = Assert.Throws<ArgumentNullException>(() => new CompositeConfigLoader(_configuration, null!, _secretsManager));
            Assert.That(ex.ParamName, Is.EqualTo("logger"));
        }

        [Test]
        public void Constructor_DoesNotThrow_WhenDependenciesAreNotNull()
        {
            // Act & Assert
            Assert.DoesNotThrow(() => new CompositeConfigLoader(_configuration, _logger, _secretsManager));
        }

        [Test]
        public void LoadAsync_ThrowsInvalidOperationException_WhenBothLocalAndAwsConfigAreMissing()
        {
            // Arrange   
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection([])
                .Build();

            var configLoader = new CompositeConfigLoader(configuration, _logger, _secretsManager);

            // Act & Assert
            Assert.ThrowsAsync<InvalidOperationException>(configLoader.LoadAsync);
        }

        [Test]
        public void LoadAsync_ThrowsInvalidOperationException_WhenAwsSecretNameIsMissing()
        {
            // Arrange   
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    {"Database:Provider", string.Empty },
                    {"Database:ConnectionString", string.Empty }
                })
                .Build();
            var configLoader = new CompositeConfigLoader(configuration, _logger, _secretsManager);

            // Act & Assert
            var ex = Assert.ThrowsAsync<InvalidOperationException>(configLoader.LoadAsync);
            Assert.That(ex.Message, Is.EqualTo("Aws:SecretName is not configured."));
            _logger.Received(1).Error("Aws:SecretName is not configured.");
        }

        [Test]
        public void LoadAsync_ThrowsInvalidOperationException_WhenAwsSecretValueIsEmpty()
        {
            // Arrange   
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    {"Database:Provider", string.Empty },
                    {"Database:ConnectionString", string.Empty },
                    {"Aws:SecretName", "secret-name-test" }
                })
                .Build();

            _secretsManager.GetSecretValueAsync(
                Arg.Any<GetSecretValueRequest>(),
                Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(new GetSecretValueResponse
                {
                    SecretString = string.Empty
                }));
            var configLoader = new CompositeConfigLoader(configuration, _logger, _secretsManager);

            // Act & Assert
            var ex = Assert.ThrowsAsync<InvalidOperationException>(configLoader.LoadAsync);
            Assert.That(ex.Message, Is.EqualTo("AWS secret value is empty."));
            _logger.Received(1).Error("AWS secret value is empty.");
        }

        [Test]
        public async Task LoadAsync_ThrowsJsonException_WhenAwsSecretIsMalformed()
        {
            {
                // Arrange   
                var configuration = new ConfigurationBuilder()
                    .AddInMemoryCollection(new Dictionary<string, string?>
                    {
                    {"Database:Provider", string.Empty },
                    {"Database:ConnectionString", string.Empty },
                    {"Aws:SecretName", "secret-name-test" }
                    })
                    .Build();

                _secretsManager.GetSecretValueAsync(
                    Arg.Any<GetSecretValueRequest>(),
                    Arg.Any<CancellationToken>())
                    .Returns(Task.FromResult(new GetSecretValueResponse
                    {
                        SecretString = "{\"invalidJson"
                    }));
                var configLoader = new CompositeConfigLoader(configuration, _logger, _secretsManager);

                // Act
                Exception ex = null!;
                try
                {
                    await configLoader.LoadAsync();
                }
                catch (Exception caught)
                {
                    ex = caught;
                }

                // Assert
                Assert.That(ex, Is.Not.Null);
                Assert.That(ex, Is.InstanceOf<JsonException>());
            }
        }

        [Test]
        public void LoadAsync_ThrowsInvalidOperationException_WhenAwsSecretIsMissingRequiredFields()
        {
            // Arrange   
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    {"Database:Provider", string.Empty },
                    {"Database:ConnectionString", string.Empty },
                    {"Aws:SecretName", "secret-name-test" }
                })
                .Build();
            _secretsManager.GetSecretValueAsync(
                Arg.Any<GetSecretValueRequest>(),
                Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(new GetSecretValueResponse
                {
                    SecretString = "{\"wrongField\":\"value\"}"
                }));
            var configLoader = new CompositeConfigLoader(configuration, _logger, _secretsManager);

            // Act & Assert
            var ex = Assert.ThrowsAsync<InvalidOperationException>(configLoader.LoadAsync);
            Assert.That(ex.Message, Is.EqualTo("AWS secret is missing required fields."));
            _logger.Received(1).Error("AWS secret is missing required fields.");
        }

        [Test]
        public void LoadAsync_ThrowsException_WhenAwsCallFails()
        {
            // Arrange   
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    {"Database:Provider", string.Empty },
                    {"Database:ConnectionString", string.Empty },
                    {"Aws:SecretName", "secret-name-test" }
                })
                .Build();

            _secretsManager.GetSecretValueAsync(
                Arg.Any<GetSecretValueRequest>(),
                Arg.Any<CancellationToken>())
                .Throws(new Exception("AWS call failed"));

            var configLoader = new CompositeConfigLoader(configuration, _logger, _secretsManager);

            // Act & Assert
            var ex = Assert.ThrowsAsync<Exception>(configLoader.LoadAsync);
            Assert.That(ex.Message, Is.EqualTo("AWS call failed"));
        }

        [Test]
        public void LoadAsync_LogsError_WhenAwsCallFails()
        {
            // Arrange   
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    {"Database:Provider", string.Empty },
                    {"Database:ConnectionString", string.Empty },
                    {"Aws:SecretName", "secret-name-test" }
                })
                .Build();

            _secretsManager.GetSecretValueAsync(
                Arg.Any<GetSecretValueRequest>(),
                Arg.Any<CancellationToken>())
                .Throws(new Exception("AWS call failed"));

            var configLoader = new CompositeConfigLoader(configuration, _logger, _secretsManager);
            // Act

            try
            {
                var (Provider, ConnectionString) = configLoader.LoadAsync().GetAwaiter().GetResult();
            }
            catch (Exception)
            {
                // Ignore exception for this test
            }

            // Assert
            _logger.Received(1).Error(Arg.Is<string>(s => s.Contains("AWS call failed")));
        }
    }
}
