using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Ekra3.BDDviaNUnit.OSpec
{
    public class Scenario<TContext>
        where TContext : class
    {
        #region Fields
        
        private readonly Func<TContext> _contextFactory;

        private readonly List<ScenarioStep<TContext>> _steps = new List<ScenarioStep<TContext>>();

        private readonly string _contextTitle; 
        
        #endregion

        #region Properties
        public string Title { get; set; } 
        #endregion

        #region ctor
        internal Scenario(string contextTitle, Func<TContext> contextFactory)
        {
            _contextTitle = contextTitle;
            _contextFactory = contextFactory;
        } 
        #endregion

        #region Public Methods
        public void Execute()
        {
            var pendingStepsExist = false;
            var exceptions = new List<Exception>();
            if (!string.IsNullOrEmpty(Title))
            {
                var scenarioTitle = string.Format("Scenario: {0}", Title);
                ConsoleHelper.WriteLineUnderlining('=', scenarioTitle.Length, "{0}", scenarioTitle);
                Console.WriteLine();
            }

            ConsoleHelper.WriteLineUnderlining('~', "Given {0}", _contextTitle);
            TContext context = null;
            var contextFactoryCalledSomeTime = false;
            foreach (var step in _steps)
            {
                if (context == null)
                {
                    if (contextFactoryCalledSomeTime)
                    {
                        context = _contextFactory != null ? _contextFactory() : null;
                    }
                    else
                    {
                        contextFactoryCalledSomeTime = true;
                        if (_contextFactory != null)
                        {
                            try
                            {
                                context = _contextFactory();
                            }
                            catch (SuccessException)
                            {

                            }
                            catch (Exception e)
                            {
                                Console.WriteLine("{0}--> ERROR: ", "");
                                Console.WriteLine("{0}", e.Message);
                                Console.WriteLine("{0}", e.StackTrace);
                                Console.WriteLine();
                                throw;
                            }
                            Console.WriteLine("{0}++> Passed", "");
                        }
                        else
                        {
                            Console.WriteLine("{0}??> Pending", "");
                            pendingStepsExist = true;
                        }
                        Console.WriteLine();
                    }
                }

                var indentLength = 0;
                var prefix = "";
                var underlineChar = '`';
                switch (step.StepType)
                {
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
                ConsoleHelper.WriteLineUnderlining(underlineChar, "{0}{1} {2}", indent, prefix, step.Name);

                if (step.Action == null)
                {
                    Console.WriteLine("{0}~~> Pending", indent);
                    pendingStepsExist = true;
                }
                else
                {
                    var stepPassed = true;
                    try
                    {
                        step.Action(context);
                    }
                    catch(SuccessException)
                    {
                            
                    }
                    catch(Exception e)
                    {
                        stepPassed = false;
                        Console.WriteLine("{0}--> ERROR: ", indent);
                        Console.WriteLine("{0}", e.Message);
                        Console.WriteLine("{0}", e.StackTrace);
                        Console.WriteLine();
                        if (step.StepType != ScenarioStepType.Then)
                            throw;
                        exceptions.Add(e);
                    }
                    if (stepPassed)
                        Console.WriteLine("{0}++> Passed", indent);
                }
                Console.WriteLine();
            }
            Console.WriteLine("".PadLeft(30, '-'));

            if (exceptions.Any())
                throw new AggregateException(exceptions);

            if (pendingStepsExist)
                Assert.Fail("Pending steps exist.");
        } 
        #endregion

        #region Internal Methods
        
        internal void AddStep(ScenarioStep<TContext> scenarioStep)
        {
            _steps.Add(scenarioStep);
        }

        internal void AddStep(ScenarioStepType scenarioStepType, string title, Action<TContext> action)
        {
            AddStep(new ScenarioStep<TContext>(scenarioStepType, title, action));
        } 

        #endregion
    }

    class ScenarioStep<TContext>
    {
        public ScenarioStepType StepType { get; private set; }
        public string Name { get; set; }
        public Action<TContext> Action { get; private set; }

        public ScenarioStep(ScenarioStepType scenarioStepType, string name, Action<TContext> action = null)
        {
            StepType = scenarioStepType;
            Name = name;
            Action = action;
        }
    }
}