using System;
using NUnit.Framework;

namespace Ekra3.BDDviaNUnit.OSpec
{
    [TestFixture]
    public class ComponentFeatureSpecification
    {
        protected void Scenario<TContext>(string title, Scenario<TContext> scenario)
            where TContext : class
        {
            scenario.Title = title;
            scenario.Execute();
        }

        protected void Scenario<TContext>(Scenario<TContext> scenario)
            where TContext : class
        {
            Scenario(null, scenario);
        }

        protected Scenario<TContext> Given<TContext>(string title, Func<TContext> context)
            where TContext : class
        {
            return new Scenario<TContext>(title, context);
        }
   
    }

    public class ScenarioAttribute : TestAttribute
    {
    }
}