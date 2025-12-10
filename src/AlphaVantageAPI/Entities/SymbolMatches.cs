namespace Tudormobile.AlphaVantage.Entities;

/// <summary>
/// Represents the collection of symbol search results returned by the Alpha Vantage symbol search API.
/// </summary>
/// <remarks>
/// This class encapsulates both the original search keywords and the list of matching symbols found,
/// providing a complete response for symbol search operations.
/// </remarks>
public class SymbolMatches
{
    /// <summary>
    /// Gets the search keywords used to find matching symbols.
    /// </summary>
    /// <value>The original search query string that was used to perform the symbol search.</value>
    public string Keywords { get; init; } = String.Empty;

    /// <summary>
    /// Gets the list of symbol matches found for the search query.
    /// </summary>
    /// <value>
    /// A list of <see cref="SymbolMatch"/> objects representing securities that match the search criteria.
    /// Returns an empty list if no matches are found.
    /// </value>
    /// <remarks>
    /// Results are typically ordered by relevance, with the best matches appearing first based on their
    /// <see cref="SymbolMatch.MatchScore"/> values.
    /// </remarks>
    public List<SymbolMatch> Matches { get; init; } = [];
}
