using System.Text.Json;

namespace Tudormobile.AlphaVantage;

/// <summary>
/// Defines a contract for accessing financial data and services provided by the Alpha Vantage API.
/// </summary>
/// <remarks>Implementations of this interface enable applications to retrieve market data, such as stock prices,
/// currency exchange rates, and technical indicators, from the Alpha Vantage platform. Methods and properties for
/// specific data retrieval are defined in derived interfaces or implementations.</remarks>
public interface IAlphaVantageClient
{
    /// <summary>
    /// Asynchronously retrieves the raw JSON response for the specified Alpha Vantage function and symbol.
    /// </summary>
    /// <param name="function">The Alpha Vantage API function to query. Determines the type of financial data returned.</param>
    /// <param name="symbol">The symbol representing the financial instrument to query. Must be a valid Alpha Vantage symbol; cannot be null
    /// or empty.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the raw JSON string returned by the
    /// Alpha Vantage API.</returns>
    Task<string> GetJsonStringAsync(AlphaVantageFunction function, string symbol, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves the raw JSON response for the specified Alpha Vantage function and query parameters.
    /// </summary>
    /// <param name="function">The Alpha Vantage API function to query. Determines the type of financial data returned.</param>
    /// <param name="queryParameters">A dictionary of query parameters to include in the API request. Keys and values must be valid Alpha Vantage parameters.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the raw JSON string returned by the
    /// Alpha Vantage API.</returns>
    Task<string> GetJsonStringAsync(AlphaVantageFunction function, IDictionary<string, string> queryParameters, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves a JSON document containing data from the Alpha Vantage API for the specified function
    /// and symbol.
    /// </summary>
    /// <param name="function">The Alpha Vantage function to query, which determines the type of financial data to retrieve.</param>
    /// <param name="symbol">The symbol representing the financial instrument (such as a stock ticker) for which data is requested. Cannot be
    /// null or empty.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="JsonDocument"/> with
    /// the requested data from Alpha Vantage.</returns>
    Task<JsonDocument> GetJsonDocumentAsync(AlphaVantageFunction function, string symbol, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves a JSON document containing data from the Alpha Vantage API for the specified function
    /// and query parameters.
    /// </summary>
    /// <param name="function">The Alpha Vantage function to query, which determines the type of financial data to retrieve.</param>
    /// <param name="queryParameters">A dictionary of query parameters to include in the API request. Keys and values must be valid Alpha Vantage parameters.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="JsonDocument"/> with
    /// the requested data from Alpha Vantage.</returns>
    Task<JsonDocument> GetJsonDocumentAsync(AlphaVantageFunction function, IDictionary<string, string> queryParameters, CancellationToken cancellationToken = default);
}
