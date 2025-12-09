namespace Tudormobile.AlphaVantage.entities;

public class AlphaVantageResponse<T> where T : class
{
    public string? ErrorMessage { get; set; }
    public T? Result { get; set; }
    public bool IsSuccess => string.IsNullOrEmpty(ErrorMessage) && Result != null;
}
