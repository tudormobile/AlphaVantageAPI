using System.Text.Json;
using Tudormobile.AlphaVantage.Extensions;

namespace AlphaVantageAPI.Tests;

[TestClass]
public class EarningsEstimatesParserTests
{
    private readonly string validJson = @"
{
    ""symbol"" : ""IBM"",
    ""estimates"": [
        {
            ""date"": ""2026-12-31"",
            ""horizon"": ""next fiscal year"",
            ""eps_estimate_average"": ""12.1788"",
            ""eps_estimate_high"": ""12.7800"",
            ""eps_estimate_low"": ""11.2700"",
            ""eps_estimate_analyst_count"": ""21.0000"",
            ""eps_estimate_average_7_days_ago"": ""12.1757"",
            ""eps_estimate_average_30_days_ago"": ""12.1003"",
            ""eps_estimate_average_60_days_ago"": ""11.9406"",
            ""eps_estimate_average_90_days_ago"": ""11.8656"",
            ""eps_estimate_revision_up_trailing_7_days"": ""1.0000"",
            ""eps_estimate_revision_down_trailing_7_days"": null,
            ""eps_estimate_revision_up_trailing_30_days"": ""15.0000"",
            ""eps_estimate_revision_down_trailing_30_days"": ""3.0000"",
            ""revenue_estimate_average"": ""70129006340.00"",
            ""revenue_estimate_high"": ""71320000000.00"",
            ""revenue_estimate_low"": ""69522000000.00"",
            ""revenue_estimate_analyst_count"": ""21.00""
        },
        {
            ""date"": ""2025-12-31"",
            ""horizon"": ""current fiscal year"",
            ""eps_estimate_average"": ""10.5000"",
            ""eps_estimate_high"": ""11.0000"",
            ""eps_estimate_low"": ""10.0000"",
            ""eps_estimate_analyst_count"": ""20.0000"",
            ""eps_estimate_average_7_days_ago"": ""10.4800"",
            ""eps_estimate_average_30_days_ago"": ""10.4500"",
            ""eps_estimate_average_60_days_ago"": ""10.3000"",
            ""eps_estimate_average_90_days_ago"": ""10.2000"",
            ""eps_estimate_revision_up_trailing_7_days"": ""2.0000"",
            ""eps_estimate_revision_down_trailing_7_days"": ""1.0000"",
            ""eps_estimate_revision_up_trailing_30_days"": ""10.0000"",
            ""eps_estimate_revision_down_trailing_30_days"": ""2.0000"",
            ""revenue_estimate_average"": ""65000000000.00"",
            ""revenue_estimate_high"": ""66000000000.00"",
            ""revenue_estimate_low"": ""64000000000.00"",
            ""revenue_estimate_analyst_count"": ""20.00""
        }
    ]
}";

    [TestMethod]
    public void FromDocument_WithValidData_ParsesCorrectly()
    {
        // Arrange
        var document = JsonDocument.Parse(validJson);

        // Act
        var result = EarningsEstimatesParser.FromDocument(document, "IBM");

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual("IBM", result.Symbol);
        Assert.HasCount(2, result.Estimates);

        var firstEstimate = result.Estimates[0];
        Assert.AreEqual(new DateOnly(2026, 12, 31), firstEstimate.Date);
        Assert.AreEqual("next fiscal year", firstEstimate.Horizon);
        Assert.AreEqual(12.1788m, firstEstimate.EpsEstimateAverage);
        Assert.AreEqual(12.7800m, firstEstimate.EpsEstimateHigh);
        Assert.AreEqual(11.2700m, firstEstimate.EpsEstimateLow);
        Assert.AreEqual(21, firstEstimate.EpsEstimateAnalystCount);
        Assert.AreEqual(12.1757m, firstEstimate.EpsEstimateAverage7DaysAgo);
        Assert.AreEqual(12.1003m, firstEstimate.EpsEstimateAverage30DaysAgo);
        Assert.AreEqual(11.9406m, firstEstimate.EpsEstimateAverage60DaysAgo);
        Assert.AreEqual(11.8656m, firstEstimate.EpsEstimateAverage90DaysAgo);
        Assert.AreEqual(1, firstEstimate.EpsEstimateRevisionUpTrailing7Days);
        Assert.IsNull(firstEstimate.EpsEstimateRevisionDownTrailing7Days);
        Assert.AreEqual(15, firstEstimate.EpsEstimateRevisionUpTrailing30Days);
        Assert.AreEqual(3, firstEstimate.EpsEstimateRevisionDownTrailing30Days);
        Assert.AreEqual(70129006340.00m, firstEstimate.RevenueEstimateAverage);
        Assert.AreEqual(71320000000.00m, firstEstimate.RevenueEstimateHigh);
        Assert.AreEqual(69522000000.00m, firstEstimate.RevenueEstimateLow);
        Assert.AreEqual(21, firstEstimate.RevenueEstimateAnalystCount);

        var secondEstimate = result.Estimates[1];
        Assert.AreEqual(new DateOnly(2025, 12, 31), secondEstimate.Date);
        Assert.AreEqual("current fiscal year", secondEstimate.Horizon);
        Assert.AreEqual(10.5000m, secondEstimate.EpsEstimateAverage);
        Assert.AreEqual(11.0000m, secondEstimate.EpsEstimateHigh);
        Assert.AreEqual(10.0000m, secondEstimate.EpsEstimateLow);
        Assert.AreEqual(20, secondEstimate.EpsEstimateAnalystCount);
        Assert.AreEqual(10.4800m, secondEstimate.EpsEstimateAverage7DaysAgo);
        Assert.AreEqual(10.4500m, secondEstimate.EpsEstimateAverage30DaysAgo);
        Assert.AreEqual(10.3000m, secondEstimate.EpsEstimateAverage60DaysAgo);
        Assert.AreEqual(10.2000m, secondEstimate.EpsEstimateAverage90DaysAgo);
        Assert.AreEqual(2, secondEstimate.EpsEstimateRevisionUpTrailing7Days);
        Assert.AreEqual(1, secondEstimate.EpsEstimateRevisionDownTrailing7Days);
        Assert.AreEqual(10, secondEstimate.EpsEstimateRevisionUpTrailing30Days);
        Assert.AreEqual(2, secondEstimate.EpsEstimateRevisionDownTrailing30Days);
        Assert.AreEqual(65000000000.00m, secondEstimate.RevenueEstimateAverage);
        Assert.AreEqual(66000000000.00m, secondEstimate.RevenueEstimateHigh);
        Assert.AreEqual(64000000000.00m, secondEstimate.RevenueEstimateLow);
        Assert.AreEqual(20, secondEstimate.RevenueEstimateAnalystCount);
    }

