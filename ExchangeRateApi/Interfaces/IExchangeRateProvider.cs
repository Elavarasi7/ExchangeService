namespace ExchangeRateApi.Interfaces;

public interface IExchangeRateProvider
{
   public Task<decimal> GetRateAsync(string fromCurrency, string toCurrency, CancellationToken cancellationToken);
}