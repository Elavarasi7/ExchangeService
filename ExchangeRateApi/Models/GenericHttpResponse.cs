namespace ExchangeRateApi.Models;

public class GenericHttpResponse
{
    public string? Message { get; set; }

    public bool IsSuccess { get; set; }

    public HttpStatusCode ResponseCode { get; set; }

    public static GenericHttpResponse BuildSuccessResponse(string message, HttpStatusCode statusCode)
    {
        return new GenericHttpResponse
        {
            Message = message,
            IsSuccess = true,
            ResponseCode = statusCode
        };
    }


    public static GenericHttpResponse BuildSuccessResponse()
    {
        return BuildSuccessResponse("Ok", HttpStatusCode.OK);
    }

    public static GenericHttpResponse BuildIgnoreResponse(string message)
    {
        return new GenericHttpResponse
        {
            Message = message,
            IsSuccess = false,
            ResponseCode = HttpStatusCode.PreconditionFailed
        };
    }

    public static GenericHttpResponse BuildErrorResponse(string message, HttpStatusCode httpStatusCode)
    {
        return new GenericHttpResponse
        {
            Message = message,
            IsSuccess = false,
            ResponseCode = httpStatusCode
        };
    }

    public static GenericHttpResponse BuildErrorResponse(string message, int statusCode)
    {
        var httpStatusCode = (HttpStatusCode)Enum.Parse(typeof(HttpStatusCode), statusCode.ToString());

        return BuildErrorResponse(message, httpStatusCode);
    }
}
