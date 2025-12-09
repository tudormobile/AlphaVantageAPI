namespace Tudormobile.AlphaVantage;

/// <summary>
/// Specifies the available Alpha Vantage API functions for retrieving financial time series and market data.
/// </summary>
/// <remarks>Use this enumeration to select the type of data to request from the Alpha Vantage API, such as daily,
/// weekly, or monthly time series, adjusted data, global quotes, or symbol search results. The selected value
/// determines the structure and content of the data returned by the API.</remarks>
public enum AlphaVantageFunction
{
    /// <summary>
    /// Represents the daily time series function for retrieving daily historical data from the API.
    /// </summary>
    TIME_SERIES_DAILY,

    /// <summary>
    /// Represents the weekly time series data interval for a financial or data API.
    /// </summary>
    TIME_SERIES_WEEKLY,

    /// <summary>
    /// Represents the weekly adjusted time series data interval for financial data queries.
    /// </summary>
    /// <remarks>Use this value to request weekly time series data that includes adjustments such as splits
    /// and dividend corrections. This is typically used with APIs that support multiple time intervals for historical
    /// financial data.</remarks>
    TIME_SERIES_WEEKLY_ADJUSTED,

    /// <summary>
    /// Represents monthly time series data, where each data point corresponds to a single month.
    /// </summary>
    TIME_SERIES_MONTHLY,

    /// <summary>
    /// Represents the monthly adjusted time series data type for financial data queries.
    /// </summary>
    TIME_SERIES_MONTHLY_ADJUSTED,

    /// <summary>
    /// Represents the global quote endpoint or identifier, typically used to retrieve real-time market data for a
    /// specific security.
    /// </summary>
    /// <remarks>This value is commonly used in financial APIs to request the latest price and related
    /// information for a single stock or asset. The exact usage may depend on the context in which this identifier is
    /// applied.</remarks>
    GLOBAL_QUOTE,

    /// <summary>
    /// Specifies the available options for symbol search operations.
    /// </summary>
    SYMBOL_SEARCH
}

