using Core.Messages;
using FluentValidation.Results;
using MediatR;
using Pedido.Integrations.Sdk.Abstractions;
using Pedido.Integrations.Sdk.Endpoints.Pagamento;
using Pedido.Integrations.Sdk.Models.Requests;

namespace Pedido.Application.Application.Queries
{
    public sealed class PedidoStatusCommandHandler : CommandHandler, IRequestHandler<PedidoStatusCommand, ValidationResult>
    {
        private readonly IClient _client;

        public PedidoStatusCommandHandler(IClient client)
        {
            _client = client;
        }

        public async Task<ValidationResult> Handle(PedidoStatusCommand request, CancellationToken cancellationToken)
        {
            var body = new PagamentoStatusRequest
            {
                IdPagamento = Guid.NewGuid(),
            };

            var endpoint = new PagamentoStatusEndpoint(body);

            await _client.SendAsync(endpoint, cancellationToken);

            return new ValidationResult();
        }
    }
}
