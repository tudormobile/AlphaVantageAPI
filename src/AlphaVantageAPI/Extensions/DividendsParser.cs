using System.Text.Json;
using Tudormobile.AlphaVantage.Entities;

namespace Tudormobile.AlphaVantage.Extensions;

internal class DividendsParser : EntityParser
{
    private const string DATA_PROPERTY = "data";
    private const string DECLARATION_DATE_PROPERTY = "declaration_date";
    private const string EX_DIVIDEND_DATE_PROPERTY = "ex_dividend_date";
    private const string RECORD_DATE_PROPERTY = "record_date";
    private const string PAYMENT_DATE_PROPERTY = "payment_date";
    private const string AMOUNT_PROPERTY = "amount";

    internal static Dividends? FromDocument(JsonDocument jsonDocument, string symbol)
    {
        var root = jsonDocument.RootElement;
        if (root.TryGetProperty(DATA_PROPERTY, out JsonElement dataElement) && dataElement.ValueKind == JsonValueKind.Array)
        {
            var dividends = new List<Dividend>();
            foreach (var item in dataElement.EnumerateArray())
            {
                var dividend = new Dividend
                {
                    DeclarationDate = GetDateOnlyValue(item, DECLARATION_DATE_PROPERTY),
                    ExDividendDate = GetDateOnlyValue(item, EX_DIVIDEND_DATE_PROPERTY),
                    RecordDate = GetDateOnlyValue(item, RECORD_DATE_PROPERTY),
                    PaymentDate = GetDateOnlyValue(item, PAYMENT_DATE_PROPERTY),
                    Amount = GetDecimalValue(item, AMOUNT_PROPERTY)
                };
                dividends.Add(dividend);
            }

            return new Dividends
            {
                Symbol = symbol,
                Data = dividends
            };
        }
        return null;
    }
}

/* Ref: https://www.alphavantage.co/query?function=DIVIDENDS&symbol=IBM&apikey=demo
{
    "data": [
        {
            "declaration_date": "2023-08-10",
            "ex_dividend_date": "2023-08-11",
            "record_date": "2023-08-14",
            "payment_date": "2023-09-01",
            "amount": "0.2300"
        },
        {
            "declaration_date": "2023-05-09",
            "ex_dividend_date": "2023-05-10",
            "record_date": "2023-05-11",
            "payment_date": "2023-06-10",
            "amount": "0.2300"
        }
    ]
}
*/