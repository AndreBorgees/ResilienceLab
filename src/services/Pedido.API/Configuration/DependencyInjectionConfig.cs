using Core.Mediator;
using FluentValidation.Results;
using MediatR;
using Pedido.Application.Application.Commands;
using Pedido.Application.Application.Queries;

namespace Pedido.API.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            // Commands
            services.AddScoped<IRequestHandler<InserirPedidoCommand, ValidationResult>, PedidoCommandHandler>();
            services.AddScoped<IRequestHandler<PedidoStatusCommand, ValidationResult>, PedidoStatusCommandHandler>();
            
            // Application
            services.AddScoped<IMediatorHandler, MediatorHandler>();
        }
    }
}
