using System.Text.Json;
using Tudormobile.AlphaVantage.Entities;

namespace Tudormobile.AlphaVantage.Extensions;

internal class TreasuryYieldParser : EntityParser
{
    private const string NAME_PROPERTY = "name";
    private const string INTERVAL_PROPERTY = "interval";
    private const string UNIT_PROPERTY = "unit";
    private const string DATA_PROPERTY = "data";
    private const string DATE_PROPERTY = "date";
    private const string VALUE_PROPERTY = "value";

    internal static TreasuryYield? FromDocument(JsonDocument jsonDocument,
        TreasuryYieldInterval interval,
        TreasuryYieldMaturity maturity)
    {
        var root = jsonDocument.RootElement;
        if (root.TryGetProperty(NAME_PROPERTY, out _))
        {
            var name = GetStringValue(root, NAME_PROPERTY);
            var unit = GetStringValue(root, UNIT_PROPERTY);

            if (root.TryGetProperty(DATA_PROPERTY, out JsonElement dataElement) && dataElement.ValueKind == JsonValueKind.Array)
            {
                var data = new List<TreasuryYieldData>();
                foreach (var item in dataElement.EnumerateArray())
                {
                    var yieldData = new TreasuryYieldData
                    {
                        Date = GetDateOnlyValue(item, DATE_PROPERTY),
                        Value = GetDecimalValue(item, VALUE_PROPERTY)
                    };
                    data.Add(yieldData);
                }

                return new TreasuryYield
                {
                    Name = name,
                    Interval = Enum.IsDefined(interval) ? interval : TreasuryYieldInterval.Monthly,
                    Maturity = Enum.IsDefined(maturity) ? maturity : TreasuryYieldMaturity._10Year,
                    Unit = unit,
                    Data = data
                };
            }
        }
        return null;
    }
}

/* Ref: https://www.alphavantage.co/query?function=TREASURY_YIELD&interval=monthly&maturity=10year&apikey=demo
{
    "name": "10-Year Treasury Constant Maturity Rate",
    "interval": "monthly",
    "unit": "percent",
    "data": [
        {
            "date": "2025-11-01",
            "value": "4.09"
        },
        {
            "date": "2025-10-01",
            "value": "4.06"
        }
    ]
}
*/