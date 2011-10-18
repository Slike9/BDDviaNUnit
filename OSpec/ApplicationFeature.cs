using System;
using NUnit.Framework;

namespace Ekra3.BDDviaNUnit.OSpec
{
    [TestFixture]
    public class ApplicationFeature
    {
        public class AspectAttribute : TestAttribute
        {
        }

        public class ScenarioAttribute : TestAttribute
        {
        }


        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            Description();
        }

        protected virtual void Description()
        {
            
        }

        protected void InOrder(string text)
        {
            Console.WriteLine("In order {0}", text);
        }

        protected void AsA(string text)
        {
            Console.WriteLine("As a {0}", text);
        }

        protected void AsAn(string text)
        {
            Console.WriteLine("As an {0}", text);
        }

        protected void Want(string text)
        {
            Console.WriteLine("Want {0}", text);
        }

        protected void SoThat(string text)
        {
            Console.WriteLine("So that {0}", text);
        }

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

        protected Scenario<object> Given(string title)
        {
            return Given(title, (Func<object>)null);
        }

        protected void Passed<TContext>(TContext context)
        {

        }

        protected void Pending<TContext>(TContext context)
        {
            Assert.Fail("The step is pending");
        }

        protected void Failed<TContext>(TContext context)
        {
            Assert.Fail("The step is failed");
        }

        protected Func<object> GivenPassed()
        {
            return () => (object) null;
        }

        protected Func<object> GivenFailed()
        {
            Assert.Fail("Given faild");
// ReSharper disable HeuristicUnreachableCode
            return () => (object)null;
// ReSharper restore HeuristicUnreachableCode
        }

        protected void AspectPending()
        {
            Assert.Fail("Aspect pending");
        }

        protected void AspectFailed()
        {
            Assert.Fail("Aspect pending");
        }

    }
}