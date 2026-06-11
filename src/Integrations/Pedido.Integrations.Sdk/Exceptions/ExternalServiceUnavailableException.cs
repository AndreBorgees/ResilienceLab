namespace Pedido.Integrations.Sdk.Exceptions
{
    public sealed class ExternalServiceUnavailableException : ExternalApiException
    {
        public ExternalServiceUnavailableException(string? responseBody)
            : base(
                    message: "A API externa está temporariamente indisponível.",
                    statusCode: 503,
                    responseBody: responseBody)
        {
        }
    }
}
