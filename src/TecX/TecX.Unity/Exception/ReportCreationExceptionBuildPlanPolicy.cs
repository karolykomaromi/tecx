namespace TecX.Unity.Exception
{
    using System;

    using Microsoft.Practices.ObjectBuilder2;

    using TecX.Common;

    public class ReportCreationExceptionBuildPlanPolicy : IBuildPlanPolicy
    {
        private readonly IBuildPlanPolicy inner;

        private readonly Action<Exception, IBuilderContext, IBuildPlanPolicy> report;

        public ReportCreationExceptionBuildPlanPolicy(IBuildPlanPolicy inner, Action<Exception, IBuilderContext, IBuildPlanPolicy> report)
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