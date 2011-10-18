using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Ekra3.BDDviaNUnit.OSpec
{
    public class ScenarioRunner
    {
        public void Run(ScenarioStep scenario)
        {
            var exceptions = new List<Exception>();
            object ctx = null;
            for (var step = scenario; step != null; step = step.NextStep)
            {
                var indentLength = 0;
                var prefix = "";
                var underlineChar = '`';
                switch (step.StepType)
                {
                    case ScenarioStepType.Scenario:
                        indentLength = 0;
                        prefix = "Scenario: ";
                        underlineChar = '=';
                        break;
                    case ScenarioStepType.Given:
                        indentLength = 0;
                        prefix = "Given: ";
                        underlineChar = '~';
                        break;
                    case ScenarioStepType.AndGiven:
                        indentLength = 0;
                        prefix = "And given";
                        underlineChar = '~';
                        break;
                    case ScenarioStepType.ButGiven:
                        indentLength = 0;
                        prefix = "But given";
                        underlineChar = '~';
                        break;
                    case ScenarioStepType.When:
                        indentLength = 2;
                        prefix = "When";
                        underlineChar = '^';
                        break;
                    case ScenarioStepType.AndWhen:
                        indentLength = 2;
                        prefix = "And when";
                        break;
                    case ScenarioStepType.Then:
                        indentLength = 2;
                        prefix = "Then";
                        break;
                }
                var indent = "".PadLeft(indentLength);

                var stepPassed = true;
                try
                {
                    ctx = step.Execute(ctx);
                }
                catch (SuccessException)
                {

                }
                catch (Exception e)
                {
                    stepPassed = false;
                    ConsoleHelper.WriteLineUnderlining(underlineChar, "X: {0}{1} {2}", indent, prefix, step.Title);
                    Console.WriteLine("{0}", e.Message);
                    Console.WriteLine("{0}", e.StackTrace);
                    Console.WriteLine();
                    if (step.StepType != ScenarioStepType.Then)
                        throw;
                    exceptions.Add(e);
                }

                if (step.StepType == ScenarioStepType.Scenario)
                {
                    ConsoleHelper.WriteLineUnderlining(underlineChar, "{0}{1} {2}", indent, prefix, step.Title);
                    Console.WriteLine();
                }
                else
                {
                    if (stepPassed)
                    {
                        ConsoleHelper.WriteLineUnderlining(underlineChar, "V: {0}{1} {2}", indent, prefix, step.Title);
                    }
                }
            }

            Console.WriteLine("".PadLeft(30, '-'));

            if (exceptions.Any())
                throw new AggregateException(exceptions);
        }
    }
}