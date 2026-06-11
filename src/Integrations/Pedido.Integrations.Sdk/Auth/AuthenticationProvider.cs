using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Pedido.Integrations.Sdk.Abstractions;
using Pedido.Integrations.Sdk.Configurations;
using Pedido.Integrations.Sdk.Endpoints.Autenticacao;
using Pedido.Integrations.Sdk.Models.Requests;

namespace Pedido.Integrations.Sdk.Auth
{
    public sealed class AuthenticationProvider : IAuthenticationProvider
    {
        private const string CacheKey = "MinhaIntegracao:AccessToken";

        private readonly IAuthClient _clientAuth;
        private readonly ConfigurationOptions _options;
        private readonly IMemoryCache _memoryCache;

        public AuthenticationProvider(IAuthClient clientAuth, 
            IOptions<ConfigurationOptions> options,
            IMemoryCache memoryCache)
        {
            _clientAuth = clientAuth;
            _options = options.Value;
            _memoryCache = memoryCache;
        }

        public async Task<string> GetTokenAsync(CancellationToken ct = default)
        {
            if (_memoryCache.TryGetValue(CacheKey, out string? cachedToken) &&
                !string.IsNullOrEmpty(cachedToken))
            {
                return cachedToken;
            }

            var endpoint = new GerarTokenEndpoint(new GerarTokenRequest
            {
                ApiKey = _options.ApiKey
            });

            var response = await _clientAuth.SendAsync(endpoint, ct);

            if (response is null || string.IsNullOrWhiteSpace(response.AccessToken))
                throw new InvalidOperationException("Erro ao obter token");

            var expiresInSeconds = response.ExpiresIn > 60
                ? response.ExpiresIn - 60
                : response.ExpiresIn; 

            _memoryCache.Set(CacheKey, response.AccessToken, TimeSpan.FromSeconds(expiresInSeconds));

            return response.AccessToken;
        }
    }
}
