using Tudormobile.AlphaVantage;
namespace SimpleConsoleApp;

internal class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");

        var apiKey = "demo"; // Replace with your actual API key
        var httpClient = new HttpClient();
        var client = new AlphaVantageClient(apiKey, httpClient);

        var function = AlphaVantageFunction.GLOBAL_QUOTE;
        var symbol = "IBM";
        var jsonTask = client.GetJsonStringAsync(function, symbol);

        jsonTask.Wait();

        Console.WriteLine(jsonTask.Result);
    }
}

/* Program Output:
Hello, World!
{
"Global Quote": {
    "01. symbol": "IBM",
    "02. open": "125.0000",
    "03. high": "126.5000",
    "04. low": "124.5000",
    "05. price": "125.7500",
    "06. volume": "3500000",
    "07. latest trading day": "2023-10-20",
    "08. previous close": "124.8000",
    "09. change": "0.9500",
    "10. change percent": "0.7615%"
}
}
*/
