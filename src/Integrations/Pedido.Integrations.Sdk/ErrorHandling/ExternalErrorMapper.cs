using Pedido.Integrations.Sdk.Exceptions;
using System.Net;

namespace Pedido.Integrations.Sdk.ErrorHandling
{
    public static class ExternalErrorMapper
    {
        public static async Task ThrowAsync(
            HttpResponseMessage response,
            CancellationToken cancellationToken)
        {
            var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);

            throw response.StatusCode switch
            {
                HttpStatusCode.TooManyRequests =>
                    new ExternalRateLimitException(responseBody),

                HttpStatusCode.InternalServerError
                or HttpStatusCode.BadGateway
                or HttpStatusCode.GatewayTimeout =>
                    new ExternalServerErrorException(
                        statusCode: (int)response.StatusCode,
                        responseBody: responseBody),

                HttpStatusCode.Unauthorized =>
                    new ExternalUnauthorizedException(responseBody),

                HttpStatusCode.ServiceUnavailable =>
                    new ExternalServiceUnavailableException(responseBody),

                _ =>
                    new ExternalApiException(
                        message: "A API externa retornou um erro.",
                        statusCode: (int)response.StatusCode,
                        responseBody: responseBody)
            };
        }
    }
}
