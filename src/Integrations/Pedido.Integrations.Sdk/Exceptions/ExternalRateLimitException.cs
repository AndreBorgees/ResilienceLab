namespace Pedido.Integrations.Sdk.Exceptions
{
    public sealed class ExternalRateLimitException : ExternalApiException
    {
        public ExternalRateLimitException(string? responseBody)
            : base(
                    message: "A API externa recusou a chamada por excesso de requisições.",
                    statusCode: 429,
                    responseBody: responseBody)
        {
        }
    }
}
