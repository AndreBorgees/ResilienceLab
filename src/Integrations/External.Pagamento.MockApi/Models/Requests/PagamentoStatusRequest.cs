namespace External.Pagamento.MockApi.Models.Requests
{
    public sealed class PagamentoStatusRequest
    {
        public Guid IdPagamento { get; set; } = Guid.Empty;
    }
}
