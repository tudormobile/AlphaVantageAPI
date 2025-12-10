namespace Tudormobile.AlphaVantage.Entities;

/// <summary>
/// Represents price and volume data for a single time period in a financial time series.
/// </summary>
/// <remarks>
/// This class contains the standard OHLCV (Open, High, Low, Close, Volume) data points commonly
/// used in financial analysis and charting. All price values are represented as decimals for precision,
/// while volume is represented as a long integer to accommodate large trading volumes.
/// </remarks>
public class TimeSeriesData
{
    /// <summary>
    /// Gets the opening price for the time period.
    /// </summary>
    /// <value>The price at which the security first traded during the time period.</value>
    public decimal Open { get; init; }

    /// <summary>
    /// Gets the highest price reached during the time period.
    /// </summary>
    /// <value>The maximum price at which the security traded during the time period.</value>
    public decimal High { get; init; }

    /// <summary>
    /// Gets the lowest price reached during the time period.
    /// </summary>
    /// <value>The minimum price at which the security traded during the time period.</value>
    public decimal Low { get; init; }

    /// <summary>
    /// Gets the closing price for the time period.
    /// </summary>
    /// <value>The final price at which the security traded during the time period.</value>
    public decimal Close { get; init; }

    /// <summary>
    /// Gets the total trading volume for the time period.
    /// </summary>
    /// <value>The total number of shares or contracts traded during the time period.</value>
    public long Volume { get; init; }
}
