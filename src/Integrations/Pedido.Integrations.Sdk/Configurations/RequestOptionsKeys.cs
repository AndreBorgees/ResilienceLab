namespace Pedido.Integrations.Sdk.Configurations
{
    public static class RequestOptionsKeys
    {
        public static readonly HttpRequestOptionsKey<bool> RequiresAuthentication = 
            new("RequiresAuthentication");
    }
}
