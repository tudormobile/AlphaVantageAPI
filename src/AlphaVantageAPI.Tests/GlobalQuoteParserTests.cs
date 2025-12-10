namespace AlphaVantageAPI.Tests;

[TestClass]
public class GlobalQuoteParserTests
{
    [TestMethod]
    public void TestFromDocument()
    {
        var json = @"{
            ""Global Quote"": {
                ""01. symbol"": ""IBM"",
                ""02. open"": ""125.0000"",
                ""03. high"": ""130.0000"",
                ""04. low"": ""124.0000"",
                ""05. price"": ""128.0000"",
                ""06. volume"": ""3000000"",
                ""07. latest trading day"": ""2024-06-14"",
                ""08. previous close"": ""126.0000"",
                ""09. change"": ""2.0000"",
                ""10. change percent"": ""1.58730158730159%""
            }
        }";
        var jsonDocument = System.Text.Json.JsonDocument.Parse(json);
        var globalQuote = Tudormobile.AlphaVantage.Extensions.GlobalQuoteParser.FromDocument(jsonDocument, "IBM");
        Assert.IsNotNull(globalQuote);
        Assert.AreEqual("IBM", globalQuote!.Symbol);
        Assert.AreEqual(125.0000m, globalQuote.Open);
        Assert.AreEqual(130.0000m, globalQuote.High);
        Assert.AreEqual(124.0000m, globalQuote.Low);
        Assert.AreEqual(128.0000m, globalQuote.Price);
        Assert.AreEqual(3000000, globalQuote.Volume);
        Assert.AreEqual(DateOnly.Parse("2024-06-14"), globalQuote.LatestTradingDay);
        Assert.AreEqual(126.0000m, globalQuote.PreviousClose);
        Assert.AreEqual(2.0000m, globalQuote.Change);

        Assert.AreEqual("1.587%", globalQuote.ChangePercent, "Should have rounded to 3-digits.");
    }

    [TestMethod]
    public void TestFromDocumentWithAllMissingData()
    {
        var json = @"{
            ""Global Quote"": {
                ""01. symbol"": null,
                ""02. open"": null,
                ""03. high"": null,
                ""04. low"": null,
                ""05. price"": null,
                ""06. volume"": null,
                ""07. latest trading day"": null,
                ""08. previous close"": null,
                ""09. change"": null,
                ""10. change percent"": null
            }
        }";
        var jsonDocument = System.Text.Json.JsonDocument.Parse(json);
        var globalQuote = Tudormobile.AlphaVantage.Extensions.GlobalQuoteParser.FromDocument(jsonDocument, "IBM");

        Assert.IsNotNull(globalQuote);
        Assert.AreEqual("", globalQuote.Symbol);
        Assert.AreEqual(0m, globalQuote.Open);
        Assert.AreEqual(0m, globalQuote.High);
        Assert.AreEqual(0m, globalQuote.Low);
        Assert.AreEqual(0m, globalQuote.Price);
        Assert.AreEqual(0, globalQuote.Volume);
        Assert.AreEqual(DateOnly.MinValue, globalQuote.LatestTradingDay);
        Assert.AreEqual(0m, globalQuote.PreviousClose);
        Assert.AreEqual(0m, globalQuote.Change);

        Assert.AreEqual("0.000%", globalQuote.ChangePercent, "Should have rounded to 3-digits.");

    }
}
