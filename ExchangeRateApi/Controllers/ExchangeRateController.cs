namespace ExchangeRateApi.Controllers
{
    [ApiController]
    [Route("ExchangeService")]
    public sealed class ExchangeRateController(IExchangeRateProvider _rates, ILogger<ExchangeRateController> _logger) : ControllerBase
    {
        [HttpPost()]
        public async Task<ActionResult<ExchangeResponse>> Convert(
            [FromBody] ExchangeRequest request,
            CancellationToken token)
        {
            _logger.LogInformation("Convert request started: {@Request}", request);
            var validationResult = ValidateRequest(request);
            if (validationResult != null)
            {
                // LOG: Validation failed, log validationResult
                return validationResult;
            }

            var inputCurrency = NormalizeCurrency(request.InputCurrency);
            var outputCurrency = NormalizeCurrency(request.OutputCurrency);

            try
            {
                // LOG: Calling rate provider, log input/output currencies
                var rate = await _rates.GetRateAsync(inputCurrency, outputCurrency, token).ConfigureAwait(false);
                if (rate <= 0)
                {
                    // LOG: Invalid rate received from provider, log rate value
                    return StatusCode(502, new { message = "Received invalid rate from provider." });
                }

                var response = CreateExchangeResponse(request, inputCurrency, outputCurrency, rate);
                // LOG: Conversion successful, log response details
                return Ok(response);
            }
            catch (OperationCanceledException)
            {
                // LOG: Request was canceled, log cancellation event
                return Problem(title: "Request canceled", statusCode: 499, detail: "The operation was canceled.");
            }
            catch (Exception ex)
            {
                // LOG: Exception occurred, log exception details
                return Problem(title: "Conversion failed", detail: ex.Message, statusCode: 500);
            }
        }

        private ActionResult? ValidateRequest(ExchangeRequest? request)
        {
            // LOG: Validating request, log request object
            if (request is null)
                return BadRequest(new { message = "Request body is required." });
            if (request.Amount <= 0)
                return BadRequest(new { message = "Amount must be > 0." });
            if (string.IsNullOrWhiteSpace(request.InputCurrency) || string.IsNullOrWhiteSpace(request.OutputCurrency))
                return BadRequest(new { message = "Both inputCurrency and outputCurrency are required." });
            if (request.InputCurrency.Trim().ToUpperInvariant() == request.OutputCurrency.Trim().ToUpperInvariant())
                return BadRequest(new { message = "inputCurrency and outputCurrency must be different." });
            return null;
        }

        private static string NormalizeCurrency(string currency) =>
            currency.Trim().ToUpperInvariant();

        private static ExchangeResponse CreateExchangeResponse(
            ExchangeRequest request, string inputCurrency, string outputCurrency, decimal rate)
        {
            // LOG: Creating ExchangeResponse, log input parameters and calculated values
            return new ExchangeResponse
            {
                Amount = request.Amount,
                InputCurrency = inputCurrency,
                OutputCurrency = outputCurrency,
                Value = request.Amount * rate
            };
        }
    }
}
