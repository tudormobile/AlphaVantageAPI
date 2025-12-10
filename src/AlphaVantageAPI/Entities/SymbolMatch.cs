namespace Tudormobile.AlphaVantage.Entities;

/// <summary>
/// Represents a matching symbol result from a symbol search query.
/// </summary>
/// <remarks>
/// This class contains detailed information about a financial instrument that matches the search criteria,
/// including trading hours, location, and match relevance score.
/// </remarks>
public class SymbolMatch
{
    /// <summary>
    /// Gets the stock symbol or ticker for this match.
    /// </summary>
    /// <value>The stock symbol (e.g., "AAPL", "MSFT") that uniquely identifies the security.</value>
    public string Symbol { get; init; } = String.Empty;

    /// <summary>
    /// Gets the full name of the company or security.
    /// </summary>
    /// <value>The complete name of the financial instrument or company.</value>
    public string Name { get; init; } = String.Empty;

    /// <summary>
    /// Gets the type of financial instrument.
    /// </summary>
    /// <value>The instrument type (e.g., "Equity", "ETF", "Mutual Fund").</value>
    public MatchTypes MatchType { get; init; }

    /// <summary>
    /// Gets the geographic region where the security trades.
    /// </summary>
    /// <value>The region or country code (e.g., "United States", "United Kingdom").</value>
    public Regions Region { get; init; }

    /// <summary>
    /// Gets the name of the region associated with this instance.
    /// </summary>
    public string RegionName { get; init; } = String.Empty;

    /// <summary>
    /// Gets the market opening time for this security.
    /// </summary>
    /// <value>The time when the market opens, expressed in local time.</value>
    /// <remarks>This value is always in the local market timezone.</remarks>
    public TimeOnly MarketOpen { get; init; }

    /// <summary>
    /// Gets the market closing time for this security.
    /// </summary>
    /// <value>The time when the market closes, expressed in local time.</value>
    /// <remarks>This value is always in the local market timezone.</remarks>
    public TimeOnly MarketClose { get; init; }

    /// <summary>
    /// Gets the currency in which the security is traded.
    /// </summary>
    /// <value>The ISO currency code (e.g., "USD", "EUR", "GBP").</value>
    public string Currency { get; init; } = String.Empty;

    /// <summary>
    /// Gets the relevance score indicating how well this result matches the search query.
    /// </summary>
    /// <value>A score between 0.0 and 1.0, where higher values indicate better matches.</value>
    /// <remarks>A score of 1.0 represents a perfect match, while lower scores indicate partial matches.</remarks>
    public double MatchScore { get; init; }

    /// <summary>
    /// Defines the types of financial instruments that can be matched in symbol search results.
    /// </summary>
    public enum MatchTypes
    {
        /// <summary>
        /// Matches any financial instrument type without filtering.
        /// </summary>
        Any = -1,

        /// <summary>
        /// Represents an unknown or unrecognized financial instrument type.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// Represents equity securities such as common stock or preferred stock.
        /// </summary>
        Equity,

        /// <summary>
        /// Represents Exchange-Traded Funds (ETFs).
        /// </summary>
        ETF,

        /// <summary>
        /// Represents mutual fund investments.
        /// </summary>
        MutualFund,

        /// <summary>
        /// Represents market indices that track the performance of a group of securities.
        /// </summary>
        Index,

        /// <summary>
        /// Represents commodity investments such as gold, oil, or agricultural products.
        /// </summary>
        Commodity,

        /// <summary>
        /// Represents foreign exchange (forex) currency pairs.
        /// </summary>
        Currency,

        /// <summary>
        /// Represents digital or cryptocurrency assets.
        /// </summary>
        Cryptocurrency,

        /// <summary>
        /// Represents fixed-income debt securities.
        /// </summary>
        Bond
    }

    /// <summary>
    /// Defines the geographic regions where financial instruments can be traded.
    /// </summary>
    public enum Regions
    {
        /// <summary>
        /// Matches any geographic region without filtering.
        /// </summary>
        Any = -1,

        /// <summary>
        /// Represents an unknown or unrecognized region.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// Represents the United States market.
        /// </summary>
        US,

        /// <summary>
        /// Represents the United Kingdom market.
        /// </summary>
        UK,

        /// <summary>
        /// Represents the Frankfurt (Germany) market.
        /// </summary>
        FFM,
    }
}
