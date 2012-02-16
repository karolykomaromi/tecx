namespace TecX.Unity.Error
{
    using System;

    using Microsoft.Practices.ObjectBuilder2;

    using TecX.Common;

    public class ReportCreationErrorStrategy : BuilderStrategy
    {
        private readonly Action<Exception, IBuilderContext, IBuildPlanPolicy> report;

        public ReportCreationErrorStrategy(Action<Exception, IBuilderContext, IBuildPlanPolicy> report)
        {
            Guard.AssertNotNull(report, "report");

            this.report = report;
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

            if (!(plan is ReportCreationErrorBuildPlanPolicy))
            {
                IBuildPlanPolicy policy = new ReportCreationErrorBuildPlanPolicy(plan, this.report);

                buildPlanLocation.Set(policy, context.BuildKey);
            }
        }
    }
}