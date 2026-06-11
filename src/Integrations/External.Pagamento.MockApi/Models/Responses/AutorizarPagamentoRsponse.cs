namespace External.Pagamento.MockApi.Models.Responses
{
    public sealed class AutorizarPagamentoRsponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;
        public Guid IdPagamento { get; set; } = Guid.Empty;
    }
}
