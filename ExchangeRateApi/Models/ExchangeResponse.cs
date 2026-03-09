namespace ExchangeRateApi.Models
{
    public class ExchangeResponse
    {
        public decimal Amount { get; init; }
        public required string InputCurrency { get; init; }
        public required string OutputCurrency { get; init; }
        public decimal Value { get; init; }
        //public decimal Rate { get; init; }
        //public DateTime ExchangeTime { get; init; }
        //public int StatusCode { get; init; }
        //public string? Message { get; init; }
    }
}