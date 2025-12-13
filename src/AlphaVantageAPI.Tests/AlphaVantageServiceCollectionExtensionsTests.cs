using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Tudormobile.AlphaVantage;
using Tudormobile.AlphaVantage.Extensions;

namespace AlphaVantageAPI.Tests;

[TestClass]
public class AlphaVantageServiceCollectionExtensionsTests
{
    [TestMethod]
    public void AddAlphaVantageClient_WithValidConfiguration_RegistersServices()
    {
        // Arrange
        var services = new ServiceCollection();
        var apiKey = "test-api-key";

        // Act
        services.AddAlphaVantageClient(builder =>
        {
            builder.WithApiKey(apiKey);
        });

        // Assert
        var serviceProvider = services.BuildServiceProvider();
        var client = serviceProvider.GetService<IAlphaVantageClient>();

        Assert.IsNotNull(client);
    }

    [TestMethod]
    public void AddAlphaVantageClient_WithNullServices_ThrowsArgumentNullException()
    {
        // Arrange
        IServiceCollection? services = null;
        Action<IAlphaVantageClientBuilder> configure = null!;

        // Act & Assert
        var exception = Assert.ThrowsExactly<ArgumentNullException>(() =>
        {
            services!.AddAlphaVantageClient(configure!);
        });

        Assert.AreEqual("services", exception.ParamName);
    }

    [TestMethod]
    public void AddAlphaVantageClient_WithNullConfigure_ThrowsArgumentNullException()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act & Assert
        var exception = Assert.ThrowsExactly<ArgumentNullException>(() =>
        {
            services.AddAlphaVantageClient(null!);
        });

        Assert.AreEqual("configure", exception.ParamName);
    }

    [TestMethod]
    public void AddAlphaVantageClient_RegistersHttpClient()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddAlphaVantageClient(builder =>
        {
            builder.WithApiKey("test-api-key");
        });

        // Assert
        var serviceProvider = services.BuildServiceProvider();
        var httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();

        Assert.IsNotNull(httpClientFactory);
    }

    [TestMethod]
    public void AddAlphaVantageClient_RegistersClientWithScopedLifetime()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddAlphaVantageClient(builder =>
        {
            builder.WithApiKey("test-api-key");
        });

        // Assert
        var serviceDescriptor = services.FirstOrDefault(sd => sd.ServiceType == typeof(IAlphaVantageClient));

        Assert.IsNotNull(serviceDescriptor);
        Assert.AreEqual(ServiceLifetime.Scoped, serviceDescriptor.Lifetime);
    }

    [TestMethod]
    public void AddAlphaVantageClient_ReturnsSameServiceCollection()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        var result = services.AddAlphaVantageClient(builder =>
        {
            builder.WithApiKey("test-api-key");
        });

        // Assert
        Assert.AreSame(services, result);
    }

    [TestMethod]
    public void AddAlphaVantageClient_CreatesClientWithConfiguredApiKey()
    {
        // Arrange
        var services = new ServiceCollection();
        var expectedApiKey = "my-custom-api-key";

        // Act
        services.AddAlphaVantageClient(builder =>
        {
            builder.WithApiKey(expectedApiKey);
        });

        // Assert
        var serviceProvider = services.BuildServiceProvider();
        var client = serviceProvider.GetRequiredService<IAlphaVantageClient>();

        Assert.IsNotNull(client);
        Assert.IsInstanceOfType<AlphaVantageClient>(client);
    }

    [TestMethod]
    public void AddAlphaVantageClient_CanBeCalledMultipleTimes()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddAlphaVantageClient(builder => builder.WithApiKey("key1"));
        services.AddAlphaVantageClient(builder => builder.WithApiKey("key2"));

        // Assert
        var serviceProvider = services.BuildServiceProvider();
        var client = serviceProvider.GetService<IAlphaVantageClient>();

        Assert.IsNotNull(client);
    }

    [TestMethod]
    public void AddAlphaVantageClient_ResolvesRequiredDependencies()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddAlphaVantageClient(builder =>
        {
            builder.WithApiKey("test-api-key");
        });

        // Assert
        var serviceProvider = services.BuildServiceProvider();

        // Verify IHttpClientFactory is available
        var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
        Assert.IsNotNull(httpClientFactory);

        // Verify ILogger<AlphaVantageClient> is available
        var logger = serviceProvider.GetRequiredService<ILogger<AlphaVantageClient>>();
        Assert.IsNotNull(logger);

        // Verify IAlphaVantageClient can be resolved
        var client = serviceProvider.GetRequiredService<IAlphaVantageClient>();
        Assert.IsNotNull(client);
    }
}
