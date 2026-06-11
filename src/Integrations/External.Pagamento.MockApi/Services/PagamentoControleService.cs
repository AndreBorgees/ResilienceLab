using External.Pagamento.MockApi.Scenarios;

namespace External.Pagamento.MockApi.Services
{
    public sealed class PagamentoControleService
    {
        private readonly PagamentoScenarioState _scenarioState;

        public PagamentoControleService(PagamentoScenarioState scenarioState)
        {
            _scenarioState = scenarioState;
        }

        public PagamentoScenarioType GetScenario()
        {
            return _scenarioState.CurrentScenario;
        }

        public void SetScenario(PagamentoScenarioType scenario)
        {
            _scenarioState.CurrentScenario = scenario;
        }

        public async Task AplicarScenarioAsync(CancellationToken cancellationToken = default)
        {
            switch (_scenarioState.CurrentScenario)
            {
                case PagamentoScenarioType.Success:
                    return;
                case PagamentoScenarioType.Failure:
                    throw new InvalidOperationException("Simulação de falha no processamento do pagamento.");
                case PagamentoScenarioType.Timeout:
                    await Task.Delay(TimeSpan.FromMinutes(1), cancellationToken);
                    return;
                case PagamentoScenarioType.Retry:
                    throw new TimeoutSimuladoException("Simulação de falha - retry");
                default:
                    return;
            }
        }
    }

    public sealed class TooManyRequestsException : Exception
    {
        public TooManyRequestsException(string message) : base(message)
        {
        }
    }
    public sealed class TimeoutSimuladoException : Exception
    {
        public TimeoutSimuladoException(string message) : base(message) { }
    }
}
