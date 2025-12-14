namespace Tudormobile.AlphaVantage.Entities;

/// <summary>
/// Represents a collection of dividend payment records for a specific stock symbol.
/// </summary>
/// <remarks>
/// This class contains historical dividend data retrieved from the Alpha Vantage API,
/// including all dividend payments for a given security over time.
/// </remarks>
public class Dividends : IEntity
{
    /// <summary>
    /// Gets or sets the stock symbol or ticker for which dividend data is provided.
    /// </summary>
    /// <value>The stock symbol (e.g., "AAPL", "MSFT") that uniquely identifies the security.</value>
    public string Symbol { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the collection of individual dividend payment records.
    /// </summary>
    /// <value>A list of <see cref="Dividend"/> objects, each representing a single dividend payment event.
    /// The list is ordered chronologically by the dividend payment dates.</value>
    public List<Dividend> Data { get; set; } = [];
}

/// <summary>
/// Represents a single dividend payment event for a stock.
/// </summary>
/// <remarks>
/// This class encapsulates all key dates and payment information associated with a dividend distribution,
/// following the standard corporate dividend payment timeline from declaration through payment.
/// </remarks>
public class Dividend : IEntity
{
    /// <summary>
    /// Gets or sets the date when the company's board of directors announced the dividend.
    /// </summary>
    /// <value>The declaration date marks when the dividend was officially announced to shareholders.</value>
    public DateOnly DeclarationDate { get; set; }

    /// <summary>
    /// Gets or sets the ex-dividend date, which is the cutoff date for dividend eligibility.
    /// </summary>
    /// <value>Investors who purchase the stock on or after this date are not entitled to receive the declared dividend.
    /// To receive the dividend, shares must be owned before this date.</value>
    public DateOnly ExDividendDate { get; set; }

    /// <summary>
    /// Gets or sets the record date, which determines which shareholders are eligible to receive the dividend.
    /// </summary>
    /// <value>The date on which the company reviews its records to determine the shareholders of record
    /// who will receive the dividend payment.</value>
    public DateOnly RecordDate { get; set; }

    /// <summary>
    /// Gets or sets the date when the dividend payment is distributed to eligible shareholders.
    /// </summary>
    /// <value>The actual date when dividend payments are sent to shareholders who owned the stock as of the record date.</value>
    public DateOnly PaymentDate { get; set; }

    /// <summary>
    /// Gets or sets the dividend payment amount per share.
    /// </summary>
    /// <value>The dollar amount paid per share as a dividend, expressed as a decimal value (e.g., 0.25 represents $0.25 per share).</value>
    public decimal Amount { get; set; }
}

/* ref:
{
    "declaration_date": "2023-08-10",
    "ex_dividend_date": "2023-08-11",
    "record_date": "2023-08-14",
    "payment_date": "2023-09-01",
    "amount": "0.2300"
}
*/
