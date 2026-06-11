using Pedido.Integrations.Sdk.Models.Requests;
using Pedido.Integrations.Sdk.Models.Responses;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Pedido.Integrations.Sdk.Endpoints.Autenticacao
{
    public sealed class GerarTokenEndpoint : EndpointBase<GerarTokenResponse>
    {
        private readonly GerarTokenRequest _request;

        public GerarTokenEndpoint(GerarTokenRequest request)
        {
            _request = request;
        }

        public override HttpMethod HttpMethod => HttpMethod.Post;

        public override bool RequiresAuthentication => false;

        public override string GetUrl() => "oauth/token";

        public override HttpContent? GetContent()
        {
            var json = JsonSerializer.Serialize(_request);
            return new StringContent(json, System.Text.Encoding.UTF8, "application/json");
        }

        public override void ConfigureRequest(HttpRequestMessage request)
        {
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}
