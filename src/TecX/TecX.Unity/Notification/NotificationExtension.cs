namespace TecX.Unity.Notification
{
    using System;

    using Microsoft.Practices.ObjectBuilder2;
    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.ObjectBuilder;

    using TecX.Common;

    public class NotificationExtension : UnityContainerExtension
    {
        private Action<Exception, IBuilderContext, IBuildPlanPolicy> report;

        public NotificationExtension()
            : this((ex, ctx, policy) => { })
        {
        }

        public NotificationExtension(Action<Exception, IBuilderContext, IBuildPlanPolicy> report)
        {
            Guard.AssertNotNull(report, "handler");

            this.Report = report;
        }

        public event EventHandler<CreatingObjectEventArgs> Creating = delegate { };
 
        public event EventHandler<CreatedObjectEventArgs> Created = delegate { }; 

        public Action<Exception, IBuilderContext, IBuildPlanPolicy> Report
        {
            get
            {
                return this.report;
            }

            set
            {
                Guard.AssertNotNull(value, "Report");

                this.report = value;
            }
        }

        protected override void Initialize()
        {
            IBuilderStrategy strategy = new NotificationStrategy(this);

            this.Context.Strategies.Add(strategy, UnityBuildStage.PreCreation);
        }

        private class NotificationStrategy : BuilderStrategy
        {
            private readonly NotificationExtension extension;

            public NotificationStrategy(NotificationExtension extension)
            {
                Guard.AssertNotNull(extension, "extension");

                this.extension = extension;
            }

            public override void PreBuildUp(IBuilderContext context)
            {
                // this code is mostly taken from Unity's BuildPlanStrategy
                // we need to find the current IBuildPlanPolicy which is not available until after the BuildPlanStrategy ran
                // so we need to perform its actions here
                IPolicyList buildPlanLocation;

                IBuildPlanPolicy plan = context.Policies.Get<IBuildPlanPolicy>(context.BuildKey, out buildPlanLocation);
                if (plan == null ||

                    // OverriddenBuildPlanMarkerPolicy is not accessible here so the name matching is a workaround
                    string.Equals(plan.GetType().Name, "OverriddenBuildPlanMarkerPolicy", StringComparison.InvariantCultureIgnoreCase))
                {
                    IPolicyList creatorLocation;

                    IBuildPlanCreatorPolicy planCreator = context.Policies.Get<IBuildPlanCreatorPolicy>(context.BuildKey, out creatorLocation);
                    if (planCreator != null)
                    {
                        plan = planCreator.CreatePlan(context, context.BuildKey);

                        buildPlanLocation = buildPlanLocation ?? creatorLocation;
                    }
                }

                if (!(plan is NotificationBuildPlanPolicy))
                {
                    IBuildPlanPolicy policy = new NotificationBuildPlanPolicy(plan, this.extension);

                    buildPlanLocation.Set(policy, context.BuildKey);
                }
            }
        }

        private class NotificationBuildPlanPolicy : IBuildPlanPolicy
        {
            private readonly IBuildPlanPolicy inner;

            private readonly NotificationExtension extension;

            public NotificationBuildPlanPolicy(IBuildPlanPolicy inner, NotificationExtension extension)
            {
                Guard.AssertNotNull(inner, "inner");
                Guard.AssertNotNull(extension, "extension");

                this.inner = inner;
                this.extension = extension;
            }

            public void BuildUp(IBuilderContext context)
            {
                try
                {
                    this.extension.Creating(this.extension, new CreatingObjectEventArgs());
                    this.inner.BuildUp(context);
                    this.extension.Created(this.extension, new CreatedObjectEventArgs());
                }
                catch (Exception ex)
                {
                    this.extension.Report(ex, context, this.inner);

                    // Unity's infrastructure relies on this exception so don't consume it!
                    throw;
                }
            }
        }
    }

    public class CreatedObjectEventArgs : EventArgs
    {
    }

    public class CreatingObjectEventArgs : EventArgs
    {
    }
}
