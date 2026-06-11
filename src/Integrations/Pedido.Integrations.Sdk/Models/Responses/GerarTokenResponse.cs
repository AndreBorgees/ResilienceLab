namespace Pedido.Integrations.Sdk.Models.Responses
{
    public sealed class GerarTokenResponse
    {
        public string AccessToken { get; set; } = string.Empty;
        public int ExpiresIn { get; set; }
    }
}
