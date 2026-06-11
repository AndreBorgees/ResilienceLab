using System.Net;
using System.Threading.RateLimiting;

namespace Pedido.Integrations.Sdk.Handlers
{
    public sealed class ClientSideRateLimitedHandler : DelegatingHandler
    {
        private readonly RateLimiter _rateLimiter;

        public ClientSideRateLimitedHandler(RateLimiter rateLimiter)
        {
            _rateLimiter = rateLimiter;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            using var rateLimitCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

            rateLimitCts.CancelAfter(TimeSpan.FromSeconds(90));

            RateLimitLease lease;

            try
            {
                lease = await _rateLimiter.AcquireAsync(permitCount: 1, cancellationToken: rateLimitCts.Token);
            }
            catch (OperationCanceledException) when (!cancellationToken.IsCancellationRequested) 
            {
                return new HttpResponseMessage(HttpStatusCode.TooManyRequests)
                {
                    RequestMessage = request,
                    ReasonPhrase = "Client-side rate limit timeout"
                };   
            }

            using (lease)
            {
                if (!lease.IsAcquired)
                {
                    return new HttpResponseMessage(HttpStatusCode.TooManyRequests)
                    {
                        RequestMessage = request,
                        ReasonPhrase = "Client-side falha no rate limit"
                    };
                }

                return await base.SendAsync(request, cancellationToken);
            }
        }
    }
}
