using NUnit.Framework;

namespace Ekra3.BDDviaNUnit
{
    public class ScenarioAttribute : TestFixtureAttribute
    {
        public string Text { get; private set; }

        public ScenarioAttribute()
        {

        }

        public ScenarioAttribute(string text)
        {
            Text = text;
        }
    }

    public class Given : TestFixtureSetUpAttribute
    {

    }

    public class When : TestFixtureSetUpAttribute
    {

    }

    public class ThenAttribute : TestAttribute
    {

    }
}