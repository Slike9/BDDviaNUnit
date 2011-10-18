namespace Ekra3.BDDviaNUnit.OSpec
{
    public static class ScenarioStepExtension
    {
        public static void Run<TCtx>(this ThenStep<TCtx> thenStep)
        {
            new ScenarioRunner().Run(thenStep.Scenario);
        }
    }
}