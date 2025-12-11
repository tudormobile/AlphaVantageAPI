using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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
    /// Adds AlphaVantage client services to the specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
    /// <param name="configure">The action used to configure the <see cref="IAlphaVantageClientBuilder"/>.</param>
    /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
    public static IServiceCollection AddAlphaVantageClient(
        this IServiceCollection services,
        Action<IAlphaVantageClientBuilder> configure)
    {
        if (services == null)
        {
            throw new ArgumentNullException(nameof(services));
        }
        if (configure == null)
        {
            throw new ArgumentNullException(nameof(configure));
        }

        // Create a builder to capture configuration
        var builder = new AlphaVantageClientBuilder();
        configure(builder);

        // Register HttpClient for AlphaVantageClient
        services.AddHttpClient(nameof(AlphaVantageClient));

        // Register IAlphaVantageClient with a factory that provides the API key
        services.AddScoped<IAlphaVantageClient>(sp =>
        {
            var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
            var logger = sp.GetRequiredService<ILogger<AlphaVantageClient>>();

            // Use the captured API key from the builder
            return new AlphaVantageClient(httpClientFactory, builder.ApiKey, logger);
        });

        return services;
    }
}