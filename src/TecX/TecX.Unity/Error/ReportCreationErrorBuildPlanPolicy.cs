namespace TecX.Unity.Error
{
    using System;

    using Microsoft.Practices.ObjectBuilder2;

    using TecX.Common;

    public class ReportCreationErrorBuildPlanPolicy : IBuildPlanPolicy
    {
        private readonly IBuildPlanPolicy inner;

        private readonly Action<Exception, IBuilderContext, IBuildPlanPolicy> report;

        public ReportCreationErrorBuildPlanPolicy(IBuildPlanPolicy inner, Action<Exception, IBuilderContext, IBuildPlanPolicy> report)
        {
            Guard.AssertNotNull(inner, "inner");
            Guard.AssertNotNull(report, "report");

            this.inner = inner;
            this.report = report;
        }

        public void BuildUp(IBuilderContext context)
        {
            try
            {
                this.inner.BuildUp(context);
            }
            catch (Exception ex)
            {
                this.report(ex, context, this.inner);

                // Unity's infrastructure relies on this exception so don't consume it!
                throw;
            }
        }
    }
}