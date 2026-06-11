namespace Pedido.Integrations.Sdk.Abstractions
{
    public interface IAuthenticationProvider
    {
        public Task<string> GetTokenAsync(CancellationToken ct = default);
    }
}
