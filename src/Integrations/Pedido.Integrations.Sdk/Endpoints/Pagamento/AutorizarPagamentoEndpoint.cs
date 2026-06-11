using Pedido.Integrations.Sdk.Models.Requests;
using Pedido.Integrations.Sdk.Models.Responses;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Pedido.Integrations.Sdk.Endpoints.Pagamento
{
    public sealed class AutorizarPagamentoEndpoint : EndpointBase<AutorizarPagamentoResponse>
    {
        private readonly AutorizarPagamentoRequest _autorizarPagamentoResponse;

        public AutorizarPagamentoEndpoint(AutorizarPagamentoRequest autorizarPagamentoResponse)
        {
            _autorizarPagamentoResponse = autorizarPagamentoResponse;
        }

        public override HttpMethod HttpMethod => HttpMethod.Post;

        public override bool RequiresAuthentication => true;

        public override string GetUrl() => "/pagamentos/autorizar";

        public override HttpContent? GetContent()
        {
            var json = JsonSerializer.Serialize(_autorizarPagamentoResponse);
            return new StringContent(json, System.Text.Encoding.UTF8, "application/json");
        }

        public override void ConfigureRequest(HttpRequestMessage request)
        {
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}
