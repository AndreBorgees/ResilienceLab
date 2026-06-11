namespace Pedido.Integrations.Sdk.Exceptions
{
    public sealed class ExternalServerErrorException : ExternalApiException
    {
        public ExternalServerErrorException(string? responseBody, int statusCode)
            : base(
                    message: "A API externa retornou erro interno.",
                    statusCode: statusCode,
                    responseBody: responseBody)
        {
        }
    }
}
