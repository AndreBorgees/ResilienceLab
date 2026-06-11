using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System.Text.Json;

namespace Pedido.API.Idempotency
{
    public class RedisIdempotencyService : IIdempotencyService
    {
        private readonly IDatabase _database;
        private readonly IdempotencyOptions _options;
        
        private static readonly JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

        public RedisIdempotencyService(
            IConnectionMultiplexer connectionMultiplexer, 
            IOptions<IdempotencyOptions> options)
        {
            _database = connectionMultiplexer.GetDatabase();
            _options = options.Value;
        }

        public async Task<IdempotencyRecord?> GetAsync(
            string key, CancellationToken 
            cancellationToken = default)
        {
            var redisKey = BuildRedisKey(key);

            var value = await _database.StringGetAsync(redisKey);

            if (value.IsNullOrEmpty)
                return null;

             return JsonSerializer.Deserialize<IdempotencyRecord?>(value.ToString(), jsonSerializerOptions);
        }

        public async Task CompleteAsync(
            string key, 
            string responseHash, 
            int statusCode, 
            string responseBody, 
            TimeSpan expiration, 
            CancellationToken cancellationToken = default)
        {
            var redisKey = BuildRedisKey(key);

            var record = new IdempotencyRecord
            {
                Key = key,
                RequestHash = responseHash,
                Status = IdempotencyStatus.Completed,
                StatusCode = statusCode,
                ResponsBody = responseBody,
                CreatedAt = DateTimeOffset.UtcNow,
                CompletedAt = DateTimeOffset.UtcNow
            };

            var json = JsonSerializer.Serialize(record, jsonSerializerOptions);

            await _database.StringSetAsync(redisKey, json, expiry: expiration);
        }

        public Task FailAsync(
            string key, 
            string requestHash, 
            int statusCode, 
            string responseBody, 
            TimeSpan expiration, 
            CancellationToken cancellationToken = default)
        {
            var redisKey = BuildRedisKey(key);

            var record = new IdempotencyRecord
            {
                Key = key,
                RequestHash = requestHash,
                Status = IdempotencyStatus.Failed,
                StatusCode = statusCode,
                ResponsBody = responseBody,
                CreatedAt = DateTimeOffset.UtcNow,
                CompletedAt = DateTimeOffset.UtcNow
            };

            var json = JsonSerializer.Serialize(record, jsonSerializerOptions);

            return _database.StringSetAsync(redisKey, json, expiry: expiration);
        }

        public async Task<bool> TryStartProcessingAsyn(
            string key, 
            string requestHash, 
            TimeSpan expiration, 
            CancellationToken cancellationToken = default)
        {
            var redisKey = BuildRedisKey(key);

            var record = new IdempotencyRecord
            {
                Key = key,
                RequestHash = requestHash,
                Status = IdempotencyStatus.Processing,
                CreatedAt = DateTimeOffset.Now
            };

            var json = JsonSerializer.Serialize(record, jsonSerializerOptions);

            return await _database.StringSetAsync(
                redisKey, 
                json, 
                expiry: expiration, 
                when: When.NotExists);
        }

        private string BuildRedisKey(string key) => $"{_options.KeyPrefix}:{key}";
    }
}
