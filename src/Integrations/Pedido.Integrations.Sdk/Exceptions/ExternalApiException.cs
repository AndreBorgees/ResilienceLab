namespace Pedido.Integrations.Sdk.Exceptions
{
    public class ExternalApiException : Exception
    {
        public int StatusCode { get; }
        public string? ResponseBody { get;  }

        public ExternalApiException(
            string message,
            int statusCode, 
            string? responseBody = null) : base(message)
        {
            StatusCode = statusCode;
            ResponseBody = responseBody;
        }
    }
}
