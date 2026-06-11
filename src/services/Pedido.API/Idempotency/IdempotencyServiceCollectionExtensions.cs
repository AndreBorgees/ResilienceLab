using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace Pedido.API.Idempotency
{
    public static class IdempotencyServiceCollectionExtensions
    {
        public static IServiceCollection AddIdempotency(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<IdempotencyOptions>(configuration.GetSection("Idempotency"));

            services.AddSingleton<IConnectionMultiplexer>(serviceProvider =>
            {
                var options = serviceProvider
                .GetRequiredService<IOptions<IdempotencyOptions>>()
                .Value;

                return ConnectionMultiplexer.Connect(options.RedisConnectionString);
            });

            services.AddScoped<IdempotencyFilter>();

            services.AddScoped<IIdempotencyService, RedisIdempotencyService>();

            return services;
        }
    }
}
