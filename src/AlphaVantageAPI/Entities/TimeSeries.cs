namespace Tudormobile.AlphaVantage.Entities;

/// <summary>
/// Represents time series data for a financial instrument, containing historical price and volume information
/// organized by date and time interval.
/// </summary>
/// <remarks>
/// This class provides a structured representation of time series data from Alpha Vantage, supporting various
/// time intervals from intraday (1-minute) to long-term (monthly) data. The data is stored as a dictionary
/// keyed by date for efficient lookup and retrieval.
/// </remarks>
public class TimeSeries
{
    /// <summary>
    /// Gets the stock symbol or ticker for this quote.
    /// </summary>
    /// <value>The stock symbol (e.g., "AAPL", "MSFT") that uniquely identifies the security.</value>
    public string Symbol { get; init; } = String.Empty;

    /// <summary>
    /// Gets the date associated with this time series data point.
    /// </summary>
    /// <value>The date and time of the time series entry.</value>
    public DateOnly LastUpdated { get; init; }

    /// <summary>
    /// Gets the time interval for this time series data.
    /// </summary>
    /// <value>A <see cref="TimeSeriesInterval"/> value indicating the granularity of the time series data
    /// (e.g., daily, weekly, intraday intervals).</value>
    public TimeSeriesInterval Interval { get; init; }

    /// <summary>
    /// Gets the dictionary containing time series data points indexed by date.
    /// </summary>
    /// <value>A dictionary where each key is a <see cref="DateOnly"/> representing the data point's timestamp,
    /// and each value is a <see cref="TimeSeriesData"/> object containing the corresponding price and volume information.</value>
    public IDictionary<DateOnly, TimeSeriesData> Data { get; init; } = new Dictionary<DateOnly, TimeSeriesData>();

    /// <summary>
    /// Defines the supported time intervals for time series data retrieval.
    /// </summary>
    public enum TimeSeriesInterval
    {
        /// <summary>
        /// Represents 1-minute interval intraday data.
        /// </summary>
        OneMin,

        /// <summary>
        /// Represents 5-minute interval intraday data.
        /// </summary>
        FiveMin,

        /// <summary>
        /// Represents 15-minute interval intraday data.
        /// </summary>
        FifteenMin,

        /// <summary>
        /// Represents 30-minute interval intraday data.
        /// </summary>
        ThirtyMin,

        /// <summary>
        /// Represents 60-minute (1-hour) interval intraday data.
        /// </summary>
        SixtyMin,

        /// <summary>
        /// Represents daily time series data with one data point per trading day.
        /// </summary>
        Daily,

        /// <summary>
        /// Represents weekly time series data with one data point per week.
        /// </summary>
        Weekly,

        /// <summary>
        /// Represents monthly time series data with one data point per month.
        /// </summary>
        Monthly
    }
}
