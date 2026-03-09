namespace ExchangeRateApi.Services
{
    public class ExchangeRateProvider : IExchangeRateProvider
    {
        private readonly ExchangeRateApiOptions _options;
        private readonly ILogger _logger;
        private readonly IFlurlBase _flurlBase;

        public ExchangeRateProvider(IOptions<ExchangeRateApiOptions> opts, ILogger<ExchangeRateProvider> logger, IFlurlBase flurlBase)
        {
            _options = opts?.Value ?? throw new ArgumentNullException(nameof(opts));
            if (string.IsNullOrWhiteSpace(_options.BaseUrl))
                throw new ArgumentException("ExchangeRateApi:BaseUrl missing");
            if (string.IsNullOrWhiteSpace(_options.ApiKey))
                throw new ArgumentException("ExchangeRateApi:ApiKey missing");
            _logger = logger;
            _flurlBase = flurlBase;
        }

        public async Task<decimal> GetRateAsync(string fromCurrency, string toCurrency, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("GetRateAsync called with fromCurrency: {FromCurrency}, toCurrency: {ToCurrency}",
                    fromCurrency, toCurrency);

                if (string.IsNullOrWhiteSpace(fromCurrency) || fromCurrency.Length != 3)
                    throw new ArgumentException("fromCurrency must be a 3-letter code", nameof(fromCurrency));

                if (string.IsNullOrWhiteSpace(toCurrency) || toCurrency.Length != 3)
                    throw new ArgumentException("toCurrency must be a 3-letter code", nameof(toCurrency));

                var from = fromCurrency.Trim().ToUpperInvariant();
                var to = toCurrency.Trim().ToUpperInvariant();

                // Build the relative API path
                var path = $"{_options.Version}/{_options.ApiKey}/pair/{from}/{to}";

                // Build Flurl IFlurlRequest
                IFlurlRequest requestFunc() =>
                    _options.BaseUrl.AppendPathSegment(path)
                        .WithTimeout(TimeSpan.FromSeconds(10));

                // Execute GET with retries, Retry count can also be configured in appsettings.json
                var (response, meta) =
                    await _flurlBase.FlurlGetAsync<ExchangePairApiResponse>(
                        requestFunc,
                        retryCount: 3,
                        sleepDurationProvider: i => TimeSpan.FromSeconds(i),
                        cancellationToken);

                if (response == null)
                    throw new Exception("Empty response from ExchangeRate API");

                return response.ConversionRate;
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                _logger.LogError(ex, "Failed to retrieve exchange rate for {FromCurrency}->{ToCurrency}",
                    fromCurrency, toCurrency);
                throw new Exception("Failed to retrieve exchange rate.", ex);
            }
        } 
    }
}
