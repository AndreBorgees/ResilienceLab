namespace Pedido.API.Idempotency
{
    public interface IIdempotencyService
    {
        Task<IdempotencyRecord?> GetAsync(
            string key,
            CancellationToken cancellationToken = default);

        Task<bool> TryStartProcessingAsyn(
            string key,
            string requestHash,
            TimeSpan expiration,
            CancellationToken cancellationToken = default);

        Task CompleteAsync(
            string key,
            string responseHash,
            int statusCode,
            string responseBody,
            TimeSpan expiration,
            CancellationToken cancellationToken = default);

        Task FailAsync(
            string key,
            string requestHash,
            int statusCode,
            string responseBody,
            TimeSpan expiration,
            CancellationToken cancellationToken = default);
    }
}
