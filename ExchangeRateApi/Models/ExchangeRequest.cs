namespace ExchangeRateApi.Models
{
    public class ExchangeRequest
    {
        public decimal Amount { get; init; }
        public required string InputCurrency { get; init; }
        public required string OutputCurrency { get; init; }
    }
}
