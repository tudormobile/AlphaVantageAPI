using Microsoft.Extensions.DependencyInjection;
namespace Tudormobile.AlphaVantage.Extensions;

/// <summary>
/// Provides extension methods for registering Alpha Vantage client services with an <see cref="IServiceCollection"/>.
/// </summary>
/// <remarks>Use these extension methods to add and configure Alpha Vantage API clients in ASP.NET Core dependency
/// injection containers. This enables applications to access financial data from Alpha Vantage using strongly-typed
/// services.</remarks>
public static class AlphaVantageServiceCollectionExtensions
{
    /// <summary>
    /// Adds and configures an Alpha Vantage API client for dependency injection, enabling typed access to Alpha Vantage
    /// financial data services within the application.
    /// </summary>
    /// <remarks>Registers the Alpha Vantage client as a typed HTTP client with a handler lifetime of five
    /// minutes. This method should be called during application startup to ensure the client is available for injection
    /// throughout the application's lifetime.</remarks>
    /// <param name="services">The service collection to which the Alpha Vantage client and its dependencies will be added.</param>
    /// <param name="configure">Configuration options, such as the ApiKey.</param>
    /// <returns>The same service collection instance, allowing for method chaining.</returns>
    public static IServiceCollection AddAlphaVantageClient(
        this IServiceCollection services,
    Action<AlphaVantageOptions> configure)
    {
        services.Configure(configure);
        services.AddHttpClient<IAlphaVantageClient, AlphaVantageClient>();
        return services;
    }
}