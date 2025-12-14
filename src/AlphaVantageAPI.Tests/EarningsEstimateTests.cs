using Tudormobile.AlphaVantage.Entities;

namespace AlphaVantageAPI.Tests;

[TestClass]
public class EarningsEstimateTests
{
    [TestMethod]
    public void Constructor_SetsPropertiesToDefault()
    {
        // Act
        var estimate = new EarningsEstimate();

        // Assert
        Assert.AreEqual(default, estimate.Date);
        Assert.AreEqual(string.Empty, estimate.Horizon);
        Assert.AreEqual(0m, estimate.EpsEstimateAverage);
        Assert.AreEqual(0m, estimate.EpsEstimateHigh);
        Assert.AreEqual(0m, estimate.EpsEstimateLow);
        Assert.IsNull(estimate.EpsEstimateAnalystCount);
        Assert.AreEqual(0m, estimate.EpsEstimateAverage7DaysAgo);
        Assert.AreEqual(0m, estimate.EpsEstimateAverage30DaysAgo);
        Assert.AreEqual(0m, estimate.EpsEstimateAverage60DaysAgo);
        Assert.AreEqual(0m, estimate.EpsEstimateAverage90DaysAgo);
        Assert.AreEqual(0, estimate.EpsEstimateRevisionUpTrailing7Days);
        Assert.IsNull(estimate.EpsEstimateRevisionDownTrailing7Days);
        Assert.IsNull(estimate.EpsEstimateRevisionUpTrailing30Days);
        Assert.IsNull(estimate.EpsEstimateRevisionDownTrailing30Days);
        Assert.AreEqual(0m, estimate.RevenueEstimateAverage);
        Assert.AreEqual(0m, estimate.RevenueEstimateHigh);
        Assert.AreEqual(0m, estimate.RevenueEstimateLow);
        Assert.AreEqual(0, estimate.RevenueEstimateAnalystCount);
    }

    [TestMethod]
    public void SetProperties_WithValidValues_StoresCorrectly()
    {
        // Arrange
        var date = new DateOnly(2026, 12, 31);
        var horizon = "next fiscal year";

        // Act
        var estimate = new EarningsEstimate
        {
            Date = date,
            Horizon = horizon,
            EpsEstimateAverage = 12.1788m,
            EpsEstimateHigh = 12.7800m,
            EpsEstimateLow = 11.2700m,
            EpsEstimateAnalystCount = 21,
            EpsEstimateAverage7DaysAgo = 12.1757m,
            EpsEstimateAverage30DaysAgo = 12.1003m,
            EpsEstimateAverage60DaysAgo = 11.9406m,
            EpsEstimateAverage90DaysAgo = 11.8656m,
            EpsEstimateRevisionUpTrailing7Days = 1,
            EpsEstimateRevisionDownTrailing7Days = null,
            EpsEstimateRevisionUpTrailing30Days = 15,
            EpsEstimateRevisionDownTrailing30Days = 3,
            RevenueEstimateAverage = 70129006340.00m,
            RevenueEstimateHigh = 71320000000.00m,
            RevenueEstimateLow = 69522000000.00m,
            RevenueEstimateAnalystCount = 21
        };

        // Assert
        Assert.AreEqual(date, estimate.Date);
        Assert.AreEqual(horizon, estimate.Horizon);
        Assert.AreEqual(12.1788m, estimate.EpsEstimateAverage);
        Assert.AreEqual(12.7800m, estimate.EpsEstimateHigh);
        Assert.AreEqual(11.2700m, estimate.EpsEstimateLow);
        Assert.AreEqual(21, estimate.EpsEstimateAnalystCount);
        Assert.AreEqual(12.1757m, estimate.EpsEstimateAverage7DaysAgo);
        Assert.AreEqual(12.1003m, estimate.EpsEstimateAverage30DaysAgo);
        Assert.AreEqual(11.9406m, estimate.EpsEstimateAverage60DaysAgo);
        Assert.AreEqual(11.8656m, estimate.EpsEstimateAverage90DaysAgo);
        Assert.AreEqual(1, estimate.EpsEstimateRevisionUpTrailing7Days);
        Assert.IsNull(estimate.EpsEstimateRevisionDownTrailing7Days);
        Assert.AreEqual(15, estimate.EpsEstimateRevisionUpTrailing30Days);
        Assert.AreEqual(3, estimate.EpsEstimateRevisionDownTrailing30Days);
        Assert.AreEqual(70129006340.00m, estimate.RevenueEstimateAverage);
        Assert.AreEqual(71320000000.00m, estimate.RevenueEstimateHigh);
        Assert.AreEqual(69522000000.00m, estimate.RevenueEstimateLow);
        Assert.AreEqual(21, estimate.RevenueEstimateAnalystCount);
    }

