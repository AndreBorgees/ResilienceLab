namespace Pedido.Integrations.Sdk.Abstractions
{
    public interface IEndpoint<TResponse>
    {
        HttpMethod HttpMethod { get; }

        bool RequiresAuthentication { get; }

        string GetUrl();

        HttpContent? GetContent();

        void ConfigureRequest(HttpRequestMessage request); 
    }
}
