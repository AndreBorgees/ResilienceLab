using Pedido.Integrations.Sdk.Abstractions;
using Pedido.Integrations.Sdk.Configurations;
using System.Net.Http.Headers;

namespace Pedido.Integrations.Sdk.Handlers
{
    public sealed class AuthHandler : DelegatingHandler
    {
        private readonly IAuthenticationProvider _authenticationProvider;

        public AuthHandler(IAuthenticationProvider authenticationProvider)
        {
            _authenticationProvider = authenticationProvider;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request.Options.TryGetValue(RequestOptionsKeys.RequiresAuthentication, out var requiresAuth) && requiresAuth is true)
            {
                var token = await _authenticationProvider.GetTokenAsync(cancellationToken);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