    [TestMethod]
    public void EpsEstimateProperties_WithPreciseDecimals_RetainPrecision()
    {
        // Arrange
        var estimate = new EarningsEstimate
        {
            // Act
            EpsEstimateAverage = 12.1788m,
            EpsEstimateHigh = 12.7800m,
            EpsEstimateLow = 11.2700m
        };

        // Assert
        Assert.AreEqual(12.1788m, estimate.EpsEstimateAverage);
        Assert.AreEqual(12.7800m, estimate.EpsEstimateHigh);
        Assert.AreEqual(11.2700m, estimate.EpsEstimateLow);
    }

    [TestMethod]
    public void RevenueEstimateProperties_WithLargeValues_StoresCorrectly()
    {
        // Arrange
        var estimate = new EarningsEstimate
        {
            // Act
            RevenueEstimateAverage = 70129006340.00m,
            RevenueEstimateHigh = 71320000000.00m,
            RevenueEstimateLow = 69522000000.00m
        };

        // Assert
        Assert.AreEqual(70129006340.00m, estimate.RevenueEstimateAverage);
        Assert.AreEqual(71320000000.00m, estimate.RevenueEstimateHigh);
        Assert.AreEqual(69522000000.00m, estimate.RevenueEstimateLow);
    }

    [TestMethod]
    public void NullableProperties_CanBeSetToNull()
    {
        // Arrange
        var estimate = new EarningsEstimate
        {
            EpsEstimateAnalystCount = 10,
            EpsEstimateRevisionDownTrailing7Days = 5,
            EpsEstimateRevisionUpTrailing30Days = 15,
            EpsEstimateRevisionDownTrailing30Days = 3
        };

        // Act
        estimate.EpsEstimateAnalystCount = null;
        estimate.EpsEstimateRevisionDownTrailing7Days = null;
        estimate.EpsEstimateRevisionUpTrailing30Days = null;
        estimate.EpsEstimateRevisionDownTrailing30Days = null;

        // Assert
        Assert.IsNull(estimate.EpsEstimateAnalystCount);
        Assert.IsNull(estimate.EpsEstimateRevisionDownTrailing7Days);
        Assert.IsNull(estimate.EpsEstimateRevisionUpTrailing30Days);
        Assert.IsNull(estimate.EpsEstimateRevisionDownTrailing30Days);
    }

    [TestMethod]
    public void NullableProperties_CanHaveValues()
    {
        // Arrange
        var estimate = new EarningsEstimate
        {
            // Act
            EpsEstimateAnalystCount = 21,
            EpsEstimateRevisionDownTrailing7Days = 0,
            EpsEstimateRevisionUpTrailing30Days = 15,
            EpsEstimateRevisionDownTrailing30Days = 3
        };

        // Assert
        Assert.AreEqual(21, estimate.EpsEstimateAnalystCount);
        Assert.AreEqual(0, estimate.EpsEstimateRevisionDownTrailing7Days);
        Assert.AreEqual(15, estimate.EpsEstimateRevisionUpTrailing30Days);
        Assert.AreEqual(3, estimate.EpsEstimateRevisionDownTrailing30Days);
    }

    [TestMethod]
    public void Horizon_WithTypicalValues_StoresCorrectly()
    {
        // Arrange
        var estimate = new EarningsEstimate
        {
            // Act & Assert - Test different horizon values
            Horizon = "current quarter"
        };
        Assert.AreEqual("current quarter", estimate.Horizon);

        estimate.Horizon = "next quarter";
        Assert.AreEqual("next quarter", estimate.Horizon);

        estimate.Horizon = "current fiscal year";
        Assert.AreEqual("current fiscal year", estimate.Horizon);

        estimate.Horizon = "next fiscal year";
        Assert.AreEqual("next fiscal year", estimate.Horizon);
    }

    [TestMethod]
    public void EarningsEstimate_ImplementsIEntity()
    {
        // Arrange
        var estimate = new EarningsEstimate();

        // Act & Assert
        Assert.IsInstanceOfType<IEntity>(estimate);
    }

    [TestMethod]
    public void Date_WithVariousDates_StoresCorrectly()
    {
        // Arrange
        var estimate = new EarningsEstimate
        {
            // Act & Assert
            Date = new DateOnly(2025, 3, 31)
        };
        Assert.AreEqual(new DateOnly(2025, 3, 31), estimate.Date);

        estimate.Date = new DateOnly(2026, 12, 31);
        Assert.AreEqual(new DateOnly(2026, 12, 31), estimate.Date);

        estimate.Date = DateOnly.MinValue;
        Assert.AreEqual(DateOnly.MinValue, estimate.Date);

        estimate.Date = DateOnly.MaxValue;
        Assert.AreEqual(DateOnly.MaxValue, estimate.Date);
    }

