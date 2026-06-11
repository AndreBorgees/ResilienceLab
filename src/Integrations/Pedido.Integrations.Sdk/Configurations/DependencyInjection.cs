using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Pedido.Integrations.Sdk.Abstractions;
using Pedido.Integrations.Sdk.Auth;
using Pedido.Integrations.Sdk.Clients;
using Pedido.Integrations.Sdk.Handlers;
using System.Threading.RateLimiting;

namespace Pedido.Integrations.Sdk.Configurations
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddMinhaIntegracaoSdk(
        this IServiceCollection services,
        IConfiguration configuration)
        {
            services.Configure<ConfigurationOptions>(configuration.GetSection(ConfigurationOptions.SectionName));

            services.AddMemoryCache();

            services.AddTransient<LoggingHandler>();
            services.AddTransient<AuthHandler>();
            services.AddTransient<ClientSideRateLimitedHandler>();

            services.AddScoped<IAuthenticationProvider, AuthenticationProvider>();

            services.AddHttpClient<IClient, Client>()
                .ConfigureHttpClient((serviceProvider, client) =>
                {
                    var options = serviceProvider
                    .GetRequiredService<IOptions<ConfigurationOptions>>().Value;

                    ConfigureHttpClient(client, options);
                })
                .AddHttpMessageHandler<LoggingHandler>()
                .AddHttpMessageHandler<ClientSideRateLimitedHandler>()
                .AddHttpMessageHandler<AuthHandler>()
                .AddPolicyHandler(ResilienceConfiguration.GetRetryPolcy(retryCount: 3))
                .AddPolicyHandler(ResilienceConfiguration.GetCircuitBreakerPolicy(handledEventsAllowedBeforeBreaking: 5, durationOfBreak: TimeSpan.FromSeconds(30)));

            services.AddHttpClient<IAuthClient, Client>()
                .ConfigureHttpClient((serviceProvider, client) =>
                {
                    var options = serviceProvider
                    .GetRequiredService<IOptions<ConfigurationOptions>>().Value;

                    ConfigureHttpClient(client, options);
                })
                .AddHttpMessageHandler<LoggingHandler>();

            services.AddSingleton<RateLimiter>(_ =>
            {
                return new TokenBucketRateLimiter(new TokenBucketRateLimiterOptions
                {
                    TokenLimit = 1,
                    TokensPerPeriod = 1,
                    ReplenishmentPeriod = TimeSpan.FromSeconds(40),
                    QueueLimit = 1,
                    QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                    AutoReplenishment = true,
                });
            });

            return services;
        }

        private static void ConfigureHttpClient(
        HttpClient client,
        ConfigurationOptions options)
        {
            client.BaseAddress = new Uri(options.BaseUrl);
            client.Timeout = TimeSpan.FromSeconds(options.Timeout);
        }
    }
}
