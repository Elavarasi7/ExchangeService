namespace ExchangeRateApi.Interfaces
{
    public interface IFlurlBase 
    {
         Task<(TResponse, GenericHttpResponse)> FlurlGetAsync<TResponse>(Func<IFlurlRequest> requestFunc, int retryCount, Func<int, TimeSpan>? sleepDurationProvider, CancellationToken cancellationToken);

        //Add post, put, delete methods as needed in future
    }
}
