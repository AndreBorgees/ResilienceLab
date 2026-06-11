namespace Pedido.Integrations.Sdk.Configurations
{
    public sealed class ConfigurationOptions
    {
        public const string SectionName = "MinhaIntegracao";

        public string BaseUrl { get; set; } = string.Empty;
        public string ApiKey { get; set; } = string.Empty;
        public int Timeout { get; set; } 
    }
}
