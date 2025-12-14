using System.Text.Json;

namespace Tudormobile.AlphaVantage.Extensions;

internal class EntityParser
{
    internal static decimal GetDecimalValue(JsonElement element, string propertyName, decimal defaultValue = 0m)
        => GetPropertyValue(element, propertyName, ParseDecimal, defaultValue);

    internal static double GetDoubleValue(JsonElement element, string propertyName, double defaultValue = 0)
    => GetPropertyValue(element, propertyName, ParseDouble, defaultValue);

    internal static DateOnly GetDateOnlyValue(JsonElement element, string propertyName, DateOnly defaultValue = default)
        => GetPropertyValue(element, propertyName, ParseDateOnly, defaultValue);

    internal static TimeOnly GetTimeOnlyValue(JsonElement element, string propertyName, TimeOnly defaultValue = default)
    => GetPropertyValue(element, propertyName, ParseTimeOnly, defaultValue);

    internal static long GetLongValue(JsonElement element, string propertyName, long defaultValue = 0L)
        => GetPropertyValue(element, propertyName, ParseLong, defaultValue);

    internal static int GetIntValue(JsonElement element, string propertyName, int defaultValue = 0)
        => GetPropertyValue(element, propertyName, ParseInt, defaultValue);

    internal static int? GetNullableIntValue(JsonElement element, string propertyName)
        => GetPropertyValue(element, propertyName, ParseNullableInt, default(int?));

    internal static string GetStringValue(JsonElement element, string propertyName, string defaultValue = "")
        => GetPropertyValue(element, propertyName, ParseString, defaultValue);

    internal static TEnum GetEnumValue<TEnum>(JsonElement element, string propertyName, TEnum defaultValue) where TEnum : struct, Enum
    {
        if (element.TryGetProperty(propertyName, out JsonElement propertyValue))
        {
            var stringValue = propertyValue.GetString();
            if (!string.IsNullOrEmpty(stringValue) && Enum.TryParse<TEnum>(stringValue, true, out var enumValue))
            {
                return enumValue;
            }
        }
        return defaultValue;
    }

    internal static T GetPropertyValue<T>(JsonElement element, string propertyName, Func<JsonElement, T, T> parseFunction, T defaultValue)
        => element.TryGetProperty(propertyName, out JsonElement propertyValue) ? parseFunction(propertyValue, defaultValue) : defaultValue;

    internal static string ParseString(JsonElement element, string defaultValue)
        => element.GetString() ?? defaultValue;

    internal static decimal ParseDecimal(JsonElement element, decimal defaultValue)
        => decimal.TryParse(element.GetString(), out var result) ? result : defaultValue;

    internal static double ParseDouble(JsonElement element, double defaultValue)
    => double.TryParse(element.GetString(), out var result) ? result : defaultValue;

    internal static DateOnly ParseDateOnly(JsonElement element, DateOnly defaultValue)
        => DateOnly.TryParse(element.GetString(), out var result) ? result : defaultValue;

    internal static long ParseLong(JsonElement element, long defaultValue)
        => long.TryParse(element.GetString(), out var result) ? result : defaultValue;

    internal static int ParseInt(JsonElement element, int defaultValue)
    => int.TryParse(element.GetString(), out var result) ? result : defaultValue;

    internal static int? ParseNullableInt(JsonElement element, int? defaultValue)
    {
        if (element.ValueKind == JsonValueKind.Null) return null;
        _ = decimal.TryParse(element.GetString(), out var result);
        return (int)result;
    }

    internal static TimeOnly ParseTimeOnly(JsonElement element, TimeOnly defaultValue)
        => TimeOnly.TryParse(element.GetString(), out var result) ? result : defaultValue;
}
