using Pedido.Integrations.Sdk.Abstractions;
using Pedido.Integrations.Sdk.Configurations;
using Pedido.Integrations.Sdk.ErrorHandling;
using System.Text.Json;

namespace Pedido.Integrations.Sdk.Clients
{
    public sealed class Client : IClient, IAuthClient
    {
        private readonly HttpClient _httpClient;
        
        public Client(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<TResponse?> SendAsync<TResponse>(IEndpoint<TResponse> endpoint, CancellationToken ct = default)
        {
            var request = new HttpRequestMessage(
                endpoint.HttpMethod,
                endpoint.GetUrl());

            request.Options.Set(RequestOptionsKeys.RequiresAuthentication, endpoint.RequiresAuthentication);

            var content = endpoint.GetContent();
            if (content is not null)
                request.Content = content;

            endpoint.ConfigureRequest(request);

            var response = await _httpClient.SendAsync(request, ct);

            if (!response.IsSuccessStatusCode)
                await ExternalErrorMapper.ThrowAsync(response, ct);

            var json = await response.Content.ReadAsStringAsync();

            if(string.IsNullOrWhiteSpace(json))
                return default!;

            return JsonSerializer.Deserialize<TResponse>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            })!;
        }
    }
}
