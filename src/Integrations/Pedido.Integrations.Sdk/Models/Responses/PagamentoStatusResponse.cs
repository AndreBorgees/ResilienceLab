namespace Pedido.Integrations.Sdk.Models.Responses
{
    public sealed class PagamentoStatusResponse
    {
        public Guid IdPagamento { get; set; } = Guid.Empty;
        public string Status { get; set; } = string.Empty;
        public decimal? Valor { get; set; }
    }
}
