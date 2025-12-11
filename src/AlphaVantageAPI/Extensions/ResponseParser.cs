using System.Text.Json;
using Tudormobile.AlphaVantage.Entities;

namespace Tudormobile.AlphaVantage.Extensions;

internal class ResponseParser
{
    internal static AlphaVantageResponse<T> CreateResponse<T>(T? result, JsonDocument jsonDocument, string errorMessage = "Unknown error occurred.") where T : class, IEntity
    {
        return new AlphaVantageResponse<T>
        {
            Result = result,
            ErrorMessage = result == null ? FindErrorMessage(jsonDocument.RootElement, errorMessage) : null
        };
    }

    private static string FindErrorMessage(JsonElement root, string defaultMessage)
    {
        if (root.TryGetProperty("Information", out JsonElement informationElement))
        {
            var message = informationElement.GetString();
            if (!string.IsNullOrEmpty(message))
            {
                return message;
            }
        }

        if (root.TryGetProperty("Error Message", out JsonElement errorElement))
        {
            var message = errorElement.GetString();
            if (!string.IsNullOrEmpty(message))
            {
                return message;
            }
        }

        return defaultMessage;
    }
}