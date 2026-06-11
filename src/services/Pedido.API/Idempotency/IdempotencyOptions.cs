namespace Pedido.API.Idempotency
{
    public sealed class IdempotencyOptions
    {
        public string RedisConnectionString { get; set; } = "localhost:6379";
        public TimeSpan Expiration { get; set; } = TimeSpan.FromMinutes(5);
        public string HeaderName { get; set; } = "Idempotency-Key";
        public string KeyPrefix { get; set; } = "idempotency";
    }
}
