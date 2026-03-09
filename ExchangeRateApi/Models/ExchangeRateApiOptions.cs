namespace ExchangeRateApi.Models
{
    public class ExchangeRateApiOptions
    {
        public required string BaseUrl { get; init; }
        public string? ApiKey { get; init; }
        public string? Version { get; init; } 

    }
}
