namespace External.Pagamento.MockApi.Models.Requests
{
    public sealed class AutorizarPagamentoRequest
    {
        public string IdPedido { get; set; } = string.Empty;
        public decimal Valor { get; set; }
    }
}
