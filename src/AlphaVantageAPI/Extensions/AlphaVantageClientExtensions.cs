using System.Text.Json;
using Tudormobile.AlphaVantage.Entities;

namespace Tudormobile.AlphaVantage.Extensions;

/// <summary>
/// Provides extension methods for working with instances of <see cref="IAlphaVantageClient"/>.
/// </summary>
/// <remarks>This class contains static methods that extend the functionality of <see cref="IAlphaVantageClient"/>
/// implementations, enabling fluent configuration and builder patterns. These extensions are intended to simplify
/// client setup and integration scenarios.</remarks>
public static class AlphaVantageClientExtensions
{
    extension(IAlphaVantageClient)
    {
        /// <summary>
        /// Creates a new builder for configuring and constructing an instance of an Alpha Vantage client.
        /// </summary>
        /// <returns>An <see cref="IBuilder{IAlphaVantageClient}"/> that can be used to configure and build an <see
        /// cref="IAlphaVantageClient"/> instance.</returns>
        public static IAlphaVantageClientBuilder GetBuilder()
        {
            return new AlphaVantageClientBuilder();
        }
    }

    /// <summary>
    /// Retrieves daily time series data for the specified stock symbol.
    /// </summary>
    /// <param name="client">The Alpha Vantage client instance.</param>
    /// <param name="symbol">The stock symbol to retrieve daily time series data for.</param>
    /// <returns>A <see cref="Task{JsonDocument}"/> containing the daily time series data.</returns>
    public static async Task<JsonDocument> TimeSeriesDailyAsync(this IAlphaVantageClient client, string symbol)
        => await client.GetJsonDocumentAsync(AlphaVantageFunction.TIME_SERIES_DAILY, symbol);

    /// <summary>
    /// Retrieves monthly time series data for the specified stock symbol.
    /// </summary>
    /// <param name="client">The Alpha Vantage client instance.</param>
    /// <param name="symbol">The stock symbol to retrieve monthly time series data for.</param>
    /// <param name="adjusted">If true, returns adjusted monthly data; otherwise, returns unadjusted monthly data. Default is false.</param>
    /// <returns>A <see cref="Task{JsonDocument}"/> containing the monthly time series data.</returns>
    public static async Task<JsonDocument> TimeSeriesMonthlyAsync(this IAlphaVantageClient client, string symbol, bool adjusted = false)
        => await client.GetJsonDocumentAsync(adjusted ? AlphaVantageFunction.TIME_SERIES_MONTHLY_ADJUSTED : AlphaVantageFunction.TIME_SERIES_MONTHLY, symbol);

    /// <summary>
    /// Retrieves weekly time series data for the specified stock symbol.
    /// </summary>
    /// <param name="client">The Alpha Vantage client instance.</param>
    /// <param name="symbol">The stock symbol to retrieve weekly time series data for.</param>
    /// <param name="adjusted">If true, returns adjusted weekly data; otherwise, returns unadjusted weekly data. Default is false.</param>
    /// <returns>A <see cref="Task{JsonDocument}"/> containing the weekly time series data.</returns>
    public static async Task<JsonDocument> TimeSeriesWeeklyAsync(this IAlphaVantageClient client, string symbol, bool adjusted = false)
        => await client.GetJsonDocumentAsync(adjusted ? AlphaVantageFunction.TIME_SERIES_WEEKLY_ADJUSTED : AlphaVantageFunction.TIME_SERIES_WEEKLY, symbol);

    /// <summary>
    /// Retrieves the latest global quote information for the specified stock symbol.
    /// </summary>
    /// <param name="client">The Alpha Vantage client instance.</param>
    /// <param name="symbol">The stock symbol to retrieve the global quote for.</param>
    /// <returns>A <see cref="Task{JsonDocument}"/> containing the global quote data including price, volume, and trading information.</returns>
    public static async Task<JsonDocument> GlobalQuoteAsync(this IAlphaVantageClient client, string symbol)
        => await client.GetJsonDocumentAsync(AlphaVantageFunction.GLOBAL_QUOTE, symbol);

