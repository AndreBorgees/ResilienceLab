using External.Pagamento.MockApi.Models.Requests;
using External.Pagamento.MockApi.Models.Responses;
using External.Pagamento.MockApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace External.Pagamento.MockApi.Controllers
{
    [ApiController]
    [Route("pagamentos")]
    public sealed class PagamentoController : ControllerBase
    {
        private readonly PagamentoControleService _pagamentoControleService;

        public PagamentoController(PagamentoControleService pagamentoControleService)
        {
            _pagamentoControleService = pagamentoControleService;
        }

        [HttpPost]
        [Route("autorizar")]
        public async Task<IActionResult> AutorizarPagamento(
            [FromBody] AutorizarPagamentoRequest request,
            CancellationToken cancellationToken)
        {
            try
            {
                await _pagamentoControleService.AplicarScenarioAsync(cancellationToken);

                var response = new AutorizarPagamentoRsponse
                {
                    Success = true,
                    Status = "Autorizado",
                    Message = "Pagamento autorizado com sucesso.",
                    IdPagamento = Guid.NewGuid(),
                };

                return Ok(response);
            }
            catch (TooManyRequestsException ex)
            {
                return StatusCode(StatusCodes.Status429TooManyRequests, new
                {
                    message = ex.Message
                });
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new
                {
                    message = ex.Message
                });
            }
            catch (TimeoutSimuladoException ex)
            {
                return StatusCode(408, new { message = ex.Message });
            }
        }

        [HttpGet("{idPagamento}")]
        public async Task<IActionResult> BuscarStatusPagamento(
            [FromRoute] Guid idPagamento,
            CancellationToken cancellationToken)
        {
            try
            {
                await _pagamentoControleService.AplicarScenarioAsync(cancellationToken);

                var response = new PagamentoStatusResponse
                {
                    IdPagamento = idPagamento,
                    Status = "Autorizado",
                    Valor = 100.00m
                };

                return Ok(response);
            }
            catch (TooManyRequestsException ex)
            {
                return StatusCode(StatusCodes.Status429TooManyRequests, new
                {
                    message = ex.Message
                });
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new
                {
                    message = ex.Message
                });
            }
        }
    }
}
