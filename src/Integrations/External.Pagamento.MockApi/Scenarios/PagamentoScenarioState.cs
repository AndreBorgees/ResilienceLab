namespace External.Pagamento.MockApi.Scenarios
{
    public sealed class PagamentoScenarioState
    {
        public PagamentoScenarioType CurrentScenario { get; set; } = PagamentoScenarioType.Success;

        public void SetScenario(PagamentoScenarioType scenarioType)
        {
            CurrentScenario = scenarioType;
        }
    }
}
