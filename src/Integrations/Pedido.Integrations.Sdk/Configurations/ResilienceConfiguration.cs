using Polly;
using Polly.Extensions.Http;
using System.Net;

namespace Pedido.Integrations.Sdk.Configurations
{
    public sealed class ResilienceConfiguration
    {
        public static IAsyncPolicy<HttpResponseMessage> GetRetryPolcy(int retryCount)
        {
           return HttpPolicyExtensions
                .HandleTransientHttpError()
                .WaitAndRetryAsync(retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                 (outcome, delay, retryAttempt, context) =>
                 {
                     System.Diagnostics.Debug.WriteLine(
                         $"Retry {retryAttempt} após {delay.TotalSeconds}s. Erro: {outcome.Exception?.GetType().Name}");
                 });
        }

        public static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy(int handledEventsAllowedBeforeBreaking, TimeSpan durationOfBreak)
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .CircuitBreakerAsync(handledEventsAllowedBeforeBreaking, TimeSpan.FromSeconds(durationOfBreak.TotalSeconds));
        }
    }
}
