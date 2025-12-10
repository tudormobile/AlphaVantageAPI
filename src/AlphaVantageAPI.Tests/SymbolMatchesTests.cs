namespace AlphaVantageAPI.Tests;

[TestClass]
public class SymbolMatchesTests
{
    [TestMethod]
    public void DefaultContructorTest()
    {
        var symbolMatches = new Tudormobile.AlphaVantage.Entities.SymbolMatches();
        Assert.IsNotNull(symbolMatches);
        Assert.AreEqual(string.Empty, symbolMatches.Keywords);
        Assert.IsNotNull(symbolMatches.Matches);
        Assert.IsEmpty(symbolMatches.Matches);
    }
}