    [TestMethod]
    public void HistoricalAverages_ShowEstimateTrend()
    {
        // Arrange
        var estimate = new EarningsEstimate
        {
            EpsEstimateAverage = 12.1788m,
            EpsEstimateAverage7DaysAgo = 12.1757m,
            EpsEstimateAverage30DaysAgo = 12.1003m,
            EpsEstimateAverage60DaysAgo = 11.9406m,
            EpsEstimateAverage90DaysAgo = 11.8656m
        };

        // Assert - Verify upward trend
        Assert.IsGreaterThan(estimate.EpsEstimateAverage7DaysAgo, estimate.EpsEstimateAverage);
        Assert.IsGreaterThan(estimate.EpsEstimateAverage30DaysAgo, estimate.EpsEstimateAverage7DaysAgo);
        Assert.IsGreaterThan(estimate.EpsEstimateAverage60DaysAgo, estimate.EpsEstimateAverage30DaysAgo);
        Assert.IsGreaterThan(estimate.EpsEstimateAverage90DaysAgo, estimate.EpsEstimateAverage60DaysAgo);
    }

    [TestMethod]
    public void RevisionCounts_WithZeroValues_StoresCorrectly()
    {
        // Arrange
        var estimate = new EarningsEstimate
        {
            // Act
            EpsEstimateRevisionUpTrailing7Days = 0,
            EpsEstimateRevisionDownTrailing7Days = 0,
            EpsEstimateRevisionUpTrailing30Days = 0,
            EpsEstimateRevisionDownTrailing30Days = 0
        };

        // Assert
        Assert.AreEqual(0, estimate.EpsEstimateRevisionUpTrailing7Days);
        Assert.AreEqual(0, estimate.EpsEstimateRevisionDownTrailing7Days);
        Assert.AreEqual(0, estimate.EpsEstimateRevisionUpTrailing30Days);
        Assert.AreEqual(0, estimate.EpsEstimateRevisionDownTrailing30Days);
    }

    [TestMethod]
    public void AnalystCounts_WithPositiveValues_StoresCorrectly()
    {
        // Arrange
        var estimate = new EarningsEstimate
        {
            // Act
            EpsEstimateAnalystCount = 21,
            RevenueEstimateAnalystCount = 21
        };

        // Assert
        Assert.AreEqual(21, estimate.EpsEstimateAnalystCount);
        Assert.AreEqual(21, estimate.RevenueEstimateAnalystCount);
    }

    [TestMethod]
    public void TwoEstimates_AreIndependent()
    {
        // Arrange
        var estimate1 = new EarningsEstimate
        {
            Date = new DateOnly(2025, 12, 31),
            Horizon = "current fiscal year",
            EpsEstimateAverage = 10.0m
        };

        var estimate2 = new EarningsEstimate
        {
            Date = new DateOnly(2026, 12, 31),
            Horizon = "next fiscal year",
            EpsEstimateAverage = 12.0m
        };

        // Act
        estimate1.EpsEstimateAverage = 11.0m;

        // Assert
        Assert.AreEqual(11.0m, estimate1.EpsEstimateAverage);
        Assert.AreEqual(12.0m, estimate2.EpsEstimateAverage);
    }

    [TestMethod]
    public void Horizon_CanBeEmptyString()
    {
        // Arrange
        var estimate = new EarningsEstimate
        {
            // Act
            Horizon = ""
        };

        // Assert
        Assert.AreEqual("", estimate.Horizon);
    }

    [TestMethod]
    public void NegativeRevisions_AreAllowed()
    {
        // Arrange
        var estimate = new EarningsEstimate
        {
            // Act - While semantically odd, the type allows negative values
            EpsEstimateRevisionUpTrailing7Days = -1,
            EpsEstimateRevisionDownTrailing7Days = -2
        };

        // Assert
        Assert.AreEqual(-1, estimate.EpsEstimateRevisionUpTrailing7Days);
        Assert.AreEqual(-2, estimate.EpsEstimateRevisionDownTrailing7Days);
    }

    [TestMethod]
    public void DecimalProperties_WithNegativeValues_StoresCorrectly()
    {
        // Arrange
        var estimate = new EarningsEstimate
        {
            // Act - Negative EPS estimates are possible for loss-making companies
            EpsEstimateAverage = -1.50m,
            EpsEstimateHigh = -0.50m,
            EpsEstimateLow = -2.00m
        };

        // Assert
        Assert.AreEqual(-1.50m, estimate.EpsEstimateAverage);
        Assert.AreEqual(-0.50m, estimate.EpsEstimateHigh);
        Assert.AreEqual(-2.00m, estimate.EpsEstimateLow);
    }
}