    [TestMethod]
    public void FromDocument_WithEmptyDocument_ReturnsNull()
    {
        // Arrange
        var document = JsonDocument.Parse("{}");

        // Act
        var result = EarningsEstimatesParser.FromDocument(document, "IBM");

        // Assert
        Assert.IsNull(result);
    }

    [TestMethod]
    public void FromDocument_WithEmptyDataArray_ReturnsEmptyList()
    {
        // Arrange
        var json = @"{ ""estimates"": [] }";
        var document = JsonDocument.Parse(json);

        // Act
        var result = EarningsEstimatesParser.FromDocument(document, "IBM");

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual("IBM", result.Symbol);
        Assert.IsEmpty(result.Estimates);
    }

    [TestMethod]
    public void FromDocument_WithNullValues_UsesDefaults()
    {
        // Arrange
        var jsonWithNulls = @"
{
    ""symbol"" : ""IBM"",
    ""estimates"": [
        {
            ""date"": null,
            ""horizon"": null,
            ""eps_estimate_average"": null,
            ""eps_estimate_high"": null,
            ""eps_estimate_low"": null,
            ""eps_estimate_analyst_count"": null,
            ""eps_estimate_average_7_days_ago"": null,
            ""eps_estimate_average_30_days_ago"": null,
            ""eps_estimate_average_60_days_ago"": null,
            ""eps_estimate_average_90_days_ago"": null,
            ""eps_estimate_revision_up_trailing_7_days"": null,
            ""eps_estimate_revision_down_trailing_7_days"": null,
            ""eps_estimate_revision_up_trailing_30_days"": null,
            ""eps_estimate_revision_down_trailing_30_days"": null,
            ""revenue_estimate_average"": null,
            ""revenue_estimate_high"": null,
            ""revenue_estimate_low"": null,
            ""revenue_estimate_analyst_count"": null
        }
    ]
}";
        var document = JsonDocument.Parse(jsonWithNulls);

        // Act
        var result = EarningsEstimatesParser.FromDocument(document, "IBM");

        // Assert
        Assert.IsNotNull(result);
        Assert.HasCount(1, result.Estimates);
        var estimate = result.Estimates[0];
        Assert.AreEqual(default, estimate.Date);
        Assert.AreEqual("", estimate.Horizon);
        Assert.AreEqual(0m, estimate.EpsEstimateAverage);
        Assert.IsNull(estimate.EpsEstimateAnalystCount);
        Assert.IsNull(estimate.EpsEstimateRevisionDownTrailing7Days);
    }

    [TestMethod]
    public void FromDocument_WithSingleEstimate_ParsesCorrectly()
    {
        // Arrange
        var jsonSingle = @"
{
    ""symbol"" : ""IBM"",
    ""estimates"": [
        {
            ""date"": ""2026-12-31"",
            ""horizon"": ""next fiscal year"",
            ""eps_estimate_average"": ""12.1788"",
            ""eps_estimate_high"": ""12.7800"",
            ""eps_estimate_low"": ""11.2700"",
            ""eps_estimate_analyst_count"": ""21.0000"",
            ""eps_estimate_average_7_days_ago"": ""12.1757"",
            ""eps_estimate_average_30_days_ago"": ""12.1003"",
            ""eps_estimate_average_60_days_ago"": ""11.9406"",
            ""eps_estimate_average_90_days_ago"": ""11.8656"",
            ""eps_estimate_revision_up_trailing_7_days"": ""1.0000"",
            ""eps_estimate_revision_down_trailing_7_days"": null,
            ""eps_estimate_revision_up_trailing_30_days"": ""15.0000"",
            ""eps_estimate_revision_down_trailing_30_days"": ""3.0000"",
            ""revenue_estimate_average"": ""70129006340.00"",
            ""revenue_estimate_high"": ""71320000000.00"",
            ""revenue_estimate_low"": ""69522000000.00"",
            ""revenue_estimate_analyst_count"": ""21.00""
        }
    ]
}";
        var document = JsonDocument.Parse(jsonSingle);

        // Act
        var result = EarningsEstimatesParser.FromDocument(document, "AAPL");

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual("AAPL", result.Symbol);
        Assert.HasCount(1, result.Estimates);
        Assert.AreEqual(12.1788m, result.Estimates[0].EpsEstimateAverage);
        Assert.AreEqual(new DateOnly(2026, 12, 31), result.Estimates[0].Date);
        Assert.AreEqual("next fiscal year", result.Estimates[0].Horizon);
        Assert.AreEqual(21, result.Estimates[0].EpsEstimateAnalystCount);
    }
}
