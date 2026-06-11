using Pedido.Integrations.Sdk.Abstractions;

namespace Pedido.Integrations.Sdk.Endpoints
{
    public abstract class EndpointBase<TResponse> : IEndpoint<TResponse>
    {
        public abstract HttpMethod HttpMethod { get; }

        public abstract bool RequiresAuthentication { get; }

        public abstract string GetUrl();

        public virtual void ConfigureRequest(HttpRequestMessage request)
        {
            throw new NotImplementedException();
        }

        public virtual HttpContent? GetContent() => null;
    }
}
