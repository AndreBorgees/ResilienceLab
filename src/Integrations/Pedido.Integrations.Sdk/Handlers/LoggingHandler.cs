using Microsoft.Extensions.Logging;

namespace Pedido.Integrations.Sdk.Handlers
{
    public sealed class LoggingHandler : DelegatingHandler
    {
        private readonly ILogger<LoggingHandler> _logger;

        public LoggingHandler(ILogger<LoggingHandler> logger)
        {
            _logger = logger;
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, 
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("Enviando requisição para {Url} com método {Method}", request.RequestUri, request.Method);

            var response = await base.SendAsync(request, cancellationToken);

            _logger.LogInformation("Recebida resposta com status {StatusCode} para {Url}", 
                (int)response.StatusCode, request.RequestUri);

            return response;
        }
    }
}
