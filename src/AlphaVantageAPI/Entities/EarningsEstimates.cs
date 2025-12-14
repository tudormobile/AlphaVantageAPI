namespace Tudormobile.AlphaVantage.Entities;

/// <summary>
/// Represents a collection of earnings per share (EPS) and revenue estimates for a specific stock symbol.
/// </summary>
/// <remarks>
/// This class contains analyst consensus estimates for future earnings, including both quarterly and annual
/// projections with historical tracking of estimate revisions over time.
/// </remarks>
public class EarningsEstimates : IEntity
{
    /// <summary>
    /// Gets or sets the stock symbol or ticker for which earnings estimates are provided.
    /// </summary>
    /// <value>The stock symbol (e.g., "AAPL", "MSFT") that uniquely identifies the security.</value>
    public string Symbol { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the collection of individual earnings estimate records.
    /// </summary>
    /// <value>A list of <see cref="EarningsEstimate"/> objects, each representing analyst consensus estimates
    /// for a specific time horizon (quarter or fiscal year).</value>
    public List<EarningsEstimate> Estimates { get; set; } = [];
}

/// <summary>
/// Represents analyst consensus estimates for earnings per share (EPS) and revenue for a specific time period.
/// </summary>
/// <remarks>
/// This class tracks both current analyst estimates and the historical changes in those estimates over time,
/// providing insight into how analyst sentiment has evolved. Includes high/low ranges, analyst counts, and
/// revision tracking to measure estimate momentum.
/// </remarks>
public class EarningsEstimate : IEntity
{
    /// <summary>
    /// Gets or sets the date for which these estimates apply.
    /// </summary>
    /// <value>The end date of the fiscal period (quarter or year) for which estimates are provided.</value>
    public DateOnly Date { get; set; }

    /// <summary>
    /// Gets or sets the time horizon for the estimate.
    /// </summary>
    /// <value>A string describing the time period, such as "current quarter", "next quarter", 
    /// "current fiscal year", or "next fiscal year".</value>
    public string Horizon { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the average (consensus) EPS estimate from all analysts covering the stock.
    /// </summary>
    /// <value>The mean of all analyst EPS estimates, expressed as dollars per share.</value>
    public decimal EpsEstimateAverage { get; set; }

    /// <summary>
    /// Gets or sets the highest EPS estimate among all analysts.
    /// </summary>
    /// <value>The most optimistic EPS estimate, expressed as dollars per share.</value>
    public decimal EpsEstimateHigh { get; set; }

    /// <summary>
    /// Gets or sets the lowest EPS estimate among all analysts.
    /// </summary>
    /// <value>The most pessimistic EPS estimate, expressed as dollars per share.</value>
    public decimal EpsEstimateLow { get; set; }

    /// <summary>
    /// Gets or sets the number of analysts who provided EPS estimates.
    /// </summary>
    /// <value>The count of analysts contributing to the EPS consensus estimate. May be null if data is unavailable.</value>
    public int? EpsEstimateAnalystCount { get; set; }

    /// <summary>
    /// Gets or sets the average EPS estimate as it stood 7 days ago.
    /// </summary>
    /// <value>The historical consensus EPS estimate from one week prior, used to track recent changes in analyst sentiment.</value>
    public decimal EpsEstimateAverage7DaysAgo { get; set; }

    /// <summary>
    /// Gets or sets the average EPS estimate as it stood 30 days ago.
    /// </summary>
    /// <value>The historical consensus EPS estimate from one month prior, used to track medium-term changes in analyst sentiment.</value>
    public decimal EpsEstimateAverage30DaysAgo { get; set; }

    /// <summary>
    /// Gets or sets the average EPS estimate as it stood 60 days ago.
    /// </summary>
    /// <value>The historical consensus EPS estimate from two months prior, used to track longer-term estimate trends.</value>
    public decimal EpsEstimateAverage60DaysAgo { get; set; }

    /// <summary>
    /// Gets or sets the average EPS estimate as it stood 90 days ago.
    /// </summary>
    /// <value>The historical consensus EPS estimate from three months prior, used to track longer-term estimate trends.</value>
    public decimal EpsEstimateAverage90DaysAgo { get; set; }

    /// <summary>
    /// Gets or sets the number of upward estimate revisions in the trailing 7 days.
    /// </summary>
    /// <value>The count of analysts who raised their EPS estimates in the past week, indicating improving sentiment.</value>
    public int EpsEstimateRevisionUpTrailing7Days { get; set; }

    /// <summary>
    /// Gets or sets the number of downward estimate revisions in the trailing 7 days.
    /// </summary>
    /// <value>The count of analysts who lowered their EPS estimates in the past week, indicating declining sentiment.
    /// May be null if data is unavailable.</value>
    public int? EpsEstimateRevisionDownTrailing7Days { get; set; }

    /// <summary>
    /// Gets or sets the number of upward estimate revisions in the trailing 30 days.
    /// </summary>
    /// <value>The count of analysts who raised their EPS estimates in the past month, indicating improving sentiment.
    /// May be null if data is unavailable.</value>
    public int? EpsEstimateRevisionUpTrailing30Days { get; set; }

    /// <summary>
    /// Gets or sets the number of downward estimate revisions in the trailing 30 days.
    /// </summary>
    /// <value>The count of analysts who lowered their EPS estimates in the past month, indicating declining sentiment.
    /// May be null if data is unavailable.</value>
    public int? EpsEstimateRevisionDownTrailing30Days { get; set; }

    /// <summary>
    /// Gets or sets the average (consensus) revenue estimate from all analysts covering the stock.
    /// </summary>
    /// <value>The mean of all analyst revenue estimates, expressed in dollars.</value>
    public decimal RevenueEstimateAverage { get; set; }

    /// <summary>
    /// Gets or sets the highest revenue estimate among all analysts.
    /// </summary>
    /// <value>The most optimistic revenue estimate, expressed in dollars.</value>
    public decimal RevenueEstimateHigh { get; set; }

    /// <summary>
    /// Gets or sets the lowest revenue estimate among all analysts.
    /// </summary>
    /// <value>The most pessimistic revenue estimate, expressed in dollars.</value>
    public decimal RevenueEstimateLow { get; set; }

    /// <summary>
    /// Gets or sets the number of analysts who provided revenue estimates.
    /// </summary>
    /// <value>The count of analysts contributing to the revenue consensus estimate.</value>
    public int RevenueEstimateAnalystCount { get; set; }
}

/* ref: 
{
    "date": "2026-12-31",
    "horizon": "next fiscal year",
    "eps_estimate_average": "12.1788",
    "eps_estimate_high": "12.7800",
    "eps_estimate_low": "11.2700",
    "eps_estimate_analyst_count": "21.0000",
    "eps_estimate_average_7_days_ago": "12.1757",
    "eps_estimate_average_30_days_ago": "12.1003",
    "eps_estimate_average_60_days_ago": "11.9406",
    "eps_estimate_average_90_days_ago": "11.8656",
    "eps_estimate_revision_up_trailing_7_days": "1.0000",
    "eps_estimate_revision_down_trailing_7_days": null,
    "eps_estimate_revision_up_trailing_30_days": "15.0000",
    "eps_estimate_revision_down_trailing_30_days": "3.0000",
    "revenue_estimate_average": "70129006340.00",
    "revenue_estimate_high": "71320000000.00",
    "revenue_estimate_low": "69522000000.00",
    "revenue_estimate_analyst_count": "21.00"
}
 */