using System;
using NUnit.Framework;

namespace Ekra3.BDDviaNUnit.OSpec
{
    [TestFixture]
    public class ObjectSpecification
    {
        protected Scenario Scenario(string title = "")
        {
            return new Scenario(title);
        }

        protected GivenStep<object, TCtx> Given<TCtx>(string title, Func<TCtx> ctxFunc)
        {
            return Scenario().Given(title, ctxFunc);
        }
    }

    public class FeatureAttribute : TestAttribute
    {
    }
}