    /// <summary>
    /// Searches for stock symbols matching the specified keywords.
    /// </summary>
    /// <param name="client">The Alpha Vantage client instance.</param>
    /// <param name="symbol">The search keywords to find matching stock symbols.</param>
    /// <returns>A <see cref="Task{JsonDocument}"/> containing the symbol search results with matching stock symbols and company information.</returns>
    public static async Task<JsonDocument> SymbolSearchAsync(this IAlphaVantageClient client, string symbol)
        => await client.GetJsonDocumentAsync(AlphaVantageFunction.SYMBOL_SEARCH, symbol);

    /// <summary>
    /// Retrieves and parses the global quote data for the specified stock symbol into a strongly-typed response.
    /// </summary>
    /// <param name="client">The Alpha Vantage client instance.</param>
    /// <param name="symbol">The stock symbol to retrieve the global quote for.</param>
    /// <returns>A <see cref="Task{AlphaVantageResponse}"/> containing either the parsed <see cref="GlobalQuote"/> data or error information.</returns>
    /// <remarks>
    /// This method calls the Alpha Vantage Global Quote API and parses the JSON response into a strongly-typed
    /// <see cref="GlobalQuote"/> object. If the API returns an error or the data cannot be parsed, the response
    /// will contain an error message in the <see cref="AlphaVantageResponse{T}.ErrorMessage"/> property.
    /// </remarks>
    public static async Task<AlphaVantageResponse<GlobalQuote>> GlobalQuote(this IAlphaVantageClient client, string symbol)
    {
        var jsonDocument = await client.GlobalQuoteAsync(symbol);
        var result = GlobalQuoteBuilder.FromDocument(jsonDocument, symbol);
        return new AlphaVantageResponse<GlobalQuote>
        {
            Result = result,
            ErrorMessage = result == null ? FindErrorMessage(jsonDocument.RootElement, "Global Quote data not found.") : null
        };
    }

    /// <summary>
    /// Retrieves global quote data for multiple stock symbols concurrently.
    /// </summary>
    /// <param name="client">The Alpha Vantage client instance.</param>
    /// <param name="symbols">An enumerable collection of stock symbols to retrieve global quotes for.</param>
    /// <returns>A <see cref="Task{IDictionary}"/> containing a dictionary where keys are stock symbols and values are the corresponding <see cref="AlphaVantageResponse{GlobalQuote}"/> data.</returns>
    /// <remarks>
    /// This method executes multiple API calls concurrently to fetch global quote data for all provided symbols.
    /// Each symbol's result is returned as a separate <see cref="AlphaVantageResponse{GlobalQuote}"/> in the dictionary,
    /// allowing individual error handling per symbol. Failed requests will have error information in their respective
    /// <see cref="AlphaVantageResponse{T}.ErrorMessage"/> property.
    /// </remarks>
    public static async Task<IDictionary<string, AlphaVantageResponse<GlobalQuote>>> GlobalQuotes(this IAlphaVantageClient client, IEnumerable<string> symbols)
    {
        var symbolsList = symbols.ToList();
        var tasks = symbolsList.Select(symbol => client.GlobalQuote(symbol));
        var results = await Task.WhenAll(tasks);
        return symbolsList.Zip(results, (symbol, result) => new { symbol, result })
            .ToDictionary(x => x.symbol, x => x.result);
    }

    /// <summary>
    /// Retrieves and parses daily time series data for the specified stock symbol into a strongly-typed response.
    /// </summary>
    /// <param name="client">The Alpha Vantage client instance.</param>
    /// <param name="symbol">The stock symbol to retrieve daily time series data for.</param>
    /// <returns>A <see cref="Task{AlphaVantageResponse}"/> containing either the parsed <see cref="TimeSeries"/> data with daily interval or error information.</returns>
    /// <remarks>
    /// This method calls the Alpha Vantage Daily Time Series API and parses the JSON response into a strongly-typed
    /// <see cref="TimeSeries"/> object with <see cref="TimeSeries.TimeSeriesInterval.Daily"/> interval. If the API
    /// returns an error or the data cannot be parsed, the response will contain an error message in the
    /// <see cref="AlphaVantageResponse{T}.ErrorMessage"/> property.
    /// </remarks>
    public static async Task<AlphaVantageResponse<TimeSeries>> Daily(this IAlphaVantageClient client, string symbol)
        => TimeSeriesResult(await client.TimeSeriesDailyAsync(symbol), symbol, TimeSeries.TimeSeriesInterval.Daily);

