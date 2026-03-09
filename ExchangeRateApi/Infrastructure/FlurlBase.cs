namespace ExchangeRateApi.Infrastructure
{
    public class FlurlBase(ILogger<FlurlBase> logger) : IFlurlBase
    {
        //We can configure this in the Appsettings.json and read it in constructor if needed. For now, hardcoding the values which are generally worth retrying
        private static readonly List<int> httpStatusCodesWorthRetrying = [408, 500, 502, 503, 504, 429];

        private static bool IsWorthRetrying(FlurlHttpException ex)
        {
            return httpStatusCodesWorthRetrying.Contains(ex.Call.Response.StatusCode);
        }

        private AsyncRetryPolicy CreateRetryPolicy(int retryCount, Func<int, TimeSpan> sleepDurationProvider)
        {
            return Policy
                .Handle<FlurlHttpTimeoutException>()
                .Or((FlurlHttpException exception) => IsWorthRetrying(exception))
                .WaitAndRetryAsync(retryCount, sleepDurationProvider,
                (result, span, rc) => logger.LogError($"Initiate retry span of {span} - retry count- {rc} Result-> {result}"));
        }

        public async Task<(TResponse, GenericHttpResponse)> FlurlGetAsync<TResponse>(Func<IFlurlRequest> requestFunc, int retryCount, Func<int, TimeSpan>? sleepDurationProvider, CancellationToken cancellationToken)
        {
            try
            {
                async Task<TResponse> Get()
                {
                    var response = await requestFunc().GetJsonAsync<TResponse>(cancellationToken: cancellationToken);
                    return response;
                }
                if (retryCount <= 0 || sleepDurationProvider is null)
                {
                    return (await Get(), GenericHttpResponse.BuildSuccessResponse());
                }

                var waitAndRetryPolicy = CreateRetryPolicy(retryCount, sleepDurationProvider);

                logger.LogDebug("FlurlGetAsync ended:Success");
                return await waitAndRetryPolicy.ExecuteAsync(async () => (await Get(), GenericHttpResponse.BuildSuccessResponse()));
            }
            catch (FlurlHttpTimeoutException ex)
            {
                var errorMessage = await ex.GetResponseStringAsync();
                return (default!, GenericHttpResponse.BuildErrorResponse($"Request timed out - {errorMessage}", ex.StatusCode.GetValueOrDefault((int)HttpStatusCode.RequestTimeout)));
            }
            catch (FlurlHttpException ex)
            {
                var errorMessage = await ex.GetResponseStringAsync();
                return (default!, GenericHttpResponse.BuildErrorResponse(errorMessage, ex.StatusCode.GetValueOrDefault((int)HttpStatusCode.InternalServerError)));
            }
            catch (Exception ex)
            {
                return (default!, GenericHttpResponse.BuildErrorResponse(ex.Message, HttpStatusCode.InternalServerError));
            }
        }

        //Add post, put, delete methods as needed in future
    }
}
