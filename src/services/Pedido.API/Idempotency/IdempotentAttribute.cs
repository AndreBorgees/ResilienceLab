using Microsoft.AspNetCore.Mvc;

namespace Pedido.API.Idempotency
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public sealed class IdempotentAttribute : TypeFilterAttribute
    {
        public IdempotentAttribute() : base(typeof(IdempotencyFilter))
        {
        }
    }
}