    /// <summary>
    /// Retrieves and parses weekly time series data for the specified stock symbol into a strongly-typed response.
    /// </summary>
    /// <param name="client">The Alpha Vantage client instance.</param>
    /// <param name="symbol">The stock symbol to retrieve weekly time series data for.</param>
    /// <param name="adjusted">If true, returns adjusted weekly data that accounts for stock splits and dividends; otherwise, returns unadjusted weekly data. Default is false.</param>
    /// <returns>A <see cref="Task{AlphaVantageResponse}"/> containing either the parsed <see cref="TimeSeries"/> data with weekly interval or error information.</returns>
    /// <remarks>
    /// This method calls the Alpha Vantage Weekly Time Series API and parses the JSON response into a strongly-typed
    /// <see cref="TimeSeries"/> object with <see cref="TimeSeries.TimeSeriesInterval.Weekly"/> interval. If the API
    /// returns an error or the data cannot be parsed, the response will contain an error message in the
    /// <see cref="AlphaVantageResponse{T}.ErrorMessage"/> property.
    /// </remarks>
    public static async Task<AlphaVantageResponse<TimeSeries>> Weekly(this IAlphaVantageClient client, string symbol, bool adjusted = false)
        => TimeSeriesResult(await client.TimeSeriesWeeklyAsync(symbol, adjusted), symbol, TimeSeries.TimeSeriesInterval.Weekly);

    /// <summary>
    /// Retrieves and parses monthly time series data for the specified stock symbol into a strongly-typed response.
    /// </summary>
    /// <param name="client">The Alpha Vantage client instance.</param>
    /// <param name="symbol">The stock symbol to retrieve monthly time series data for.</param>
    /// <param name="adjusted">If true, returns adjusted monthly data that accounts for stock splits and dividends; otherwise, returns unadjusted monthly data. Default is false.</param>
    /// <returns>A <see cref="Task{AlphaVantageResponse}"/> containing either the parsed <see cref="TimeSeries"/> data with monthly interval or error information.</returns>
    /// <remarks>
    /// This method calls the Alpha Vantage Monthly Time Series API and parses the JSON response into a strongly-typed
    /// <see cref="TimeSeries"/> object with <see cref="TimeSeries.TimeSeriesInterval.Monthly"/> interval. If the API
    /// returns an error or the data cannot be parsed, the response will contain an error message in the
    /// <see cref="AlphaVantageResponse{T}.ErrorMessage"/> property.
    /// </remarks>
    public static async Task<AlphaVantageResponse<TimeSeries>> Monthly(this IAlphaVantageClient client, string symbol, bool adjusted = false)
        => TimeSeriesResult(await client.TimeSeriesMonthlyAsync(symbol, adjusted), symbol, TimeSeries.TimeSeriesInterval.Monthly);

    private static AlphaVantageResponse<TimeSeries> TimeSeriesResult(JsonDocument jsonDocument, string symbol, TimeSeries.TimeSeriesInterval interval)
    {
        var result = TimeSeriesBuilder.FromDocument(jsonDocument, symbol, interval);
        return new AlphaVantageResponse<TimeSeries>
        {
            Result = result,
            ErrorMessage = result == null ? FindErrorMessage(jsonDocument.RootElement, "Time series data not found.") : null
        };
    }

    private static string? FindErrorMessage(JsonElement root, string defaultMessage)
    {
        if (root.TryGetProperty("Information", out JsonElement informationElement))
        {
            return informationElement.GetString();
        }
        if (root.TryGetProperty("Error Message", out JsonElement errorElement))
        {
            return errorElement.GetString();
        }
        return defaultMessage;
    }
}
