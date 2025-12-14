using System.Text.Json;
using Tudormobile.AlphaVantage.Entities;

namespace Tudormobile.AlphaVantage.Extensions;

internal class EarningsEstimatesParser : EntityParser
{
    private const string SYMBOL_PROPERTY = "symbol";
    private const string DATA_PROPERTY = "estimates";
    private const string DATE_PROPERTY = "date";
    private const string HORIZON_PROPERTY = "horizon";
    private const string EPS_ESTIMATE_AVERAGE_PROPERTY = "eps_estimate_average";
    private const string EPS_ESTIMATE_HIGH_PROPERTY = "eps_estimate_high";
    private const string EPS_ESTIMATE_LOW_PROPERTY = "eps_estimate_low";
    private const string EPS_ESTIMATE_ANALYST_COUNT_PROPERTY = "eps_estimate_analyst_count";
    private const string EPS_ESTIMATE_AVERAGE_7_DAYS_AGO_PROPERTY = "eps_estimate_average_7_days_ago";
    private const string EPS_ESTIMATE_AVERAGE_30_DAYS_AGO_PROPERTY = "eps_estimate_average_30_days_ago";
    private const string EPS_ESTIMATE_AVERAGE_60_DAYS_AGO_PROPERTY = "eps_estimate_average_60_days_ago";
    private const string EPS_ESTIMATE_AVERAGE_90_DAYS_AGO_PROPERTY = "eps_estimate_average_90_days_ago";
    private const string EPS_ESTIMATE_REVISION_UP_TRAILING_7_DAYS_PROPERTY = "eps_estimate_revision_up_trailing_7_days";
    private const string EPS_ESTIMATE_REVISION_DOWN_TRAILING_7_DAYS_PROPERTY = "eps_estimate_revision_down_trailing_7_days";
    private const string EPS_ESTIMATE_REVISION_UP_TRAILING_30_DAYS_PROPERTY = "eps_estimate_revision_up_trailing_30_days";
    private const string EPS_ESTIMATE_REVISION_DOWN_TRAILING_30_DAYS_PROPERTY = "eps_estimate_revision_down_trailing_30_days";
    private const string REVENUE_ESTIMATE_AVERAGE_PROPERTY = "revenue_estimate_average";
    private const string REVENUE_ESTIMATE_HIGH_PROPERTY = "revenue_estimate_high";
    private const string REVENUE_ESTIMATE_LOW_PROPERTY = "revenue_estimate_low";
    private const string REVENUE_ESTIMATE_ANALYST_COUNT_PROPERTY = "revenue_estimate_analyst_count";

    internal static EarningsEstimates? FromDocument(JsonDocument jsonDocument, string symbol)
    {
        var root = jsonDocument.RootElement;
        if (root.TryGetProperty(DATA_PROPERTY, out JsonElement dataElement) && dataElement.ValueKind == JsonValueKind.Array)
        {
            var estimates = new List<EarningsEstimate>();
            foreach (var item in dataElement.EnumerateArray())
            {
                var estimate = new EarningsEstimate
                {
                    Date = GetDateOnlyValue(item, DATE_PROPERTY),
                    Horizon = GetStringValue(item, HORIZON_PROPERTY),
                    EpsEstimateAverage = GetDecimalValue(item, EPS_ESTIMATE_AVERAGE_PROPERTY),
                    EpsEstimateHigh = GetDecimalValue(item, EPS_ESTIMATE_HIGH_PROPERTY),
                    EpsEstimateLow = GetDecimalValue(item, EPS_ESTIMATE_LOW_PROPERTY),
                    EpsEstimateAnalystCount = GetNullableIntValue(item, EPS_ESTIMATE_ANALYST_COUNT_PROPERTY),
                    EpsEstimateAverage7DaysAgo = GetDecimalValue(item, EPS_ESTIMATE_AVERAGE_7_DAYS_AGO_PROPERTY),
                    EpsEstimateAverage30DaysAgo = GetDecimalValue(item, EPS_ESTIMATE_AVERAGE_30_DAYS_AGO_PROPERTY),
                    EpsEstimateAverage60DaysAgo = GetDecimalValue(item, EPS_ESTIMATE_AVERAGE_60_DAYS_AGO_PROPERTY),
                    EpsEstimateAverage90DaysAgo = GetDecimalValue(item, EPS_ESTIMATE_AVERAGE_90_DAYS_AGO_PROPERTY),
                    EpsEstimateRevisionUpTrailing7Days = (int)GetDecimalValue(item, EPS_ESTIMATE_REVISION_UP_TRAILING_7_DAYS_PROPERTY),
                    EpsEstimateRevisionDownTrailing7Days = GetNullableIntValue(item, EPS_ESTIMATE_REVISION_DOWN_TRAILING_7_DAYS_PROPERTY),
                    EpsEstimateRevisionUpTrailing30Days = GetNullableIntValue(item, EPS_ESTIMATE_REVISION_UP_TRAILING_30_DAYS_PROPERTY),
                    EpsEstimateRevisionDownTrailing30Days = GetNullableIntValue(item, EPS_ESTIMATE_REVISION_DOWN_TRAILING_30_DAYS_PROPERTY),
                    RevenueEstimateAverage = GetDecimalValue(item, REVENUE_ESTIMATE_AVERAGE_PROPERTY),
                    RevenueEstimateHigh = GetDecimalValue(item, REVENUE_ESTIMATE_HIGH_PROPERTY),
                    RevenueEstimateLow = GetDecimalValue(item, REVENUE_ESTIMATE_LOW_PROPERTY),
                    RevenueEstimateAnalystCount = (int)GetDecimalValue(item, REVENUE_ESTIMATE_ANALYST_COUNT_PROPERTY)
                };
                estimates.Add(estimate);
            }

            return new EarningsEstimates
            {
                Symbol = symbol,
                Estimates = estimates
            };
        }
        return null;
    }
}

/* Ref: https://www.alphavantage.co/query?function=EARNINGS_ESTIMATES&symbol=IBM&apikey=demo
{
    "data": [
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
    ]
}
*/