using External.Pagamento.MockApi.Models.Requests;
using External.Pagamento.MockApi.Scenarios;
using External.Pagamento.MockApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace External.Pagamento.MockApi.Controllers
{
    [ApiController]
    [Route("scenarios")]
    public sealed class ScenarioController : ControllerBase
    {
        private readonly PagamentoControleService _pagamentoControleservice;

        public ScenarioController(PagamentoControleService pagamentoControleService)
        {
            _pagamentoControleservice = pagamentoControleService;
        }

        [HttpPost("set")]
        public IActionResult SetScenario([FromBody] SetScenarioRequest request)
        {
            if (!Enum.TryParse<PagamentoScenarioType>(request.Scenario, ignoreCase: true, out var scenario))
            {
                return BadRequest(new
                {
                    message = "Cenário inválido. Use: Success, Delay, Failure, RateLimit ou Timeout."
                });
            }

            _pagamentoControleservice.SetScenario(scenario);

            return Ok(new
            {
                message = $"Cenário atualizado com sucesso. definido para: {scenario}"
            });
        }

        [HttpGet("get")]
        public IActionResult GetCurrentScenario()
        {
            var currentScenario = _pagamentoControleservice.GetScenario();
            return Ok(new
            {
                scenario = currentScenario.ToString()
            });
        }
    }
}
