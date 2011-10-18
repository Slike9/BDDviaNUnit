using NUnit.Framework;

namespace Ekra3.BDDviaNUnit.OSpec
{
    public class SomeCalcSpec : ObjectSpecification
    {
        [Feature]
        public void ShoudAddTwoNubmers()
        {
            Scenario("Adding two numbers")
                .Given("Calculator", () => new { calc = new SomeCalc(),})
                .And("Three numbers 10, 20, 30", ctx => new {ctx, a = 10, b = 20, c = 30})
               
                .When("Numbers added", ctx => new {result = ctx.ctx.calc.Add(ctx.a, ctx.b + ctx.c)})
                
                .Then("Result should be 60", ctx => Assert.That(ctx.result, Is.EqualTo(60)))
                .Run();
        }
    }
}