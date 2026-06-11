using System.Net;
using Core.Mediator;
using Microsoft.AspNetCore.Mvc;
using Pedido.API.Idempotency;
using Pedido.Application.Application.Commands;
using WebAPI.Core.Controllers;

namespace Pedido.Application.Controllers
{
    public class PedidoController : BaseController
    {
        private readonly IMediatorHandler _mediatorHandler;

        public PedidoController(IMediatorHandler mediatorHandler)
        {
            _mediatorHandler = mediatorHandler;
        }

        [HttpPost("pedidos")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [Idempotent]
        public async Task<IActionResult> InserirPedido([FromBody] InserirPedidoCommand request)
        {
            var result = await _mediatorHandler.SendCommand(request);

            return Ok(result);
        }
    }
}
