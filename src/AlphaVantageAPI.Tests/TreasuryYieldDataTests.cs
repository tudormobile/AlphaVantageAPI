using Tudormobile.AlphaVantage.Entities;

namespace AlphaVantageAPI.Tests;

[TestClass]
public class TreasuryYieldDataTests
{
    [TestMethod]
    public void Constructor_SetsPropertiesCorrectly()
    {
        // Arrange
        var expectedDate = new DateOnly(2025, 12, 13);
        var expectedValue = 4.09m;

        // Act
        var treasuryYieldData = new TreasuryYieldData
        {
            Date = expectedDate,
            Value = expectedValue
        };

        // Assert
        Assert.IsNotNull(treasuryYieldData);
        Assert.AreEqual(expectedDate, treasuryYieldData.Date);
        Assert.AreEqual(expectedValue, treasuryYieldData.Value);
    }

    [TestMethod]
    public void DefaultConstructor_InitializesWithDefaults()
    {
        // Act
        var treasuryYieldData = new TreasuryYieldData();

        // Assert
        Assert.IsNotNull(treasuryYieldData);
        Assert.AreEqual(default, treasuryYieldData.Date);
        Assert.AreEqual(default, treasuryYieldData.Value);
    }

    [TestMethod]
    public void ImplementsIEntity()
    {
        // Arrange
        var treasuryYieldData = new TreasuryYieldData();

        // Assert
        Assert.IsInstanceOfType<IEntity>(treasuryYieldData);
    }
}
