namespace Tudormobile.AlphaVantage.entities;

public class GlobalQuote
{
    public string Symbol { get; init; }
    public decimal Open { get; init; }
    public decimal High { get; init; }
    public decimal Low { get; init; }
    public decimal Price { get; init; }
    public long Volume { get; init; }
    public DateOnly LatestTradingDay { get; init; }
    public decimal PreviousClose { get; init; }
    public decimal Change { get; init; }
    public string ChangePercent => $"{(PreviousClose != 0 ? (Change / PreviousClose * 100) : 0):F2}%";
}
