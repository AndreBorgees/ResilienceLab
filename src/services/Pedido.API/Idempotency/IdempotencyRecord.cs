namespace Pedido.API.Idempotency
{
    public sealed class IdempotencyRecord
    {
        public string Key { get; init; } = string.Empty;
        public string RequestHash { get; init; } = string.Empty;
        public IdempotencyStatus Status { get; init; }
        public int? StatusCode { get; init; }
        public string? ResponsBody { get; init; }
        public DateTimeOffset? CreatedAt { get; init; }
        public DateTimeOffset? CompletedAt { get; init; }
    }
}
