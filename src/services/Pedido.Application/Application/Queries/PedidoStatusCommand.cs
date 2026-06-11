using Core.Messages;
using FluentValidation;

namespace Pedido.Application.Application.Queries
{
    public sealed class PedidoStatusCommand : Command
    {
        public Guid IdPagamento { get; set; } = Guid.Empty;

        public override bool IsValid()
        {
            ValidationResult = new PedidoStatusCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }

        public class PedidoStatusCommandValidation : AbstractValidator<PedidoStatusCommand>
        {
            public PedidoStatusCommandValidation()
            {
                RuleFor(x => x.IdPagamento)
                    .NotEmpty().WithMessage("O ID do pagamento é obrigatório.");
            }
        }
    }
}
