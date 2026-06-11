namespace Pedido.Integrations.Sdk.Abstractions
{
    public interface IClient
    {
        Task<TResponse?> SendAsync<TResponse>(
            IEndpoint<TResponse> endpoint,
            CancellationToken ct = default);
    }
}
