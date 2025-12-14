using Tudormobile.AlphaVantage.Entities;

namespace AlphaVantageAPI.Tests;

[TestClass]
public class TreasuryYieldTests
{
    [TestMethod]
    public void DefaultConstructor_InitializesWithDefaults()
    {
        // Act
        var treasuryYield = new TreasuryYield();

        // Assert
        Assert.IsNotNull(treasuryYield);
        Assert.AreEqual(string.Empty, treasuryYield.Name);
        Assert.AreEqual(string.Empty, treasuryYield.Unit);
        Assert.AreEqual(default, treasuryYield.Interval);
        Assert.AreEqual(default, treasuryYield.Maturity);
        Assert.IsNotNull(treasuryYield.Data);
        Assert.IsEmpty(treasuryYield.Data);
    }

    [TestMethod]
    public void Constructor_SetsPropertiesCorrectly()
    {
        // Arrange
        var expectedName = "10-Year Treasury Constant Maturity Rate";
        var expectedInterval = TreasuryYieldInterval.Daily;
        var expectedMaturity = TreasuryYieldMaturity._10Year;
        var expectedUnit = "percent";
        var expectedData = new List<TreasuryYieldData>
        {
            new() { Date = new DateOnly(2025, 12, 13), Value = 4.09m }
        };

        // Act
        var treasuryYield = new TreasuryYield
        {
            Name = expectedName,
            Interval = expectedInterval,
            Maturity = expectedMaturity,
            Unit = expectedUnit,
            Data = expectedData
        };

        // Assert
        Assert.IsNotNull(treasuryYield);
        Assert.AreEqual(expectedName, treasuryYield.Name);
        Assert.AreEqual(expectedInterval, treasuryYield.Interval);
        Assert.AreEqual(expectedMaturity, treasuryYield.Maturity);
        Assert.AreEqual(expectedUnit, treasuryYield.Unit);
        Assert.HasCount(1, treasuryYield.Data);
        Assert.AreEqual(new DateOnly(2025, 12, 13), treasuryYield.Data[0].Date);
        Assert.AreEqual(4.09m, treasuryYield.Data[0].Value);
    }

    [TestMethod]
    public void ImplementsIEntity()
    {
        // Arrange
        var treasuryYield = new TreasuryYield();

        // Assert
        Assert.IsInstanceOfType<IEntity>(treasuryYield);
    }
}
