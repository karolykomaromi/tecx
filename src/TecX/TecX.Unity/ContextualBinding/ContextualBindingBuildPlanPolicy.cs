using System;

using Microsoft.Practices.ObjectBuilder2;

using TecX.Common;

namespace TecX.Unity.ContextualBinding
{
    public class ContextualBindingBuildPlanPolicy : IBuildPlanPolicy
    {
        private readonly Type _to;
        private readonly Type _from;
        private readonly Func<IRequest, bool> _shouldResolve;
        private readonly IRequestHistory _history;

        public ContextualBindingBuildPlanPolicy Next { get; set; }

        public IBuildPlanPolicy LastChance { get; set; }

        public ContextualBindingBuildPlanPolicy(Type to, Type from, Func<IRequest, bool> shouldResolve,
                                                IRequestHistory history)
        {
            Guard.AssertNotNull(to, "to");
            Guard.AssertNotNull(from, "from");
            Guard.AssertNotNull(shouldResolve, "shouldResolve");
            Guard.AssertNotNull(history, "history");

            _to = to;
            _from = from;
            _shouldResolve = shouldResolve;
            _history = history;
        }

        #region Implementation of IBuildPlanPolicy

        /// <summary>
        /// Creates an instance of this build plan's type, or fills
        ///             in the existing type if passed in.
        /// </summary>
        /// <param name="context">Context used to build up the object.</param>
        public void BuildUp(IBuilderContext context)
        {
            if (context.Existing == null)
            {
                //the top of the stack contains the buildup request
                //we are currently working on, which is not interesting here
                if (_to == _history.Peek().Context.BuildKey.Type)
                    _history.Pop();

                if (_history.Count > 0)
                {
                    IRequest request = _history.Peek();

                    if (_shouldResolve(request))
                    {
                        context.Existing = context.NewBuildUp(new NamedTypeBuildKey(_from));
                    }
                }
            }

            if (context.Existing == null)
            {
                if (Next != null)
                {
                    Next.BuildUp(context);
                }
                else if (LastChance != null)
                {
                    LastChance.BuildUp(context);
                }
            }
        }

        #endregion
    }
}