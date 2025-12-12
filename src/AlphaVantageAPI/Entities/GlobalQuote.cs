namespace Tudormobile.AlphaVantage.Entities;

/// <summary>
/// Represents a global quote for a stock symbol containing current market data and trading information.
/// This class encapsulates real-time or latest available trading data for a security.
/// </summary>
public class GlobalQuote : IEntity
{
    /// <summary>
    /// Gets the stock symbol or ticker for this quote.
    /// </summary>
    /// <value>The stock symbol (e.g., "AAPL", "MSFT") that uniquely identifies the security.</value>
    public string Symbol { get; init; } = String.Empty;

    /// <summary>
    /// Gets the opening price of the security for the latest trading day.
    /// </summary>
    /// <value>The price at which the security opened trading, expressed as a decimal value.</value>
    public decimal Open { get; init; }

    /// <summary>
    /// Gets the highest price reached by the security during the latest trading day.
    /// </summary>
    /// <value>The highest trading price for the day, expressed as a decimal value.</value>
    public decimal High { get; init; }

    /// <summary>
    /// Gets the lowest price reached by the security during the latest trading day.
    /// </summary>
    /// <value>The lowest trading price for the day, expressed as a decimal value.</value>
    public decimal Low { get; init; }

    /// <summary>
    /// Gets the current or latest trading price of the security.
    /// </summary>
    /// <value>The most recent trading price, expressed as a decimal value.</value>
    public decimal Price { get; init; }

    /// <summary>
    /// Gets the total number of shares traded during the latest trading day.
    /// </summary>
    /// <value>The trading volume as a long integer representing the number of shares.</value>
    public long Volume { get; init; }

    /// <summary>
    /// Gets the date of the latest trading day for this quote.
    /// </summary>
    /// <value>The date when this trading data was recorded.</value>
    public DateOnly LatestTradingDay { get; init; }

    /// <summary>
    /// Gets the closing price from the previous trading day.
    /// </summary>
    /// <value>The previous day's closing price, expressed as a decimal value.</value>
    public decimal PreviousClose { get; init; }

    /// <summary>
    /// Gets the absolute change in price from the previous close to the current price.
    /// </summary>
    /// <value>The price change as a decimal value (positive for gains, negative for losses).</value>
    public decimal Change { get; init; }

    /// <summary>
    /// Gets the percentage change in price from the previous close to the current price.
    /// </summary>
    /// <value>A formatted string representing the percentage change with two decimal places and a '%' symbol.</value>
    /// <remarks>
    /// The percentage is calculated as (Change / PreviousClose * 100). This is strictly
    /// not the same value returned by the API, which does have different formatting.
    /// Returns "0.00%" if PreviousClose is zero to avoid division by zero.
    /// </remarks>
    public string ChangePercent => $"{(PreviousClose != 0 ? (Change / PreviousClose * 100) : 0):F3}%";
}
