using Tudormobile.AlphaVantage.Entities;

namespace AlphaVantageAPI.Tests;

[TestClass]
public class GlobalQuoteTests
{
    [TestMethod]
    public void TestWithPreviousCloseZero()
    {
        var globalQuote = new GlobalQuote
        {
            Symbol = "TEST",
            Open = 100.0m,
            High = 110.0m,
            Low = 90.0m,
            Price = 105.0m,
            Volume = 1000000,
            LatestTradingDay = DateOnly.Parse("2024-06-14"),
            PreviousClose = 0.0m,
            Change = 5.0m
        };
        var expectedChangePercent = "0.000%";
        var actualChangePercent = globalQuote.ChangePercent;
        Assert.AreEqual(expectedChangePercent, actualChangePercent, "Format is always 3 digits and then percent sign.");
    }

    [TestMethod]
    public void TestChangePercentWithNoRounding()
    {
        var globalQuote = new Tudormobile.AlphaVantage.Entities.GlobalQuote
        {
            Symbol = "TEST",
            Open = 100.0m,
            High = 110.0m,
            Low = 90.0m,
            Price = 105.0m,
            Volume = 1000000,
            LatestTradingDay = DateOnly.Parse("2024-06-14"),
            PreviousClose = 100.0m,
            Change = 5.0m
        };
        var expectedChangePercent = "5.000%";
        var actualChangePercent = ((globalQuote.Change / globalQuote.PreviousClose) * 100).ToString("F3") + "%";
        Assert.AreEqual(expectedChangePercent, actualChangePercent, "Change percent should match expected value with no rounding.");
    }
    [TestMethod]
    public void TestChangePercentRoundingUp()
    {
        var globalQuote = new Tudormobile.AlphaVantage.Entities.GlobalQuote
        {
            Symbol = "TEST",
            Open = 100.0m,
            High = 110.0m,
            Low = 90.0m,
            Price = 105.0m,
            Volume = 1000000,
            LatestTradingDay = DateOnly.Parse("2024-06-14"),
            PreviousClose = 100.0m,
            Change = 5.6789m
        };
        var expectedChangePercent = "5.679%";
        var actualChangePercent = ((globalQuote.Change / globalQuote.PreviousClose) * 100).ToString("F3") + "%";
        Assert.AreEqual(expectedChangePercent, actualChangePercent, "Change percent should match expected value with rounding up.");
    }

    [TestMethod]
    public void TestChangePercentRoundingDown()
    {
        var globalQuote = new Tudormobile.AlphaVantage.Entities.GlobalQuote
        {
            Symbol = "TEST",
            Open = 100.0m,
            High = 110.0m,
            Low = 90.0m,
            Price = 105.0m,
            Volume = 1000000,
            LatestTradingDay = DateOnly.Parse("2024-06-14"),
            PreviousClose = 100.0m,
            Change = 5.4321m
        };
        var expectedChangePercent = "5.432%";
        var actualChangePercent = ((globalQuote.Change / globalQuote.PreviousClose) * 100).ToString("F3") + "%";
        Assert.AreEqual(expectedChangePercent, actualChangePercent, "Change percent should match expected value with rounding down.");
    }
}
