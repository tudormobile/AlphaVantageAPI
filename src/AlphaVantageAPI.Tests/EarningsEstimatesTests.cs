using Tudormobile.AlphaVantage.Entities;

namespace AlphaVantageAPI.Tests;

[TestClass]
public class EarningsEstimatesTests
{
    [TestMethod]
    public void DefaultConstructor_InitializesWithDefaults()
    {
        // Act
        var earningsEstimates = new EarningsEstimates();

        // Assert
        Assert.IsNotNull(earningsEstimates);
        Assert.AreEqual(string.Empty, earningsEstimates.Symbol);
        Assert.IsNotNull(earningsEstimates.Estimates);
        Assert.IsEmpty(earningsEstimates.Estimates);
    }

    [TestMethod]
    public void Constructor_SetsPropertiesCorrectly()
    {
        // Arrange
        var expectedSymbol = "AAPL";
        var expectedEstimates = new List<EarningsEstimate>
        {
            new() {
                Date = new DateOnly(2025, 12, 31),
                Horizon = "current quarter",
                EpsEstimateAverage = 1.50m,
                RevenueEstimateAverage = 100000000m
            }
        };

        // Act
        var earningsEstimates = new EarningsEstimates
        {
            Symbol = expectedSymbol,
            Estimates = expectedEstimates
        };

        // Assert
        Assert.IsNotNull(earningsEstimates);
        Assert.AreEqual(expectedSymbol, earningsEstimates.Symbol);
        Assert.HasCount(1, earningsEstimates.Estimates);
        Assert.AreEqual(1.50m, earningsEstimates.Estimates[0].EpsEstimateAverage);
        Assert.AreEqual(100000000m, earningsEstimates.Estimates[0].RevenueEstimateAverage);
    }

    [TestMethod]
    public void ImplementsIEntity()
    {
        // Arrange
        var earningsEstimates = new EarningsEstimates();

        // Assert
        Assert.IsInstanceOfType<IEntity>(earningsEstimates);
    }

    [TestMethod]
    public void Estimates_CanAddMultipleEstimates()
    {
        // Arrange
        var earningsEstimates = new EarningsEstimates { Symbol = "MSFT" };

        // Act
        earningsEstimates.Estimates.Add(new EarningsEstimate
        {
            Horizon = "current quarter",
            EpsEstimateAverage = 2.10m
        });
        earningsEstimates.Estimates.Add(new EarningsEstimate
        {
            Horizon = "next quarter",
            EpsEstimateAverage = 2.25m
        });

        // Assert
        Assert.HasCount(2, earningsEstimates.Estimates);
        Assert.AreEqual("current quarter", earningsEstimates.Estimates[0].Horizon);
        Assert.AreEqual("next quarter", earningsEstimates.Estimates[1].Horizon);
    }
}
