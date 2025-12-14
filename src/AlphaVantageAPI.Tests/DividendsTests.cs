using Tudormobile.AlphaVantage.Entities;

namespace AlphaVantageAPI.Tests;

[TestClass]
public class DividendsTests
{
    [TestMethod]
    public void DefaultConstructor_InitializesWithDefaults()
    {
        // Act
        var dividends = new Dividends();

        // Assert
        Assert.IsNotNull(dividends);
        Assert.AreEqual(string.Empty, dividends.Symbol);
        Assert.IsNotNull(dividends.Data);
        Assert.IsEmpty(dividends.Data);
    }

    [TestMethod]
    public void Constructor_SetsPropertiesCorrectly()
    {
        // Arrange
        var expectedSymbol = "AAPL";
        var expectedData = new List<Dividend>
        {
            new() {
                DeclarationDate = new DateOnly(2025, 11, 15),
                ExDividendDate = new DateOnly(2025, 11, 28),
                RecordDate = new DateOnly(2025, 11, 29),
                PaymentDate = new DateOnly(2025, 12, 15),
                Amount = 0.25m
            }
        };

        // Act
        var dividends = new Dividends
        {
            Symbol = expectedSymbol,
            Data = expectedData
        };

        // Assert
        Assert.IsNotNull(dividends);
        Assert.AreEqual(expectedSymbol, dividends.Symbol);
        Assert.HasCount(1, dividends.Data);
        Assert.AreEqual(0.25m, dividends.Data[0].Amount);
    }

    [TestMethod]
    public void ImplementsIEntity()
    {
        // Arrange
        var dividends = new Dividends();

        // Assert
        Assert.IsInstanceOfType<IEntity>(dividends);
    }

    [TestMethod]
    public void Data_CanAddMultipleDividends()
    {
        // Arrange
        var dividends = new Dividends { Symbol = "MSFT" };

        // Act
        dividends.Data.Add(new Dividend { Amount = 0.50m });
        dividends.Data.Add(new Dividend { Amount = 0.55m });

        // Assert
        Assert.HasCount(2, dividends.Data);
        Assert.AreEqual(0.50m, dividends.Data[0].Amount);
        Assert.AreEqual(0.55m, dividends.Data[1].Amount);
    }
}
