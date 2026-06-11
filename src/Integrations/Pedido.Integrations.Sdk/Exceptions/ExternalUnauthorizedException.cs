namespace Pedido.Integrations.Sdk.Exceptions
{
    public sealed class ExternalUnauthorizedException : ExternalApiException
    {
        public ExternalUnauthorizedException(string? responseBody)
            : base(
                    message: "A API externa recusou a chamada por falha de autenticação.",
                    statusCode: 401,
                    responseBody: responseBody)
        {
        }
    }
}
