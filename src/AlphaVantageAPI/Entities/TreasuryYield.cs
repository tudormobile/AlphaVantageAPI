namespace Tudormobile.AlphaVantage.Entities;

/// <summary>
/// Represents U.S. Treasury yield data for a specific maturity period.
/// </summary>
/// <remarks>
/// This class contains historical yield data for U.S. Treasury securities, which are government debt instruments
/// used as benchmarks for risk-free rates in financial markets. Treasury yields are fundamental indicators of
/// economic conditions and interest rate expectations.
/// </remarks>
public class TreasuryYield : IEntity
{
    /// <summary>
    /// Gets or sets the full name of the Treasury instrument.
    /// </summary>
    /// <value>A descriptive name such as "10-Year Treasury Constant Maturity Rate" or "3-Month Treasury Bill".</value>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the time interval between data points in the yield series.
    /// </summary>
    /// <value>The frequency of yield observations, such as daily, weekly, or monthly.</value>
    public TreasuryYieldInterval Interval { get; set; }

    /// <summary>
    /// Gets or sets the maturity period of the Treasury security.
    /// </summary>
    /// <value>The time to maturity, ranging from 3 months to 30 years.</value>
    public TreasuryYieldMaturity Maturity { get; set; }

    /// <summary>
    /// Gets or sets the unit of measurement for the yield values.
    /// </summary>
    /// <value>Typically "percent", indicating that yield values are expressed as percentage rates (e.g., 4.09 means 4.09%).</value>
    public string Unit { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the collection of historical yield data points.
    /// </summary>
    /// <value>A list of <see cref="TreasuryYieldData"/> objects, each representing the yield on a specific date.
    /// Data is typically ordered chronologically, with the most recent data first.</value>
    public List<TreasuryYieldData> Data { get; set; } = [];
}

/// <summary>
/// Represents a single yield observation for a Treasury security on a specific date.
/// </summary>
/// <remarks>
/// Each data point captures the constant maturity yield for the Treasury security, which is the yield
/// on a security adjusted to a constant maturity equivalent to ensure comparability over time.
/// </remarks>
public class TreasuryYieldData : IEntity
{
    /// <summary>
    /// Gets or sets the date of the yield observation.
    /// </summary>
    /// <value>The date on which the Treasury yield was recorded.</value>
    public DateOnly Date { get; set; }

    /// <summary>
    /// Gets or sets the yield value as a percentage.
    /// </summary>
    /// <value>The yield rate expressed as a decimal representing a percentage (e.g., 4.09 represents 4.09%).
    /// This is the constant maturity rate published by the Federal Reserve.</value>
    public decimal Value { get; set; }
}

/// <summary>
/// Specifies the time interval between Treasury yield data points.
/// </summary>
public enum TreasuryYieldInterval
{
    /// <summary>
    /// Monthly yield observations, typically published on the first business day of each month.
    /// </summary>
    Monthly,

    /// <summary>
    /// Weekly yield observations, typically published once per week.
    /// </summary>
    Weekly,

    /// <summary>
    /// Daily yield observations, published on each business day.
    /// </summary>
    Daily
}

/// <summary>
/// Specifies the maturity period for U.S. Treasury securities.
/// </summary>
/// <remarks>
/// Treasury securities with different maturities serve different purposes in portfolios and react
/// differently to economic conditions. Shorter maturities (3-month) are more sensitive to Federal Reserve
/// policy, while longer maturities (30-year) reflect long-term inflation and growth expectations.
/// </remarks>
public enum TreasuryYieldMaturity
{
    /// <summary>
    /// 3-month Treasury Bill maturity. Short-term instrument highly sensitive to Federal Reserve policy rates.
    /// </summary>
    _3Month,

    /// <summary>
    /// 2-year Treasury Note maturity. Short-to-medium term instrument used to gauge near-term economic expectations.
    /// </summary>
    _2Year,

    /// <summary>
    /// 5-year Treasury Note maturity. Medium-term instrument balancing near-term and long-term expectations.
    /// </summary>
    _5Year,

    /// <summary>
    /// 7-year Treasury Note maturity. Medium-to-long term instrument.
    /// </summary>
    _7Year,

    /// <summary>
    /// 10-year Treasury Note maturity. The most widely referenced benchmark for long-term interest rates,
    /// used for mortgage rates and other long-term lending.
    /// </summary>
    _10Year,

    /// <summary>
    /// 30-year Treasury Bond maturity. The longest-term Treasury security, reflecting long-term inflation
    /// expectations and economic growth outlook.
    /// </summary>
    _30Year
}

/* Ref:
{
    "name": "10-Year Treasury Constant Maturity Rate",
    "interval": "monthly",
    "unit": "percent",
    "data": [
        {
            "date": "2025-11-01",
            "value": "4.09"
        },
        {
            "date": "2025-10-01",
            "value": "4.06"
        }
    ]
}
*/
