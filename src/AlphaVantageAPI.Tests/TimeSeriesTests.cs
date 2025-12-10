using Tudormobile.AlphaVantage.Entities;

namespace AlphaVantageAPI.Tests;

[TestClass]
public class TimeSeriesTests
{
    [TestMethod]
    public void TestTimeSeriesDefaultValues()
    {
        var timeSeries = new TimeSeries();
        Assert.AreEqual(string.Empty, timeSeries.Symbol);
        Assert.AreEqual(default(DateOnly), timeSeries.LastUpdated);
        Assert.AreEqual(TimeSeries.TimeSeriesInterval.OneMin, timeSeries.Interval);
        Assert.IsNotNull(timeSeries.Data);
        Assert.IsTrue(timeSeries.Data.Count == 0);
    }

}
