using Pedido.API.Configuration;
using Pedido.API.Idempotency;
using Pedido.Integrations.Sdk.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddApiConfig();
builder.Services.AddSwaggerConfig();
builder.Services.RegisterServices();

builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(typeof(Program).Assembly);
});

builder.Services.AddMinhaIntegracaoSdk(builder.Configuration);

builder.Services.AddIdempotency(builder.Configuration);

var app = builder.Build();

app.UseSwaggerConfig();
app.UseApiConfig();
