using Pedido.Integrations.Sdk.Models.Requests;
using Pedido.Integrations.Sdk.Models.Responses;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Pedido.Integrations.Sdk.Endpoints.Pagamento
{
    public sealed class PagamentoStatusEndpoint : EndpointBase<PagamentoStatusResponse>
    {
        private readonly PagamentoStatusRequest _pagamentoStatusRequest;
        public PagamentoStatusEndpoint(PagamentoStatusRequest pagamentoStatusRequest)
        {
            _pagamentoStatusRequest = pagamentoStatusRequest;
        }

        public override HttpMethod HttpMethod => HttpMethod.Get;

        public override bool RequiresAuthentication => true;

        public override string GetUrl() => $"pagamentos/{_pagamentoStatusRequest.IdPagamento}";

        public override HttpContent? GetContent()
        {
            var json = JsonSerializer.Serialize(_pagamentoStatusRequest);
            return new StringContent(json, System.Text.Encoding.UTF8, "application/json");
        }

        public override void ConfigureRequest(HttpRequestMessage request)
        {
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}
