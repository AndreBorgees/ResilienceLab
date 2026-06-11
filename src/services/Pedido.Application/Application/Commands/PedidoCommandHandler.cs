using Core.Messages;
using FluentValidation.Results;
using MediatR;
using Pedido.Integrations.Sdk.Abstractions;
using Pedido.Integrations.Sdk.Endpoints.Pagamento;
using Pedido.Integrations.Sdk.Models.Requests;

namespace Pedido.Application.Application.Commands
{
    public class PedidoCommandHandler : CommandHandler, IRequestHandler<InserirPedidoCommand, ValidationResult>
    {

        private readonly IClient _client;

        public PedidoCommandHandler(IClient client)
        {
            _client = client;
        }

        public async Task<ValidationResult> Handle(InserirPedidoCommand request, CancellationToken cancellationToken)
        {
            var body = new AutorizarPagamentoRequest
            {
                IdPedido = Guid.NewGuid().ToString("N"),
                Valor = request.Valor,
            };

            var endpoint = new AutorizarPagamentoEndpoint(body);

            var pedido = await _client.SendAsync(endpoint, cancellationToken);

            return new ValidationResult();
        }
    }
}
