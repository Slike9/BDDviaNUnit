using System;

namespace Ekra3.BDDviaNUnit.OSpec
{
    public abstract class ScenarioStep
    {
        #region Properties
        
        public string Title { get; private set; }

        internal ScenarioStep PreviousStep { get; private set; }

        internal ScenarioStep NextStep { get; private set; }

        internal ScenarioStep Scenario
        {
            get
            {
                return PreviousStep == null ? this : PreviousStep.Scenario;
            }
        }

        internal ScenarioStepType StepType { get; set; }

        #endregion

        #region ctor
        internal ScenarioStep(ScenarioStepType stepType, string title, ScenarioStep previousStep)
        {
            StepType = stepType;
            Title = title;
            PreviousStep = previousStep;
            if (previousStep != null)
                previousStep.NextStep = this;
        } 
        #endregion

        #region Methods
        internal virtual object Execute(object ctx)
        {
            return ctx;
        } 
        #endregion
    }

    public class Scenario : ScenarioStep
    {
        #region ctor
        public Scenario(string title)
            : base(ScenarioStepType.Scenario, title, null)
        {
        } 
        #endregion

        public GivenStep<object, TOut> Given<TOut>(string str, Func<TOut> func)
        {
            return new GivenStep<object, TOut>(str, _ => func(), this);
        }
    }

    public abstract class ScenarioStep<TCtxIn, TCtxOut> : ScenarioStep
    {
        private readonly Func<TCtxIn, TCtxOut> _contextFunc;

        #region ctor
        internal ScenarioStep(ScenarioStepType stepType, string title, Func<TCtxIn, TCtxOut> contextFunc, ScenarioStep prevStep)
            : base(stepType, title, prevStep)
        {
            _contextFunc = contextFunc;
        } 
        #endregion

        internal override object Execute(object ctx)
        {
            return _contextFunc((TCtxIn)ctx);
        }
    }

    public class GivenStep<TCtxIn, TCtxOut> : ScenarioStep<TCtxIn, TCtxOut>
    {
        internal GivenStep(string title, Func<TCtxIn, TCtxOut> func, ScenarioStep prevStep, bool isFirst = true)
            : base(isFirst ? ScenarioStepType.Given : ScenarioStepType.AndGiven, title, func, prevStep)
        {

        }

        public GivenStep<TCtxOut, TOut2> And<TOut2>(string title, Func<TCtxOut, TOut2> func)
        {
            return new GivenStep<TCtxOut, TOut2>(title, func, this, false);
        }

        public GivenStep<TCtxOut, TCtxOut> And(string title, Action<TCtxOut> action)
        {
            return new GivenStep<TCtxOut, TCtxOut>(title, ctx => { action(ctx); return ctx; }, this, false);
        }

        public WhenStep<TCtxOut, TCtxOut2> When<TCtxOut2>(string title, Func<TCtxOut, TCtxOut2> func)
        {
            return new WhenStep<TCtxOut, TCtxOut2>(title, func, this);
        }

        public WhenStep<TCtxOut, TCtxOut> When(string title, Action<TCtxOut> action)
        {
            return new WhenStep<TCtxOut, TCtxOut>(title, ctx => { action(ctx); return ctx; }, this);
        }

        public ThenStep<TCtxOut> Then(string str, Action<TCtxOut> action)
        {
            return new ThenStep<TCtxOut>(str, ctx => { action(ctx); return ctx; }, this);
        }
    }

    public class WhenStep<TCtxIn, TCtxOut> : ScenarioStep<TCtxIn, TCtxOut>
    {
        internal WhenStep(string title, Func<TCtxIn, TCtxOut> func, ScenarioStep prevStep, bool isFirst = true)
            : base(isFirst ? ScenarioStepType.When : ScenarioStepType.AndWhen, title, func, prevStep)
        {

        }

        public WhenStep<TCtxOut, TCtxOut2> And<TCtxOut2>(string str, Func<TCtxOut, TCtxOut2> func)
        {
            return new WhenStep<TCtxOut, TCtxOut2>(str, func, this, false);
        }

        public WhenStep<TCtxOut, TCtxOut> And(string str, Action<TCtxOut> action)
        {
            return new WhenStep<TCtxOut, TCtxOut>(str, ctx => { action(ctx); return ctx; }, this, false);
        }

        public ThenStep<TCtxOut> Then(string str, Action<TCtxOut> action)
        {
            return new ThenStep<TCtxOut>(str, ctx => { action(ctx); return ctx; }, this);
        }
    }

    public class ThenStep<TCtx> : ScenarioStep<TCtx, TCtx>
    {
        internal ThenStep(string title, Func<TCtx, TCtx> func, ScenarioStep prevStep)
            : base(ScenarioStepType.Then, title, func, prevStep)
        {

        }

        public WhenStep<TCtx, TCtxOut2> When<TCtxOut2>(string str, Func<TCtx, TCtxOut2> func)
        {
            return new WhenStep<TCtx, TCtxOut2>(str, func, this);
        }

        public ThenStep<TCtx> Then(string str, Action<TCtx> action)
        {
            return new ThenStep<TCtx>(str, ctx => { action(ctx); return ctx; }, this);
        }

        public WhenStep<TCtx, TCtxOut2> ModifyContext<TCtxOut2>(Func<TCtx, TCtxOut2> func)
        {
            return new WhenStep<TCtx, TCtxOut2>(string.Empty, func, this);
        }
    }
}