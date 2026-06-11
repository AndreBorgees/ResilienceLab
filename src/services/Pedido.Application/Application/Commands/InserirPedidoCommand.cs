using Core.Messages;
using FluentValidation;

namespace Pedido.Application.Application.Commands
{
    public class InserirPedidoCommand : Command
    {
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public decimal Valor { get; set; }

        public override bool IsValid()
        {
            ValidationResult = new InserirPedidoCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
        
        public class InserirPedidoCommandValidation : AbstractValidator<InserirPedidoCommand>
        {
            public InserirPedidoCommandValidation()
            {
                RuleFor(x => x.Nome)
                    .NotEmpty().WithMessage("O nome do pedido é obrigatório.")
                    .MaximumLength(100).WithMessage("O nome do pedido deve conter no máximo 100 caracteres.");
                RuleFor(x => x.Descricao)
                    .NotEmpty().WithMessage("A descrição do pedido é obrigatória.")
                    .MaximumLength(500).WithMessage("A descrição do pedido deve conter no máximo 500 caracteres.");
                RuleFor(x => x.Valor)
                   .GreaterThan(0).WithMessage("O valor do pedido deve ser maior que zero.");
            }
        }
    }
}
